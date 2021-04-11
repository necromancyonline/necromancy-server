using System;
using System.IO;
using System.Runtime.Serialization;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            type = DatabaseType.SqLite;
            sqLiteFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Database");
            scriptFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Database/Script");
            host = "localhost";
            port = 3306;
            database = "necromancy";
            user = string.Empty;
            password = string.Empty;
        }

        public DatabaseSettings(DatabaseSettings databaseSettings)
        {
            type = databaseSettings.type;
            sqLiteFolder = databaseSettings.sqLiteFolder;
            host = databaseSettings.host;
            port = databaseSettings.port;
            user = databaseSettings.user;
            password = databaseSettings.password;
            database = databaseSettings.database;
            scriptFolder = databaseSettings.scriptFolder;
        }

        [DataMember(Order = 0)] public DatabaseType type { get; set; }

        [DataMember(Order = 1)] public string sqLiteFolder { get; set; }

        [DataMember(Order = 2)] public string host { get; set; }

        [DataMember(Order = 3)] public short port { get; set; }

        [DataMember(Order = 4)] public string user { get; set; }

        [DataMember(Order = 5)] public string password { get; set; }

        [DataMember(Order = 6)] public string database { get; set; }

        [DataMember(Order = 7)] public string scriptFolder { get; set; }
    }
}
