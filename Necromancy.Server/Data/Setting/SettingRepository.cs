using System.Collections.Generic;
using System.IO;
using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SettingRepository
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SettingRepository));

        private readonly DirectoryInfo _directory;

        public SettingRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            ItemInfo = new Dictionary<int, ItemInfoSetting>();
            ItemNecromancy = new Dictionary<int, ItemNecromancySetting>();
            ItemLibrary = new Dictionary<int, ItemLibrarySetting>();
            Map = new Dictionary<int, MapSetting>();
            Strings = new StrTableSettingLookup();
            Monster = new Dictionary<int, MonsterSetting>();
            SkillBase = new Dictionary<int, SkillBaseSetting>();
            EoBase = new Dictionary<int, EoBaseSetting>();
            Npc = new Dictionary<int, NpcSetting>();
            ModelAtr = new Dictionary<int, ModelAtrSetting>();
            ModelCommon = new Dictionary<int, ModelCommonSetting>();
            Honor = new Dictionary<int, HonorSetting>();
        }

        public Dictionary<int, ItemInfoSetting> ItemInfo { get; }
        public Dictionary<int, ItemNecromancySetting> ItemNecromancy { get; }
        public Dictionary<int, ItemLibrarySetting> ItemLibrary { get; }
        public Dictionary<int, MapSetting> Map { get; }
        public Dictionary<int, MonsterSetting> Monster { get; }
        public Dictionary<int, SkillBaseSetting> SkillBase { get; }
        public Dictionary<int, EoBaseSetting> EoBase { get; }
        public StrTableSettingLookup Strings { get; }
        public Dictionary<int, NpcSetting> Npc { get; }
        public Dictionary<int, ModelAtrSetting> ModelAtr { get; }
        public Dictionary<int, ModelCommonSetting> ModelCommon { get; }
        public Dictionary<int, HonorSetting> Honor { get; }


        public SettingRepository Initialize()
        {
            ItemInfo.Clear();
            //ItemNecromancy.Clear();
            ItemLibrary.Clear();
            Map.Clear();
            Strings.Clear();
            Monster.Clear();
            SkillBase.Clear();
            EoBase.Clear();
            Npc.Clear();
            ModelAtr.Clear();
            ModelCommon.Clear();
            Honor.Clear();
            Load(Strings, "str_table.csv", new StrTableCsvReader());
            Load(ItemInfo, "iteminfo.csv", new ItemInfoCsvReader());
            Load(ItemInfo, "iteminfo2.csv", new ItemInfoCsvReader());
            //Load(ItemNecromancy, "item_necromancy.csv", new ItemNecromancyCsvReader()); //disabled migrating to new library
            Load(ItemLibrary, "itemlibrary.csv", new ItemLibrarySettingCsvReader());
            Load(Monster, "monster.csv", new MonsterCsvReader());
            Load(SkillBase, "skill_base.csv", new SkillBaseCsvReader());
            Load(SkillBase, "skill_base2.csv", new SkillBaseCsvReader());
            Load(SkillBase, "skill_base3.csv", new SkillBaseCsvReader());
            Load(EoBase, "eo_base.csv", new EoBaseCsvReader());
            Load(EoBase, "eo_base2.csv", new EoBaseCsvReader());
            Load(EoBase, "eo_base3.csv", new EoBaseCsvReader());
            Load(Npc, "npc.csv", new NpcCsvReader());
            Load(ModelAtr, "model_atr.csv", new ModelAtrCsvReader());
            Load(Map, "map.csv", new MapCsvReader(Strings));
            Load(ModelCommon, "model_common.csv", new ModelCommonCsvReader(Monster, ModelAtr));
            Load(Honor, "honor.csv", new HonorCsvReader());
            Logger.Debug($"Number of Honor titles found. {Honor.Count}");
            return this;
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                Logger.Error($"Could not load '{fileName}', file does not exist");
            }

            list.AddRange(reader.Read(file.FullName));
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            List<T> items = new List<T>();
            Load(items, fileName, reader);
            foreach (T item in items)
            {
                if (item is ISettingRepositoryItem repositoryItem)
                {
                    if (dictionary.ContainsKey(repositoryItem.Id))
                    {
                        Logger.Error($"Key: '{repositoryItem.Id}' already exists, skipping");
                        continue;
                    }
                    dictionary.Add(repositoryItem.Id, item);
                }
            }
        }

        private void Load(StrTableSettingLookup lookup, string fileName, CsvReader<StrTableSetting> reader)
        {
            List<StrTableSetting> items = new List<StrTableSetting>();
            Load(items, fileName, reader);
            foreach (StrTableSetting item in items)
            {
                lookup.Add(item);
            }
        }
    }
}
