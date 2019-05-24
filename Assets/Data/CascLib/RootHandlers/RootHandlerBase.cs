using System.Collections.Generic;
using System.Linq;

namespace CASCLib
{
    public abstract class RootHandlerBase
    {
        protected readonly Jenkins96 Hasher = new Jenkins96();
        protected CASCFolder Root;

        public virtual uint Count { get; protected set; }
        public virtual uint CountTotal { get; protected set; }
        public virtual uint CountSelect { get; protected set; }
        public virtual uint CountUnknown { get; protected set; }
        public virtual LocaleFlags Locale { get; protected set; }
        public virtual Dictionary<string, uint> ListFileEntries { get; set; }
        public bool OverrideArchive { get; protected set; }

        public abstract IEnumerable<KeyValuePair<ulong, RootEntry>> GetAllEntries();

        public abstract IEnumerable<RootEntry> GetAllEntries(ulong hash);

        public abstract IEnumerable<RootEntry> GetEntries(ulong hash);

        public abstract ulong GetHashByFileDataId(uint FileDataId);

        public abstract uint GetFileDataIdByHash(ulong hash);

        public abstract uint GetFileDataIdByName(string name);

        public abstract void LoadListFile();

        public abstract void Clear();

        protected abstract CASCFolder CreateStorageTree();

        private static readonly char[] PathDelimiters = new char[] { '/', '\\' };

        protected void CreateSubTree(CASCFolder root, ulong filehash, string file)
        {
            string[] parts = file.Split(PathDelimiters);

            CASCFolder folder = root;

            for (int i = 0; i < parts.Length; ++i)
            {
                bool isFile = (i == parts.Length - 1);

                string entryName = parts[i];

                ICASCEntry entry = folder.GetEntry(entryName);

                if (entry == null)
                {
                    if (isFile)
                    {
                        if (!CASCFile.Files.ContainsKey(filehash))
                        {
                            entry = new CASCFile(filehash, file);
                            CASCFile.Files[filehash] = (CASCFile)entry;
                        }
                        else
                            entry = CASCFile.Files[filehash];
                    }
                    else
                    {
                        entry = new CASCFolder(entryName);
                    }

                    folder.Entries[entryName] = entry;
                }

                folder = entry as CASCFolder;
            }
        }

        protected IEnumerable<RootEntry> GetEntriesForSelectedLocale(ulong hash)
        {
            var rootInfos = GetAllEntries(hash);

            if (!rootInfos.Any())
                yield break;

            var rootInfosLocale = rootInfos.Where(re => (re.LocaleFlags & Locale) != 0);

            foreach (var entry in rootInfosLocale)
                yield return entry;
        }

        public void MergeInstall(InstallHandler install)
        {
            if (install == null)
                return;

            foreach (var entry in install.GetEntries())
            {
                CreateSubTree(Root, Hasher.ComputeHash(entry.Name), entry.Name);
            }
        }

        public CASCFolder SetFlags(LocaleFlags locale, bool overridearchive = false, bool createTree = true)
        {
            Locale = locale;
            OverrideArchive = overridearchive;

            if (createTree)
                Root = CreateStorageTree();

            return Root;
        }
    }
}
