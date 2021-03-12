using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;


namespace Necromancy.Server.Systems
{
    //CLASS IS JUST FOR TESTING ABSTRACTING OF DATABASE FUNCTIONS
    //TODO MOVE UNDER /DATABASE
    public class DatabaseAccessObject
    {
        protected static readonly ILogger Logger = LogProvider.Logger(typeof(DatabaseAccessObject));

        protected const int NO_ROWS_AFFECTED = 0;
        
        private readonly string _sqLiteConnectionString;

        public DatabaseAccessObject()
        {
            _sqLiteConnectionString = makeSQLiteConnectionString();
        }

        private string makeSQLiteConnectionString()
        {
            //TODO move info to resources
            SQLiteConnectionStringBuilder sqLiteConnStrBuilder = new SQLiteConnectionStringBuilder();
            sqLiteConnStrBuilder.DataSource = "Database\\db.sqlite";            
            sqLiteConnStrBuilder.Version = 3; 
            sqLiteConnStrBuilder.Pooling = true;
            sqLiteConnStrBuilder.ForeignKeys = true;
            sqLiteConnStrBuilder.Flags = sqLiteConnStrBuilder.Flags & SQLiteConnectionFlags.StrictConformance;
            return sqLiteConnStrBuilder.ConnectionString;
        }

        protected DbConnection GetSQLConnection()
        {
            return new SQLiteConnection(_sqLiteConnectionString);
        }

        public int ExecuteNonQuery(string query, Action<DbCommand> nonQueryAction)
        {
            try
            {
                using DbConnection conn = new SQLiteConnection(_sqLiteConnectionString);
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.CommandText = query;
                nonQueryAction(command);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {query}");
                Exception(ex);
                return NO_ROWS_AFFECTED;
            }
        }

        public void ExecuteReader(string query, Action<DbCommand> nonQueryAction, Action<DbDataReader> readAction)
        {
            try
            {
                using DbConnection conn = new SQLiteConnection(_sqLiteConnectionString);

                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.CommandText = query;
                nonQueryAction(command);
                using DbDataReader reader = command.ExecuteReader();
                readAction(reader);    
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {query}");
                Exception(ex);
            }
        }

        public void Execute(string query)
        {
            try
            {
                using DbConnection conn = new SQLiteConnection(_sqLiteConnectionString);
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.ExecuteNonQuery();                    
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {query}");
                Exception(ex);
            }
        }

        protected virtual void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }

        protected DbParameter Parameter(DbCommand command, string name, object value, DbType type)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            return parameter;
        }

        protected DbParameter Parameter(DbCommand command, string name, string value)
        {
            return Parameter(command, name, value, DbType.String);
        }

        protected void AddParameter(DbCommand command, string name, object value, DbType type)
        {
            DbParameter parameter = Parameter(command, name, value, type);
            command.Parameters.Add(parameter);
        }

        protected void AddParameter(DbCommand command, string name, string value)
        {
            AddParameter(command, name, value, DbType.String);
        }

        protected void AddParameter(DbCommand command, string name, Int32 value)
        {
            AddParameter(command, name, value, DbType.Int32);
        }

        protected void AddParameter(DbCommand command, string name, float value)
        {
            AddParameter(command, name, value, DbType.Double);
        }

        protected void AddParameter(DbCommand command, string name, byte value)
        {
            AddParameter(command, name, value, DbType.Byte);
        }

        protected void AddParameter(DbCommand command, string name, UInt32 value)
        {
            AddParameter(command, name, value, DbType.UInt32);
        }

        protected void AddParameterEnumInt32<T>(DbCommand command, string name, T value) where T : Enum
        {
            AddParameter(command, name, (Int32)(object)value, DbType.Int32);
        }

        protected void AddParameter(DbCommand command, string name, DateTime? value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(DbCommand command, string name, DateTime value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(DbCommand command, string name, bool value)
        {
            AddParameter(command, name, value, DbType.Boolean);
        }

        protected void AddParameterNull(DbCommand command, string name)
        {
            AddParameter(command, name, DBNull.Value, DbType.Object);
        }
    }
}
