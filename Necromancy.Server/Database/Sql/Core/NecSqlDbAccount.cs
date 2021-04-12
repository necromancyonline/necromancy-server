using System;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertAccount =
            "INSERT INTO `account` (`name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created`) VALUES (@name, @normal_name, @hash, @mail, @mail_verified, @mail_verified_at, @mail_token, @password_token, @state, @last_login, @created);";

        private const string _SqlSelectAccountById =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `id`=@id;";

        private const string _SqlSelectAccountByName =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account` WHERE `name`=@name;";

        private const string _SqlUpdateAccount =
            "UPDATE `account` SET `name`=@name, `normal_name`=@normal_name, `hash`=@hash, `mail`=@mail, `mail_verified`=@mail_verified, `mail_verified_at`=@mail_verified_at, `mail_token`=@mail_token, `password_token`=@password_token, `state`=@state, `last_login`=@last_login, `created`=@created WHERE `id`=@id;";

        private const string _SqlDeleteAccount =
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
            int rowsAffected = ExecuteNonQuery(_SqlInsertAccount, command =>
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
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement) return null;

            account.id = (int)autoIncrement;
            return account;
        }

        public Account SelectAccountByName(string accountName)
        {
            Account account = null;
            ExecuteReader(_SqlSelectAccountByName,
                command => { AddParameter(command, "@name", accountName); }, reader =>
                {
                    if (reader.Read()) account = ReadAccount(reader);
                });

            return account;
        }

        public Account SelectAccountById(int accountId)
        {
            Account account = null;
            ExecuteReader(_SqlSelectAccountById, command => { AddParameter(command, "@id", accountId); }, reader =>
            {
                if (reader.Read()) account = ReadAccount(reader);
            });
            return account;
        }

        public bool UpdateAccount(Account account)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateAccount, command =>
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
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteAccount(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteAccount,
                command => { AddParameter(command, "@id", accountId); });
            return rowsAffected > NoRowsAffected;
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
