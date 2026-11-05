using System;
using System.Collections.Generic;
using System.IO;

namespace CapriDesktop
{
    internal enum CareKind { Water, Stretch, Eyes, Late, Meal, Game, Bored }

    internal sealed class Care
    {
        internal const long Away = 600000;
        internal static readonly string[] Names = { "Eau", "Étirements", "Regarder ailleurs", "Rappel de sommeil", "Repas", "Pause de jeu", "Ennui" };
        private readonly Func<double> seconds;
        private readonly Func<DateTime> local;
        private readonly Func<CareKind, string> words;
        private readonly double[] next = new double[7];
        private readonly bool[] pending = new bool[7];
        private readonly bool[] enabled = new bool[7];
        private readonly int[] times = new int[7];
        private bool started, resting, paused;
        private DateTime since;
        private string game;
        private long night, meal;
        private double attentionAt = -1, attentionRate = 1, activeAt;
        private int turn;
        internal CareKind Said;

        internal Care(Func<double> seconds, Func<DateTime> local, Func<CareKind, string> words)
        {
            this.seconds = seconds;
            this.local = local;
            this.words = words;
        }

        internal string Check(bool checkIns, Func<CareKind, bool> careOn, Func<CareKind, int> careTime, ref long careNight, long quiet, bool free, string playing, double noticed, double attentionGap)
        {
            if (!checkIns) { Reset(); return null; }
            double now = seconds();
            DateTime today = local();
            if (!started || resting || quiet > Away)
            {
                since = today;
                activeAt = now;
                game = playing;
                for (int reset = 0; reset < next.Length; reset++)
                {
                    CareKind resetKind = (CareKind)reset;
                    next[reset] = now + careTime(resetKind) * 60.0 * (resetKind == CareKind.Bored ? attentionGap : 1);
                    pending[reset] = false;
                    enabled[reset] = careOn(resetKind);
                    times[reset] = careTime(resetKind);
                }
                started = true;
                resting = quiet > Away;
                if (resting) return null;
            }
            for (int setting = 0; setting < next.Length; setting++)
            {
                CareKind settingKind = (CareKind)setting;
                bool on = careOn(settingKind);
                int time = careTime(settingKind);
                if (!on || on != enabled[setting] || time != times[setting])
                {
                    pending[setting] = false;
                    next[setting] = now + time * 60.0;
                }
                enabled[setting] = on;
                times[setting] = time;
            }
            if (noticed != attentionAt || attentionGap != attentionRate)
            {
                attentionAt = noticed;
                attentionRate = attentionGap;
                pending[(int)CareKind.Bored] = false;
                next[(int)CareKind.Bored] = (noticed < 0 ? now : Math.Max(noticed, activeAt)) + careTime(CareKind.Bored) * 60.0 * attentionGap;
            }
            if (!string.Equals(game, playing, StringComparison.OrdinalIgnoreCase))
            {
                if (playing != null) pending[(int)CareKind.Game] = false;
                game = playing;
                next[(int)CareKind.Game] = now + careTime(CareKind.Game) * 60.0;
            }
            for (int timer = 0; timer < next.Length; timer++)
            {
                CareKind timerKind = (CareKind)timer;
                if (timerKind == CareKind.Late || timerKind == CareKind.Meal || !enabled[timer]) continue;
                if (timerKind == CareKind.Game && game == null) continue;
                if (timerKind == CareKind.Bored && (noticed < 0 || now - noticed < times[timer] * 60.0 * attentionGap))
                {
                    pending[timer] = false;
                    continue;
                }
                if (now < next[timer]) continue;
                pending[timer] = true;
                next[timer] = now + times[timer] * 60.0 * (timerKind == CareKind.Bored ? attentionGap : 1);
            }
            DateTime evening = today.AddHours(-12).Date;
            int hour = careTime(CareKind.Late);
            DateTime bedtime = evening.AddDays(hour < 12 ? 1 : 0).AddHours(hour);
            bool late = today >= bedtime && today < evening.AddDays(1).AddHours(6);
            if (paused && late && enabled[(int)CareKind.Late]) night = evening.Ticks;
            paused = false;
            if (!late || night != evening.Ticks) pending[(int)CareKind.Late] = false;
            if (enabled[(int)CareKind.Late] && late && night != evening.Ticks && careNight != evening.Ticks)
            {
                night = evening.Ticks;
                pending[(int)CareKind.Late] = true;
            }
            DateTime lunch = today.Date.AddHours(12);
            int mealGap = careTime(CareKind.Meal);
            DateTime mealAt = today < lunch ? lunch : lunch.AddMinutes(Math.Floor((today - lunch).TotalMinutes / mealGap) * mealGap);
            bool mealtime = since < lunch && today >= mealAt && today < mealAt.AddHours(1) && mealAt.Hour < 21;
            if (!mealtime || meal != mealAt.Ticks) pending[(int)CareKind.Meal] = false;
            if (enabled[(int)CareKind.Meal] && mealtime && meal != mealAt.Ticks)
            {
                meal = mealAt.Ticks;
                pending[(int)CareKind.Meal] = true;
            }
            if (!free) return null;
            string line = null;
            for (int choice = 0; choice < pending.Length; choice++)
            {
                int index = (turn + choice) % pending.Length;
                if (!pending[index]) continue;
                string choiceLine = words((CareKind)index);
                if (string.IsNullOrWhiteSpace(choiceLine)) continue;
                line = choiceLine;
                Said = (CareKind)index;
                turn = (index + 1) % pending.Length;
                if (Said == CareKind.Late) careNight = evening.Ticks;
                break;
            }
            for (int cleared = 0; cleared < pending.Length; cleared++)
                if (pending[cleared]) next[cleared] = now + times[cleared] * 60.0 * (cleared == (int)CareKind.Bored ? attentionGap : 1);
            Array.Clear(pending, 0, pending.Length);
            return line;
        }

