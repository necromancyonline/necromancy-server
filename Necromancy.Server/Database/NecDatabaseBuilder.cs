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
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NecDatabaseBuilder));

        private readonly NecSetting _setting;
        private readonly SettingRepository _settingRepository;

        public NecDatabaseBuilder(NecSetting setting, SettingRepository settingRepository = null)
        {
            _setting = setting;
            _settingRepository = settingRepository;
            if (_settingRepository == null)
            {
                _settingRepository = new SettingRepository(_setting.RepositoryFolder).Initialize();
            }
        }

        public IDatabase Build()
        {
            IDatabase database = null;
            switch (_setting.DatabaseSettings.Type)
            {
                case DatabaseType.SQLite:
                    string sqLitePath = Path.Combine(_setting.DatabaseSettings.SqLiteFolder, "db.sqlite");
                    database = new NecSqLiteDb(sqLitePath);
                    break;
            }

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
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
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "schema_sqlite.sql"));

                // insert maps
                foreach (MapSetting mapSetting in _settingRepository.Map.Values)
                {
                    MapData mapData = new MapData();
                    mapData.Id = mapSetting.Id;
                    mapData.Country = mapSetting.Country;
                    if (mapData.Country == null)
                    {
                        mapData.Country = "";
                    }
                    mapData.Area = mapSetting.Area;                    
                    if (mapData.Area == null)
                    {
                        mapData.Area = "";
                    }
                    mapData.Place = mapSetting.Place;                    
                    if (mapData.Place == null)
                    {
                        mapData.Place = "";
                    }
                    mapData.X = mapSetting.X;
                    mapData.Y = mapSetting.Y;
                    mapData.Z = mapSetting.Z;
                    mapData.Orientation = mapSetting.Orientation;
                    if (!database.InsertMap(mapData))
                    {
                        Logger.Error($"MapId: {mapData.Id} - failed to insert`");
                        return;
                    }
                }

                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_account.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_npc_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_monster_spawn.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_skill.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_union.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_auction.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_gimmick.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_maptransition.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_ggate.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_item_library.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_item_instance.sql"));
                scriptRunner.Run(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "data_shortcut_bar.sql"));
                
            }

            SqlMigrator migrator = new SqlMigrator(database);
            migrator.Migrate(Path.Combine(_setting.DatabaseSettings.ScriptFolder, "Migrations/"));
        }
    }
}
