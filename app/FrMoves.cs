using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CapriDesktop
{
    // Capri FR's own moves and offers. The moves reuse her existing art: glitch, walk, perch and riding the cursor. The offers
    // come from a fixed list, never from what's on screen or from a brain, so a web page can't talk her into acting; a yes
    // sends the fixed request through her usual path, with her usual Security checks.
    internal sealed partial class Companion
    {
        partial void FrDemoTick(double now);

        private const int LeapMark = int.MinValue;
        private Point leapFrom, leapTo;
        private double leapStart, leapTime, leapArc;
        private Action leapThen;
        private Ledge peekSeat;
        private Point peekBack;
        private double peekUntil;
        private bool chaseRide;
        private double chaseUntil;

        internal const int MoveWander = 0, MoveTeleport = 1, MoveLeap = 2, MovePeek = 3, MoveChase = 4;

        // A plain walk or hop stays her most usual move, so the showier ones keep some surprise.
        internal static int PickMove(double roll) { return roll < 0.4 ? MoveWander : roll < 0.6 ? MoveTeleport : roll < 0.8 ? MoveLeap : roll < 0.9 ? MovePeek : MoveChase; }

        // Along the line from one spot to the other, with a parabola on top of it that's arc pixels high halfway.
        internal static Point LeapPoint(Point from, Point to, double arc, double t)
        {
            t = Math.Max(0, Math.Min(1, t));
            return new Point((int)Math.Round(from.X + (to.X - from.X) * t), (int)Math.Round(from.Y + (to.Y - from.Y) * t - arc * 4 * t * (1 - t)));
        }

        internal static double LeapDuration(double distance, int size) { return Math.Max(0.45, Math.Min(1.1, 0.35 + distance / Math.Max(1, size * 6.0))); }

        // Which side of her screen she peeks from: never one that opens onto another monitor, where she'd show whole.
        // 0 is left, 1 is right, -1 means neither side is an outer edge.
        internal static int PeekSide(bool leftOuter, bool rightOuter, double roll)
        {
            if (leftOuter && rightOuter) return roll < 0.5 ? 0 : 1;
            return leftOuter ? 0 : rightOuter ? 1 : -1;
        }

        private bool FrFree { get { return IsHandleCreated && preferences.Motion && !hidden && !dragging && !pressed && !riding && !onPanel && !falling && !hanging && !walking && !lying && outro == null; } }

        // One of her special moves in place of a plain wander, or false to leave it to Wander. Nothing showy over something full screen.
        private bool FancyMove()
        {
            if (preferences.NoFancyMoves || !FrFree || !WanderArea().IsEmpty) return false;
            switch (PickMove(random.NextDouble()))
            {
                case MoveTeleport: return TeleportMove();
                case MoveLeap: return LeapMove();
                case MovePeek: return PeekOut();
                case MoveChase: return ChaseCursor();
                default: return false;
            }
        }

        // She glitches out and drops in from high up somewhere else, on another monitor about half the time when there is one.
        private bool TeleportMove()
        {
            Point hip = Hip();
            Screen[] screens = Screen.AllScreens;
            Screen here = Screen.FromPoint(hip), there = here;
            if (screens.Length > 1 && random.NextDouble() < 0.5)
            {
                List<Screen> others = new List<Screen>();
                foreach (Screen screen in screens) if (!screen.Equals(here)) others.Add(screen);
                if (others.Count > 0) there = others[random.Next(others.Count)];
            }
            Rectangle area = there.WorkingArea;
            int margin = Math.Min(preferences.Size, area.Width / 4);
            Teleport(new Point(area.Left + margin + random.Next(Math.Max(1, area.Width - 2 * margin)), area.Top + (int)(area.Height * (0.15 + random.NextDouble() * 0.3))));
            return true;
        }

        // A jump in an arc onto the top edge of another window, or back to the taskbar if it closed on the way.
        private bool LeapMove()
        {
            if (preferences.NoSitting) return false;
            List<Ledge> spots = Spots();
            if (spots.Count == 0) return false;
            Ledge spot = WanderSpot(spots, preferences.Size, random.NextDouble());
            int margin = preferences.Size / 4;
            int x = spot.Left + margin + random.Next(Math.Max(1, spot.Right - spot.Left - 2 * margin));
            StartLeap(Seated(x, spot.Y), Math.Max(preferences.Size * 0.6, 90), delegate
            {
                Land(spot);
                if (seat != spot) DropSoon();
            });
            return true;
        }

        private void StartLeap(Point to, double arc, Action then)
        {
            if (seat != null) { Point hip = Hip(); anchor = Standing(hip.X, seat.Y); }
            seat = null;
            lying = false;
            facingLeft = to.X < anchor.X;
            double dx = to.X - anchor.X, dy = to.Y - anchor.Y;
            leapFrom = anchor;
            leapTo = to;
            leapArc = arc;
            leapThen = then;
            leapStart = clock.Elapsed.TotalSeconds;
            leapTime = LeapDuration(Math.Sqrt(dx * dx + dy * dy), preferences.Size);
            walking = true;
            walkTo = LeapMark;
            Play(walk);
            frames.Start();
        }

        private void LeapStep(double now)
        {
            double t = leapTime <= 0 ? 1 : (now - leapStart) / leapTime;
            if (t < 1) { anchor = LeapPoint(leapFrom, leapTo, leapArc, t); return; }
            Action then = leapThen;
            leapThen = null;
            anchor = leapTo;
            walking = false;
            walkTo = 0;
            leanAngle = 0;
            if (then != null) then(); else DropSoon();
        }

        // She glitches to an outer edge of her screen and sits half off it, watching for a few seconds, then glitches back.
        private bool PeekOut()
        {
            Point hip = Hip();
            Rectangle area = Screen.FromPoint(hip).WorkingArea;
            int y = area.Top + (int)(area.Height * (0.3 + random.NextDouble() * 0.4));
            bool leftOuter = !OnAScreen(new Point(area.Left - 4, y)), rightOuter = !OnAScreen(new Point(area.Right + 4, y));
            int side = PeekSide(leftOuter, rightOuter, random.NextDouble());
            if (side < 0) return false;
            int x = side == 1 ? area.Right : area.Left;
            Ledge spot = new Ledge { Left = x - 8, Right = x + 8, Y = y, Floor = true, Free = true };
            double stay = 6 + random.NextDouble() * 4;
            GlitchOut(delegate
            {
                peekBack = hip;
                Hop(spot, x);
                if (seat != spot) return;
                // Land keeps her fully on screen, so she's moved after: the middle of her body on the screen's edge, half off it.
                anchor = Seated(x, y);
                Rectangle body = BodyOnScreen(anchor, PerchedBody);
                anchor.X += x - (body.Left + body.Width / 2);
                facingLeft = side == 1;
                peekSeat = spot;
                peekUntil = clock.Elapsed.TotalSeconds + stay;
            }, false);
            return true;
        }

        private static bool OnAScreen(Point point)
        {
            foreach (Screen screen in Screen.AllScreens) if (screen.Bounds.Contains(point)) return true;
            return false;
        }

        // She jumps at their cursor and rides it for a few seconds. Clicks pass through her while she rides, as ever.
        private bool ChaseCursor()
        {
            if (preferences.Hologram) return false;
            Point cursor = Cursor.Position;
            Rectangle area = Screen.FromPoint(cursor).WorkingArea;
            Point target = Within(new Point(cursor.X - PaddingSize - (int)(preferences.Size * 0.267f) + 12, cursor.Y - PaddingSize - (int)(preferences.Size * 0.73f) - 12), area, PerchedBody);
            double stay = 6 + random.NextDouble() * 4;
            StartLeap(target, Math.Max(preferences.Size * 0.4, 60), delegate
            {
                if (hidden || dragging) { DropSoon(); return; }
                StartRide(true);
                if (!riding) { DropSoon(); return; }
                chaseRide = true;
                chaseUntil = clock.Elapsed.TotalSeconds + stay;
            });
            return true;
        }

        // Ends a peek or a chase when its time is up. Once they've moved her themselves, it's theirs and is left alone.
        private void CheckFrMoves(double now)
        {
            if (peekSeat != null)
            {
                if (seat != peekSeat) peekSeat = null;
                else if (now >= peekUntil && outro == null && !dragging && !pressed)
                {
                    peekSeat = null;
                    Teleport(peekBack);
                }
            }
            if (chaseRide)
            {
                if (!riding) chaseRide = false;
                else if (now >= chaseUntil)
                {
                    chaseRide = false;
                    StopRide();
                    DropSoon();
                }
            }
        }

        private ToolStripMenuItem FrMoveItem(string name, Func<bool> move)
        {
            return new ToolStripMenuItem(name, null, delegate
            {
                WakeRest(true);
                if (!FrFree || !move()) Say("Pas maintenant, je ne peux pas.");
            });
        }

        private void AddFrMoves(ToolStripMenuItem moves)
        {
            ToolStripMenuItem fancy = new ToolStripMenuItem("Mouvements spéciaux en balade");
            fancy.Checked = !preferences.NoFancyMoves;
            fancy.Click += delegate
            {
                preferences.NoFancyMoves = !preferences.NoFancyMoves;
                fancy.Checked = !preferences.NoFancyMoves;
                SavePreferences();
            };
            moves.DropDownItems.Add(fancy);
            moves.DropDownItems.Add(FrMoveItem("Se téléporter", TeleportMove));
            moves.DropDownItems.Add(FrMoveItem("Sauter sur une fenêtre", LeapMove));
            moves.DropDownItems.Add(FrMoveItem("Faire coucou depuis le bord", PeekOut));
            moves.DropDownItems.Add(FrMoveItem("Attraper le curseur", ChaseCursor));
        }

        private ToolStripMenuItem FrOffersItem()
        {
            ToolStripMenuItem item = new ToolStripMenuItem("Me faire des propositions");
            item.Checked = !preferences.NoInitiative;
            item.Click += delegate
            {
                preferences.NoInitiative = !preferences.NoInitiative;
                item.Checked = !preferences.NoInitiative;
                offerOpen = null;
                SavePreferences();
            };
            return item;
        }

        // Her offers: a line and either a fixed request sent as if they'd asked, or something of her own.
        private sealed class Offer
        {
            internal string Line;
            internal string Request;
            internal Func<bool> Act;
            internal double Until;
        }

        internal const int OfferTrick = 0, OfferChess = 1, OfferMusic = 2, OfferBreak = 3;
        private const double OfferLife = 60;
        private Offer offerOpen;
        private double nextOffer = -1;
        private int offerRefusals;
        private int lastOffer = -1;

        internal static int OfferPick(bool hasBrain, bool musicPlaying, bool longSession, int last, double roll)
        {
            List<int> kinds = new List<int> { OfferTrick, OfferChess };
            if (hasBrain && !musicPlaying) kinds.Add(OfferMusic);
            if (hasBrain && longSession) kinds.Add(OfferBreak);
            kinds.Remove(last);
            return kinds[Math.Max(0, Math.Min(kinds.Count - 1, (int)(roll * kinds.Count)))];
        }

        // Seconds until her next offer: 30 to 60 minutes, doubling for each no in a row, four hours at most.
        internal static double OfferGap(int refusals, double roll) { return Math.Min(240, (30 + 30 * roll) * (1 << Math.Max(0, Math.Min(refusals, 3)))) * 60; }

        private static readonly HashSet<string> OfferYes = new HashSet<string> { "oui", "ouais", "ok", "okay", "d accord", "daccord", "vas y", "vasy", "go", "yes", "yep", "yeah", "carrement", "bien sur", "oui stp", "oui merci", "oui vas y", "allez", "volontiers", "avec plaisir" };
        private static readonly HashSet<string> OfferNo = new HashSet<string> { "non", "nan", "no", "nope", "pas maintenant", "plus tard", "non merci", "non pas maintenant", "laisse", "laisse tomber", "pas envie", "une autre fois" };

        // 1 for a plain yes, -1 for a plain no and 0 for anything else, which is an ordinary message that drops the offer.
        internal static int OfferReply(string text)
        {
            string plain = (text ?? "").Normalize(NormalizationForm.FormD);
            StringBuilder letters = new StringBuilder();
            foreach (char c in plain) if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) letters.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : ' ');
            string words = Regex.Replace(Regex.Replace(letters.ToString(), @"\b(?:hey |salut )?capris?\b", " "), @"\s+", " ").Trim();
            return OfferYes.Contains(words) ? 1 : OfferNo.Contains(words) ? -1 : 0;
        }

        private bool OfferWaiting { get { return offerOpen != null && clock.Elapsed.TotalSeconds < offerOpen.Until; } }

        private void CheckOffer(double now, long quiet)
        {
            if (offerOpen != null && now >= offerOpen.Until) { offerOpen = null; offerRefusals++; nextOffer = now + OfferGap(offerRefusals, random.NextDouble()); }
            if (preferences.NoInitiative) return;
            if (nextOffer < 0) nextOffer = now + (15 + random.NextDouble() * 15) * 60;
            if (now < nextOffer || offerOpen != null) return;
            if (quiet > 120000 || Hushed || ownRest || preferences.Quiet || hidden || GamingOn || CallModeOn || SleepBusy || InChat || typer.Visible || menu.Visible || watchAlong != null || PcEars.Active || LineBusy || now < voiceUntil || speaker.Busy || riding || falling || outro != null || !WanderArea().IsEmpty) return;
            nextOffer = now + OfferGap(offerRefusals, random.NextDouble());
            int kind = OfferPick(brain.Remote != null || brain.Local != null || brain.Model != null, musicOn, now >= 90 * 60, lastOffer, random.NextDouble());
            lastOffer = kind;
            offerOpen = MakeOffer(kind, now);
            Remark("initiative", offerOpen.Line);
        }

        private Offer MakeOffer(int kind, double now)
        {
            Offer offer = new Offer { Until = now + OfferLife };
            switch (kind)
            {
                case OfferChess: offer.Line = "On fait une partie d'échecs ? Dis oui ou non."; offer.Request = "play chess"; break;
                case OfferMusic: offer.Line = "Je te mets un peu de musique ? Dis oui ou non."; offer.Request = "lance un peu de musique"; break;
                case OfferBreak: offer.Line = "Tu bosses depuis un moment. Je te rappelle de faire une pause dans 10 minutes ? Dis oui ou non."; offer.Request = "rappelle-moi de faire une pause dans 10 minutes"; break;
                default: offer.Line = "Je m'ennuie un peu. Je te montre un tour ? Dis oui ou non."; offer.Act = delegate { return FrFree && (PeekOut() || TeleportMove()); }; break;
            }
            return offer;
        }

        // True when their line answered her offer and needs nothing more. A yes to a request swaps in that fixed request.
        private bool OfferAnswered(ref string text)
        {
            if (offerOpen == null) return false;
            Offer offer = offerOpen;
            offerOpen = null;
            if (clock.Elapsed.TotalSeconds >= offer.Until) return false;
            int reply = OfferReply(text);
            if (reply == 0) return false;
            if (reply < 0)
            {
                offerRefusals++;
                nextOffer = clock.Elapsed.TotalSeconds + OfferGap(offerRefusals, random.NextDouble());
                Say("D'accord, une autre fois.");
                return true;
            }
            offerRefusals = 0;
            if (offer.Act == null) { text = offer.Request; return false; }
            Say(offer.Act() ? "Regarde bien !" : "Finalement, pas maintenant.");
            return true;
        }
    }
}
