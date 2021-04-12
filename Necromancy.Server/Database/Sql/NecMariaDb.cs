using System;
using MySql.Data.MySqlClient;
using Necromancy.Server.Database.Sql.Core;

namespace Necromancy.Server.Database.Sql
{
    /// <summary>
    ///     SQLite Necromancy database.
    /// </summary>
    public class NecMariaDb : NecSqlDb<MySqlConnection, MySqlCommand>, IDatabase
    {
        public const string MemoryDatabasePath = ":memory:";

        private const string _SelectAutoIncrement = "SELECT last_insert_rowid()";


        private readonly string _connectionString;

        public NecMariaDb(string host, short port, string user, string password, string database)
        {
            _connectionString = $"host={host};port={port};user id={user};password={password};database={database};";
        }

        public long version
        {
            get => long.Parse(Command("SELECT @@GLOBAL.user_version;", Connection()).ExecuteScalar().ToString());
            set => Command(String.Format("SET GLOBAL user_version = {0};", value), Connection()).ExecuteNonQuery();
        }

        public bool CreateDatabase()
        {
            throw new NotImplementedException();
        }

        protected override MySqlConnection Connection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        protected override MySqlCommand Command(string query, MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            return command;
        }

        protected override long AutoIncrement(MySqlConnection connection, MySqlCommand command)
        {
            return command.LastInsertedId;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
