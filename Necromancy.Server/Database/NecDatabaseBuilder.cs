using System;
using System.IO;
using Arrowgene.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(NecDatabaseBuilder));

        private readonly NecSetting _setting;
        private readonly SettingRepository _settingRepository;

        public NecDatabaseBuilder(NecSetting setting, SettingRepository settingRepository = null)
        {
            _setting = setting;
            _settingRepository = settingRepository;
            if (_settingRepository == null)
            {
                _settingRepository = new SettingRepository(_setting.repositoryFolder).Initialize();
            }
        }

        public IDatabase Build()
        {
            IDatabase database = null;
            switch (_setting.databaseSettings.type)
            {
                case DatabaseType.SqLite:
                    string sqLitePath = Path.Combine(_setting.databaseSettings.sqLiteFolder, "db.sqlite");
                    database = new NecSqLiteDb(sqLitePath);
                    break;
            }

            if (database == null)
            {
                _Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            Initialize(database);
            return database;
        }

        private void Initialize(IDatabase database)
        {
            if (database.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(database);

                // create table structure
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "schema_sqlite.sql"));

                // insert maps
                foreach (MapSetting mapSetting in _settingRepository.map.Values)
                {
                    MapData mapData = new MapData();
                    mapData.id = mapSetting.id;
                    mapData.country = mapSetting.country;
                    if (mapData.country == null)
                    {
                        mapData.country = "";
                    }
                    mapData.area = mapSetting.area;
                    if (mapData.area == null)
                    {
                        mapData.area = "";
                    }
                    mapData.place = mapSetting.place;
                    if (mapData.place == null)
                    {
                        mapData.place = "";
                    }
                    mapData.x = mapSetting.x;
                    mapData.y = mapSetting.y;
                    mapData.z = mapSetting.z;
                    mapData.orientation = mapSetting.orientation;
                    if (!database.InsertMap(mapData))
                    {
                        _Logger.Error($"MapId: {mapData.id} - failed to insert`");
                        return;
                    }
                }

                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_account.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_npc_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_monster_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_skill.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_union.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_auction.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_gimmick.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_maptransition.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_ggate.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_item_library.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_item_instance.sql"));
                scriptRunner.Run(Path.Combine(_setting.databaseSettings.scriptFolder, "data_shortcut_bar.sql"));

            }

            SqlMigrator migrator = new SqlMigrator(database);
            migrator.Migrate(Path.Combine(_setting.databaseSettings.scriptFolder, "Migrations/"));
        }
    }
}
