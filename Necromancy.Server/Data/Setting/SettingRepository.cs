using System.Collections.Generic;
using System.IO;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SettingRepository
    {
        private DirectoryInfo _directory;
        private readonly ILogger _logger;

        public SettingRepository(string folder)
        {
            _logger = LogProvider.Logger(this);
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                _logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            Items = new Dictionary<int, ItemSetting>();
            Maps = new Dictionary<int, MapSetting>();
            Strings = new StrTableSettingLookup();
            Monster = new Dictionary<int, MonsterSetting>();
        }

        public Dictionary<int, ItemSetting> Items { get; }
        public Dictionary<int, MapSetting> Maps { get; }
        public Dictionary<int, MonsterSetting> Monster { get; }
        public StrTableSettingLookup Strings { get; }

        public SettingRepository Initialize()
        {
            Items.Clear();
            Maps.Clear();
            Strings.Clear();
            Load(Strings, "str_table.csv", new StrTableCsvReader());
            Load(Items, "iteminfo.csv", new ItemInfoCsvReader());
            Load(Maps, "map.csv", new MapCsvReader(Strings));
            Load(Monster, "monster.csv", new MonsterCsvReader());
            return this;
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                _logger.Error($"Could not load '{fileName}', file does not exist");
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