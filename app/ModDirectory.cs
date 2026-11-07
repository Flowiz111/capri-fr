using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
#if !CROSS
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
#endif
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CapriDesktop
{
    internal sealed class ModListing
    {
        internal string Folder, FileName, Sha, PreviewSha;
        internal string Name = "", Author = "", About = "";
        internal long Size;
        internal int Downloads, Votes;
        internal double Rating;
        internal string Title
        {
            get
            {
                if (Name.Length > 0) return Name;
                string[] words = Folder.Split(new[] { '-', '_', '.' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++) words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);
                return string.Join(" ", words);
            }
        }
        internal string Kind
        {
            get
            {
                string extension = Path.GetExtension(FileName);
                return string.Equals(extension, Mods.CodeExtension, StringComparison.OrdinalIgnoreCase) ? "EXÉCUTE DU CODE." : string.Equals(extension, Mods.SkinExtension, StringComparison.OrdinalIgnoreCase) ? "APPARENCE." : "RÉGLAGES.";
            }
        }
        internal string Id { get { return Path.GetFileNameWithoutExtension(FileName); } }
        internal int KindIndex
        {
            get
            {
                string extension = Path.GetExtension(FileName);
                return string.Equals(extension, Mods.CodeExtension, StringComparison.OrdinalIgnoreCase) ? 2 : string.Equals(extension, Mods.SkinExtension, StringComparison.OrdinalIgnoreCase) ? 3 : 1;
            }
        }
    }

    internal sealed class ModStats
    {
        internal int Downloads, Votes;
        internal double Rating;
    }

    internal static class ModDirectory
    {
        // Capri FR: the one place that says whose mods repository this is. Page, Api, Raw and the View and Fork buttons all follow it.
        private const string Repository = "Flowiz111/capri-mods";
        internal const string Page = "https://github.com/" + Repository;
        internal static readonly string[] Kinds = { "Tous", "Réglages", "Code", "Apparence" };
        private const string Api = "https://api.github.com/repos/" + Repository + "/git/";
        private const long Largest = 96L * 1024 * 1024;
        private const long LargestList = 16L * 1024 * 1024;
        private const long LargestPreview = 2L * 1024 * 1024;
        private const long LargestIndex = 2L * 1024 * 1024;
        private const int WidestPreview = 4096, PreviewWide = 444;
        private const string Raw = "https://raw.githubusercontent.com/" + Repository + "/HEAD/";
        internal const string IndexPage = Raw + "index.json";
        internal const string StatsPage = "https://capri-reactions.lachydotmcg.workers.dev/mods";
        private static readonly byte[] PngStart = { 137, 80, 78, 71, 13, 10, 26, 10 };
        private static readonly Regex PreviewFile = new Regex(@"^mods/([A-Za-z0-9][A-Za-z0-9._-]{0,63})/preview\.png$");
        internal static string PreviewFolder = Path.Combine(Path.GetTempPath(), "capri-mod-previews");
        internal static Func<string, string, byte[]> Post = Push;
        private static readonly Regex Listed = new Regex(@"^mods/([A-Za-z0-9][A-Za-z0-9._-]{0,63})/([A-Za-z0-9][A-Za-z0-9._-]{0,63}\.capri-(?:mod|code|skin))$");
        private static readonly Regex Blob = new Regex(@"^[0-9a-f]{40}$");
        internal static Func<string, long, byte[]> Get = Fetch;

        [DataContract]
        private sealed class Tree
        {
            [DataMember(Name = "tree")] public TreeEntry[] Entries;
        }

        [DataContract]
        private sealed class TreeEntry
        {
            [DataMember(Name = "path")] public string Path;
            [DataMember(Name = "type")] public string Type;
            [DataMember(Name = "sha")] public string Sha;
            [DataMember(Name = "size")] public long Size;
        }

        [DataContract]
        private sealed class IndexBody
        {
            [DataMember(Name = "format")] public int Format;
            [DataMember(Name = "mods")] public IndexEntry[] Mods;
        }

        [DataContract]
        private sealed class IndexEntry
        {
            [DataMember(Name = "folder")] public string Folder;
            [DataMember(Name = "file")] public string File;
            [DataMember(Name = "name")] public string Name;
            [DataMember(Name = "author")] public string Author;
            [DataMember(Name = "about")] public string About;
        }

        [DataContract]
        private sealed class StatsBody
        {
            [DataMember(Name = "mods")] public StatsEntry[] Mods;
        }

        [DataContract]
        private sealed class StatsEntry
        {
            [DataMember(Name = "folder")] public string Folder;
            [DataMember(Name = "downloads")] public int Downloads;
            [DataMember(Name = "rating")] public double Rating;
            [DataMember(Name = "votes")] public int Votes;
        }

        internal static List<ModListing> Browse()
        {
            // Capri FR: GitHub's API allows 60 requests an hour per IP address, which a shared connection can use up. tree.json in the
            // repository is a copy of the same list, kept for that case. Each download is still checked against its checksum.
            byte[] tree;
            try { tree = Get(Api + "trees/HEAD?recursive=1", LargestList); }
            catch (WebException) { tree = Get(Raw + "tree.json", LargestList); }
            List<ModListing> found = Parse(tree);
            try { ReadIndex(found, Get(IndexPage, LargestIndex)); }
            catch (Exception) { }
            return found;
        }

        private static void ReadIndex(List<ModListing> listings, byte[] json)
        {
            if (json == null || json.Length > LargestIndex) return;
            IndexBody index;
            using (MemoryStream body = new MemoryStream(json)) index = (IndexBody)new DataContractJsonSerializer(typeof(IndexBody)).ReadObject(body);
            if (index == null || index.Format != 1) return;
            Dictionary<string, ModListing> listed = new Dictionary<string, ModListing>(StringComparer.Ordinal);
            foreach (ModListing listing in listings) listed[listing.Folder + "/" + listing.FileName] = listing;
            foreach (IndexEntry entry in index.Mods ?? new IndexEntry[0])
            {
                if (entry == null || entry.Folder == null || entry.File == null) continue;
                ModListing listing;
                if (!listed.TryGetValue(entry.Folder + "/" + entry.File, out listing)) continue;
                listing.Name = Plain(entry.Name, 120);
                listing.Author = Plain(entry.Author, 80);
                listing.About = Plain(entry.About, 600);
            }
        }

        private static string Plain(string value, int most)
        {
            string line = Regex.Replace(value ?? "", @"[\p{Cc}\p{Cf}\s]+", " ").Trim();
            if (line.Length > most) line = line.Substring(0, char.IsHighSurrogate(line[most - 1]) ? most - 1 : most).TrimEnd();
            return line;
        }

        internal static List<ModListing> Parse(byte[] json)
        {
            Tree tree;
            try
            {
                using (MemoryStream body = new MemoryStream(json)) tree = (Tree)new DataContractJsonSerializer(typeof(Tree)).ReadObject(body);
            }
            catch (SerializationException) { throw new ModException("L'annuaire des mods a envoyé quelque chose d'illisible."); }
            List<ModListing> found = new List<ModListing>();
            Dictionary<string, string> previews = new Dictionary<string, string>();
            foreach (TreeEntry entry in tree.Entries ?? new TreeEntry[0])
            {
                if (entry == null || entry.Type != "blob" || entry.Path == null || entry.Sha == null || entry.Size <= 0 || entry.Size > LargestPreview || !Blob.IsMatch(entry.Sha)) continue;
                Match picture = PreviewFile.Match(entry.Path);
                if (picture.Success && !picture.Groups[1].Value.Contains("..")) previews[picture.Groups[1].Value] = entry.Sha;
            }
            foreach (TreeEntry entry in tree.Entries ?? new TreeEntry[0])
            {
                if (entry == null || entry.Type != "blob" || entry.Path == null || entry.Sha == null || entry.Size <= 0 || entry.Size > Largest || !Blob.IsMatch(entry.Sha)) continue;
                Match match = Listed.Match(entry.Path);
                if (!match.Success || match.Groups[1].Value.Contains("..") || match.Groups[2].Value.Contains("..")) continue;
                string previewSha;
                previews.TryGetValue(match.Groups[1].Value, out previewSha);
                found.Add(new ModListing { Folder = match.Groups[1].Value, FileName = match.Groups[2].Value, Sha = entry.Sha, Size = entry.Size, PreviewSha = previewSha });
            }
            found.Sort(delegate(ModListing a, ModListing b) { return StringComparer.OrdinalIgnoreCase.Compare(a.Folder + "/" + a.FileName, b.Folder + "/" + b.FileName); });
            return found;
        }

        internal static List<ModListing> Filter(List<ModListing> listings, string search, int kind)
        {
            string[] words = (search ?? "").Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return listings.FindAll(delegate(ModListing listing)
            {
                if (kind != 0 && listing.KindIndex != kind) return false;
                string named = listing.Folder + " " + listing.FileName + " " + listing.Title + " " + listing.Author + " " + listing.About;
                foreach (string word in words) if (named.IndexOf(word, StringComparison.OrdinalIgnoreCase) < 0) return false;
                return true;
            });
        }

        internal static string Download(ModListing listing, string under)
        {
            // Capri FR: from the raw file rather than GitHub's API, which allows 60 requests an hour per IP address. The checksum is what makes it safe either way.
            byte[] bytes = Get(Raw + "mods/" + listing.Folder + "/" + listing.FileName, Largest);
            if (bytes.Length != listing.Size || BlobSha(bytes) != listing.Sha) throw new ModException(listing.FileName + " ne correspond pas à ce que l'annuaire annonce : il n'a pas été installé.");
            string folder = Path.Combine(under, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, Path.GetFileName(listing.FileName));
            File.WriteAllBytes(file, bytes);
            return file;
        }

        internal static byte[] Preview(ModListing listing, string cache)
        {
            if (listing.PreviewSha == null) return null;
            string kept = Path.Combine(cache, listing.PreviewSha + ".png");
            try
            {
                if (File.Exists(kept))
                {
                    byte[] saved = File.ReadAllBytes(kept);
                    if (saved.Length <= LargestPreview && BlobSha(saved) == listing.PreviewSha) return saved;
                }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            byte[] bytes = Get(Raw + "mods/" + listing.Folder + "/preview.png", LargestPreview);
            if (BlobSha(bytes) != listing.PreviewSha || !IsPng(bytes)) throw new ModException("L'aperçu de " + listing.Folder + " ne correspond pas à ce que l'annuaire annonce.");
            try
            {
                Directory.CreateDirectory(cache);
                File.WriteAllBytes(kept, bytes);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return bytes;
        }

        internal static bool IsPng(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 24) return false;
            for (int i = 0; i < PngStart.Length; i++) if (bytes[i] != PngStart[i]) return false;
            return true;
        }

        internal static long BigEndian(byte[] bytes, int at)
        {
            return ((long)bytes[at] << 24) | ((long)bytes[at + 1] << 16) | ((long)bytes[at + 2] << 8) | bytes[at + 3];
        }

#if !CROSS
        internal static Image Picture(byte[] bytes)
        {
            if (!IsPng(bytes)) return null;
            long wide = BigEndian(bytes, 16), tall = BigEndian(bytes, 20);
            if (wide < 1 || tall < 1 || wide > WidestPreview || tall > WidestPreview) return null;
            try
            {
                using (MemoryStream body = new MemoryStream(bytes))
                using (Image read = Image.FromStream(body))
                {
                    int width = Math.Min(PreviewWide, read.Width), height = Math.Max(1, (int)Math.Round(read.Height * (double)width / read.Width));
                    Bitmap small = new Bitmap(width, height);
                    using (Graphics paint = Graphics.FromImage(small))
                    {
                        paint.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        paint.DrawImage(read, 0, 0, width, height);
                    }
                    return small;
                }
            }
            catch (ArgumentException) { return null; }
            catch (ExternalException) { return null; }
            catch (OutOfMemoryException) { return null; }
        }
#else
        internal static Avalonia.Media.Imaging.Bitmap Picture(byte[] bytes)
        {
            if (!IsPng(bytes)) return null;
            long wide = BigEndian(bytes, 16), tall = BigEndian(bytes, 20);
            if (wide < 1 || tall < 1 || wide > WidestPreview || tall > WidestPreview) return null;
            try
            {
                using (MemoryStream body = new MemoryStream(bytes))
                    return Avalonia.Media.Imaging.Bitmap.DecodeToWidth(body, (int)Math.Min(PreviewWide, wide));
            }
            catch (ArgumentException) { return null; }
            catch (InvalidOperationException) { return null; }
            catch (NotSupportedException) { return null; }
        }
#endif

        internal static Dictionary<string, ModStats> Stats()
        {
            StatsBody body;
            try
            {
                using (MemoryStream read = new MemoryStream(Get(StatsPage, LargestPreview))) body = (StatsBody)new DataContractJsonSerializer(typeof(StatsBody)).ReadObject(read);
            }
            catch (SerializationException) { throw new ModException("Les chiffres de l'annuaire des mods sont illisibles."); }
            Dictionary<string, ModStats> found = new Dictionary<string, ModStats>(StringComparer.OrdinalIgnoreCase);
            foreach (StatsEntry entry in body.Mods ?? new StatsEntry[0])
                if (entry != null && entry.Folder != null) found[entry.Folder] = new ModStats { Downloads = Math.Max(0, entry.Downloads), Rating = Math.Max(0, Math.Min(5, entry.Rating)), Votes = Math.Max(0, entry.Votes) };
            return found;
        }

        internal static void Apply(List<ModListing> listings, Dictionary<string, ModStats> stats)
        {
            foreach (ModListing listing in listings)
            {
                ModStats found;
                if (!stats.TryGetValue(listing.Folder, out found)) continue;
                listing.Downloads = found.Downloads;
                listing.Rating = found.Rating;
                listing.Votes = found.Votes;
            }
        }

        internal static string RatingLine(ModListing listing)
        {
            return listing.Votes == 0 ? "Pas de note" : listing.Rating.ToString("0.0", CultureInfo.InvariantCulture) + " (" + listing.Votes + ")";
        }

        internal static string DownloadLine(ModListing listing)
        {
            string count = listing.Downloads >= 1000000 ? (listing.Downloads / 1000000.0).ToString("0.#", CultureInfo.InvariantCulture) + "M" : listing.Downloads >= 1000 ? (listing.Downloads / 1000.0).ToString("0.#", CultureInfo.InvariantCulture) + "K" : listing.Downloads.ToString(CultureInfo.InvariantCulture);
            return count + (listing.Downloads == 1 ? " téléchargement" : " téléchargements");
        }

        internal static void CountDownload(string folder)
        {
            Post(StatsPage + "/download", "{\"folder\":\"" + folder + "\"}");
        }

        internal static ModStats Rate(string folder, int stars)
        {
            byte[] answer = Post(StatsPage + "/rate", "{\"folder\":\"" + folder + "\",\"stars\":" + stars + "}");
            StatsEntry entry;
            try
            {
                using (MemoryStream read = new MemoryStream(answer)) entry = (StatsEntry)new DataContractJsonSerializer(typeof(StatsEntry)).ReadObject(read);
            }
            catch (SerializationException) { throw new ModException("La réponse à ta note est illisible."); }
            return new ModStats { Downloads = Math.Max(0, entry.Downloads), Rating = Math.Max(0, Math.Min(5, entry.Rating)), Votes = Math.Max(0, entry.Votes) };
        }

        internal static string PublishSteps(Mod mod)
        {
            string id = mod.File.Id, file = Path.GetFileName(mod.Path);
            return "1. Appuie sur Forker capri-mods et crée ta copie sur GitHub. Il te faut un compte gratuit.\n\n"
                + "2. Dans ta copie, ajoute un dossier nommé mods/" + id + "/.\n\n"
                + "3. Mets-y " + file + " et un court README.md qui dit ce qu'il fait et qui l'a créé.\n\n"
                + "4. Ajoute une image preview.png, environ 16:9. C'est ce qu'on voit dans la fenêtre des mods.\n\n"
                + "5. Ouvre une pull request vers Flowiz111/capri-mods. Le responsable du dépôt décide s'il publie ton mod."
                + (mod.Code ? "\n\nC'est un mod de code : Capri demandera aux gens avant de le lancer." : "");
        }

        internal static string BlobSha(byte[] bytes)
        {
            byte[] head = Encoding.ASCII.GetBytes("blob " + bytes.Length + "\0");
            using (SHA1 sha = SHA1.Create())
            {
                sha.TransformBlock(head, 0, head.Length, null, 0);
                sha.TransformFinalBlock(bytes, 0, bytes.Length);
                return BitConverter.ToString(sha.Hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private static byte[] Fetch(string url, long most)
        {
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "CapriFR-Desktop";
            request.Accept = url.Contains("/blobs/") ? "application/vnd.github.raw+json" : "application/vnd.github+json";
            request.Timeout = 15000;
            request.ReadWriteTimeout = 30000;
            using (WebResponse response = request.GetResponse())
            using (Stream body = response.GetResponseStream())
            using (MemoryStream kept = new MemoryStream())
            {
                byte[] chunk = new byte[81920];
                int read;
                while ((read = body.Read(chunk, 0, chunk.Length)) > 0)
                {
                    if (kept.Length + read > most) throw new ModException("C'est trop gros pour venir de l'annuaire des mods.");
                    kept.Write(chunk, 0, read);
                }
                return kept.ToArray();
            }
        }

        private static byte[] Push(string url, string json)
        {
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.UserAgent = "CapriFR-Desktop";
            request.ContentType = "application/json";
            request.Timeout = 15000;
            request.ReadWriteTimeout = 15000;
            byte[] sent = Encoding.UTF8.GetBytes(json);
            using (Stream body = request.GetRequestStream()) body.Write(sent, 0, sent.Length);
            using (WebResponse response = request.GetResponse())
            using (Stream body = response.GetResponseStream())
            using (MemoryStream kept = new MemoryStream())
            {
                body.CopyTo(kept);
                return kept.ToArray();
            }
        }

        internal static void Clean(string file)
        {
            try { Directory.Delete(Path.GetDirectoryName(file), true); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
