using System;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Logging;
using Necromancy.Server.Database.Sql.Core;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Database.Sql
{
    /// <summary>
    ///     SQLite Necromancy database.
    /// </summary>
    public class NecSqLiteDb : NecSqlDb<SQLiteConnection, SQLiteCommand>, IDatabase
    {
        public const string MEMORY_DATABASE_PATH = ":memory:";

        private const string SELECT_AUTO_INCREMENT = "SELECT last_insert_rowid()";

        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecSqLiteDb));

        private readonly string _databasePath;
        private string _connectionString;

        public NecSqLiteDb(string databasePath)
        {
            _databasePath = databasePath;
            _Logger.Info($"Database Path: {_databasePath}");
        }

        public long version
        {
            get => (long)Command("PRAGMA user_version;", Connection()).ExecuteScalar();
            set => Command(String.Format("PRAGMA user_version = {0};", value), Connection()).ExecuteNonQuery();
        }

        public bool CreateDatabase()
        {
            if (_databasePath != MEMORY_DATABASE_PATH && !File.Exists(_databasePath))
            {
                FileStream fs = File.Create(_databasePath);
                fs.Close();
                fs.Dispose();
                _Logger.Info($"Creating new v{version} database. This may take a few minutes.");
                return true;
            }

            return false;
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;
            // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
            builder.Flags = builder.Flags & SQLiteConnectionFlags.StrictConformance;

            string connectionString = builder.ToString();
            _Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override SQLiteConnection Connection()
        {
            if (_connectionString == null) _connectionString = BuildConnectionString(_databasePath);

            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            return connection.OpenAndReturn();
        }

        protected override SQLiteCommand Command(string query, SQLiteConnection connection)
        {
            return new SQLiteCommand(query, connection);
        }

        /// <summary>
        ///     Thread Safe on Connection basis.
        ///     http://www.sqlite.org/c3ref/last_insert_rowid.html
        /// </summary>
        protected override long AutoIncrement(SQLiteConnection connection, SQLiteCommand command)
        {
            return connection.LastInsertRowId;
            // long autoIncrement = NoAutoIncrement;
            // ExecuteReader(SelectAutoIncrement, reader =>
            // {
            //     if (reader.Read())
            //     {
            //         autoIncrement = reader.GetInt32(0);
            //     }
            // });
            // return autoIncrement;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
