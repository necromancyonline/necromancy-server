using System;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_ACCOUNT =
            "INSERT INTO `account` (`name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created`) VALUES (@name, @normal_name, @hash, @mail, @mail_verified, @mail_verified_at, @mail_token, @password_token, @state, @last_login, @created);";

        private const string SQL_SELECT_ACCOUNT_BY_ID =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `id`=@id;";

        private const string SQL_SELECT_ACCOUNT_BY_NAME =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `name`=@name;";

        private const string SQL_UPDATE_ACCOUNT =
            "UPDATE `account` SET `name`=@name, `normal_name`=@normal_name, `hash`=@hash, `mail`=@mail, `mail_verified`=@mail_verified, `mail_verified_at`=@mail_verified_at, `mail_token`=@mail_token, `password_token`=@password_token, `state`=@state, `last_login`=@last_login, `created`=@created WHERE `id`=@id;";

        private const string SQL_DELETE_ACCOUNT =
            "DELETE FROM `account` WHERE `id`=@id;";

        public Account CreateAccount(string name, string mail, string hash)
        {
            Account account = new Account();
            account.name = name;
            account.normalName = name.ToLowerInvariant();
            account.mail = mail;
            account.hash = hash;
            account.state = AccountStateType.User;
            account.created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_ACCOUNT, command =>
            {
                AddParameter(command, "@name", account.name);
                AddParameter(command, "@normal_name", account.normalName);
                AddParameter(command, "@hash", account.hash);
                AddParameter(command, "@mail", account.mail);
                AddParameter(command, "@mail_verified", account.mailVerified);
                AddParameter(command, "@mail_verified_at", account.mailVerifiedAt);
                AddParameter(command, "@mail_token", account.mailToken);
                AddParameter(command, "@password_token", account.passwordToken);
                AddParameterEnumInt32(command, "@state", account.state);
                AddParameter(command, "@last_login", account.lastLogin);
                AddParameter(command, "@created", account.created);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return null;

            account.id = (int)autoIncrement;
            return account;
        }

        public Account SelectAccountByName(string accountName)
        {
            Account account = null;
            ExecuteReader(SQL_SELECT_ACCOUNT_BY_NAME,
                command => { AddParameter(command, "@name", accountName); }, reader =>
                {
                    if (reader.Read()) account = ReadAccount(reader);
                });

            return account;
        }

        public Account SelectAccountById(int accountId)
        {
            Account account = null;
            ExecuteReader(SQL_SELECT_ACCOUNT_BY_ID, command => { AddParameter(command, "@id", accountId); }, reader =>
            {
                if (reader.Read()) account = ReadAccount(reader);
            });
            return account;
        }

        public bool UpdateAccount(Account account)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_ACCOUNT, command =>
            {
                AddParameter(command, "@name", account.name);
                AddParameter(command, "@normal_name", account.normalName);
                AddParameter(command, "@hash", account.hash);
                AddParameter(command, "@mail", account.mail);
                AddParameter(command, "@mail_verified", account.mailVerified);
                AddParameter(command, "@mail_verified_at", account.mailVerifiedAt);
                AddParameter(command, "@mail_token", account.mailToken);
                AddParameter(command, "@password_token", account.passwordToken);
                AddParameterEnumInt32(command, "@state", account.state);
                AddParameter(command, "@last_login", account.lastLogin);
                AddParameter(command, "@created", account.created);
                AddParameter(command, "@id", account.id);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteAccount(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_ACCOUNT,
                command => { AddParameter(command, "@id", accountId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private Account ReadAccount(DbDataReader reader)
        {
            Account account = new Account();
            account.id = GetInt32(reader, "id");
            account.name = GetString(reader, "name");
            account.normalName = GetString(reader, "normal_name");
            account.hash = GetString(reader, "hash");
            account.mail = GetString(reader, "mail");
            account.mailVerified = GetBoolean(reader, "mail_verified");
            account.mailVerifiedAt = GetDateTimeNullable(reader, "mail_verified_at");
            account.mailToken = GetStringNullable(reader, "mail_token");
            account.passwordToken = GetStringNullable(reader, "password_token");
            account.state = (AccountStateType)GetInt32(reader, "state");
            account.lastLogin = GetDateTimeNullable(reader, "last_login");
            account.created = GetDateTime(reader, "created");
            return account;
        }
    }
}