        internal void Reset()
        {
            started = false;
            paused = true;
            Array.Clear(pending, 0, pending.Length);
        }
    }

    internal sealed class CareLines
    {
        internal const string Defaults =
            "# Rappels de Capri\n\n" +
            "Modifie les lignes sous chaque titre pour changer ce qu'elle dit. Retire une ligne pour ne plus l'utiliser, ou toutes les lignes d'un titre pour couper ce rappel. Ces lignes restent sur ce PC.\n\n" +
            "## Eau\n- Bois un peu d'eau, choom. Je garde ta place.\n- Pause hydratation. Même toi, tu as besoin de faire le plein.\n\n" +
            "## Étirements\n- Déplie-toi un peu. Ces jambes marchent encore.\n- Petite pause étirements. Je reste là.\n\n" +
            "## Regarder ailleurs\n- Regarde ailleurs 20 secondes pour tes yeux. Je survivrai.\n- Regarde au loin pendant 20 secondes. Oui, encore plus loin que moi.\n\n" +
            "## Rappel de sommeil\n- Il est tard, choom. Ton lit t'attend.\n\n" +
            "## Repas\n- Tu es là depuis avant le déjeuner. Va manger quelque chose.\n- Pause repas. Ce cerveau ne tourne pas à vide.\n\n" +
            "## Pause de jeu\n- Longue session. Fais une petite pause dès que tu peux.\n- Ça fait un moment que tu joues. Étire tes mains quand tu peux.\n\n" +
            "## Ennui\n- Je m'ennuiiie.\n- J'ai déjà compté tes icônes deux fois.\n- Un de mes esprits s'invente des distractions. Inquiétant.\n- Petit gremlin. Grand ennui.\n\n" +
            "## Ennui remarqué\n- Te voilà.\n- Enfin. Salut.\n\n" +
            "## Ennui ignoré\n- Très bien. Je vais m'occuper toute seule.\n- Cool. Cool cool cool.\n";
        private readonly string path;
        private readonly Dictionary<string, string> last = new Dictionary<string, string>();
        private readonly Random dice = new Random();

        internal CareLines(string path) { this.path = path; }

        internal bool Create()
        {
            try
            {
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.CreateNew, FileAccess.Write))) writer.Write(Defaults);
                }
                else
                {
                    string existing = File.ReadAllText(path);
                    bool written = existing.IndexOf("# Capri's check-ins", StringComparison.Ordinal) >= 0;
                    foreach (string heading in Care.Names) if (existing.IndexOf("## " + heading, StringComparison.Ordinal) >= 0) written = true;
                    if (!written) File.AppendAllText(path, "\n\n" + Defaults);
                }
                return true;
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
        }

        internal string Pick(CareKind kind)
        {
            return Pick(Care.Names[(int)kind]);
        }

        internal string Pick(string name)
        {
            string text;
            try { text = File.ReadAllText(path); }
            catch (IOException) { return null; }
            catch (UnauthorizedAccessException) { return null; }
            List<string> choices = new List<string>();
            bool section = false;
            using (StringReader reader = new StringReader(text))
            {
                string row;
                while ((row = reader.ReadLine()) != null)
                {
                    row = row.Trim();
                    if (row.StartsWith("#", StringComparison.Ordinal)) section = row == "## " + name;
                    else if (section && row.StartsWith("- ", StringComparison.Ordinal) && row.Substring(2).Trim().Length > 0)
                    {
                        string words = row.Substring(2).Trim();
                        if (!choices.Contains(words)) choices.Add(words);
                    }
                }
            }
            if (choices.Count == 0) return null;
            string previous;
            if (choices.Count > 1 && last.TryGetValue(name, out previous)) choices.Remove(previous);
            string picked = choices[dice.Next(choices.Count)];
            last[name] = picked;
            return picked;
        }
    }
}
