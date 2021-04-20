using System.Collections.Generic;
using System.IO;
using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SettingRepository
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(SettingRepository));

        private readonly DirectoryInfo _directory;

        public SettingRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                _Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            itemInfo = new Dictionary<int, ItemInfoSetting>();
            itemNecromancy = new Dictionary<int, ItemNecromancySetting>();
            itemLibrary = new Dictionary<int, ItemLibrarySetting>();
            map = new Dictionary<int, MapSetting>();
            strings = new StrTableSettingLookup();
            monster = new Dictionary<int, MonsterSetting>();
            skillBase = new Dictionary<int, SkillBaseSetting>();
            eoBase = new Dictionary<int, EoBaseSetting>();
            npc = new Dictionary<int, NpcSetting>();
            modelAtr = new Dictionary<int, ModelAtrSetting>();
            modelCommon = new Dictionary<int, ModelCommonSetting>();
            honor = new Dictionary<int, HonorSetting>();
        }

        public Dictionary<int, ItemInfoSetting> itemInfo { get; }
        public Dictionary<int, ItemNecromancySetting> itemNecromancy { get; }
        public Dictionary<int, ItemLibrarySetting> itemLibrary { get; }
        public Dictionary<int, MapSetting> map { get; }
        public Dictionary<int, MonsterSetting> monster { get; }
        public Dictionary<int, SkillBaseSetting> skillBase { get; }
        public Dictionary<int, EoBaseSetting> eoBase { get; }
        public StrTableSettingLookup strings { get; }
        public Dictionary<int, NpcSetting> npc { get; }
        public Dictionary<int, ModelAtrSetting> modelAtr { get; }
        public Dictionary<int, ModelCommonSetting> modelCommon { get; }
        public Dictionary<int, HonorSetting> honor { get; }


        public SettingRepository Initialize()
        {
            itemInfo.Clear();
            //ItemNecromancy.Clear();
            itemLibrary.Clear();
            map.Clear();
            strings.Clear();
            monster.Clear();
            skillBase.Clear();
            eoBase.Clear();
            npc.Clear();
            modelAtr.Clear();
            modelCommon.Clear();
            honor.Clear();
            Load(strings, "str_table.csv", new StrTableCsvReader());
            Load(itemInfo, "iteminfo.csv", new ItemInfoCsvReader());
            Load(itemInfo, "iteminfo2.csv", new ItemInfoCsvReader());
            //Load(ItemNecromancy, "item_necromancy.csv", new ItemNecromancyCsvReader()); //disabled migrating to new library
            Load(itemLibrary, "itemlibrary.csv", new ItemLibrarySettingCsvReader());
            Load(monster, "monster.csv", new MonsterCsvReader());
            Load(skillBase, "skill_base.csv", new SkillBaseCsvReader());
            Load(skillBase, "skill_base2.csv", new SkillBaseCsvReader());
            Load(skillBase, "skill_base3.csv", new SkillBaseCsvReader());
            Load(eoBase, "eo_base.csv", new EoBaseCsvReader());
            Load(eoBase, "eo_base2.csv", new EoBaseCsvReader());
            Load(eoBase, "eo_base3.csv", new EoBaseCsvReader());
            Load(npc, "npc.csv", new NpcCsvReader());
            Load(modelAtr, "model_atr.csv", new ModelAtrCsvReader());
            Load(map, "map.csv", new MapCsvReader(strings));
            Load(modelCommon, "model_common.csv", new ModelCommonCsvReader(monster, modelAtr));
            Load(honor, "honor.csv", new HonorCsvReader());
            _Logger.Debug($"Number of Honor titles found. {honor.Count}");
            return this;
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists) _Logger.Error($"Could not load '{fileName}', file does not exist");

            list.AddRange(reader.Read(file.FullName));
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            List<T> items = new List<T>();
            Load(items, fileName, reader);
            foreach (T item in items)
                if (item is ISettingRepositoryItem repositoryItem)
                {
                    if (dictionary.ContainsKey(repositoryItem.id))
                    {
                        _Logger.Error($"Key: '{repositoryItem.id}' already exists, skipping");
                        continue;
                    }

                    dictionary.Add(repositoryItem.id, item);
                }
        }

        private void Load(StrTableSettingLookup lookup, string fileName, CsvReader<StrTableSetting> reader)
        {
            List<StrTableSetting> items = new List<StrTableSetting>();
            Load(items, fileName, reader);
            foreach (StrTableSetting item in items) lookup.Add(item);
        }
    }
}
