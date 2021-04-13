using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_SOUL = @"
            INSERT INTO
                nec_soul(account_id,name,level,created,password,experience_current,warehouse_gold,points_lawful,points_neutral,points_chaos,criminal_level,points_current,material_life,material_reincarnation,material_lawful,material_chaos)
            VALUES(@account_id,@name,@level,@created,@password,@experience_current,@warehouse_gold,@points_lawful,@points_neutral,@points_chaos,@criminal_level,@points_current,@material_life,@material_reincarnation,@material_lawful,@material_chaos)";

        private const string SQL_SELECT_SOUL_BY_ID = @"
            SELECT * FROM nec_soul WHERE id=@id";

        private const string SQL_SELECT_SOUL_BY_NAME = @"
            SELECT * FROM nec_soul WHERE name=@name";

        private const string SQL_SELECT_SOULS_BY_ACCOUNT_ID = @"
            SELECT * FROM nec_soul WHERE account_id=@account_id";

        private const string SQL_UPDATE_SOUL =
            "UPDATE `nec_soul` SET `account_id`=@account_id, `name`=@name, `level`=@level, `created`=@created, `password`=@password WHERE `id`=@id;";

        private const string SQL_DELETE_SOUL =
            "DELETE FROM `nec_soul` WHERE `id`=@id;";

        public bool InsertSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_SOUL, command =>
            {
                AddParameter(command, "@account_id", soul.accountId);
                AddParameter(command, "@name", soul.name);
                AddParameter(command, "@level", soul.level);
                AddParameter(command, "@created", soul.created);
                AddParameter(command, "@password", soul.password);
                AddParameter(command, "@experience_current", soul.experienceCurrent);
                AddParameter(command, "@warehouse_gold", soul.warehouseGold);
                AddParameter(command, "@points_lawful", soul.pointsLawful);
                AddParameter(command, "@points_neutral", soul.pointsNeutral);
                AddParameter(command, "@points_chaos", soul.pointsChaos);
                AddParameter(command, "@criminal_level", soul.criminalLevel);
                AddParameter(command, "@points_current", soul.pointsCurrent);
                AddParameter(command, "@material_life", soul.materialLife);
                AddParameter(command, "@material_reincarnation", soul.materialReincarnation);
                AddParameter(command, "@material_lawful", soul.materialLawful);
                AddParameter(command, "@material_chaos", soul.materialChaos);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            soul.id = (int)autoIncrement;
            return true;
        }

        public Soul SelectSoulById(int soulId)
        {
            Soul soul = null;
            ExecuteReader(SQL_SELECT_SOUL_BY_ID,
                command => { AddParameter(command, "@id", soulId); }, reader =>
                {
                    if (reader.Read()) soul = ReadSoul(reader);
                });
            return soul;
        }

        public Soul SelectSoulByName(string soulName)
        {
            Soul soul = null;
            ExecuteReader(SQL_SELECT_SOUL_BY_NAME,
                command => { AddParameter(command, "@name", soulName); }, reader =>
                {
                    if (reader.Read()) soul = ReadSoul(reader);
                });
            return soul;
        }

        public List<Soul> SelectSoulsByAccountId(int accountId)
        {
            List<Soul> souls = new List<Soul>();
            ExecuteReader(SQL_SELECT_SOULS_BY_ACCOUNT_ID,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Soul soul = ReadSoul(reader);
                        souls.Add(soul);
                    }
                });
            return souls;
        }

        public bool UpdateSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_SOUL, command =>
            {
                AddParameter(command, "@account_id", soul.accountId);
                AddParameter(command, "@name", soul.name);
                AddParameter(command, "@level", soul.level);
                AddParameter(command, "@created", soul.created);
                AddParameter(command, "@id", soul.id);
                AddParameter(command, "@password", soul.password);
                AddParameter(command, "@experience_current", soul.experienceCurrent);
                AddParameter(command, "@warehouse_gold", soul.warehouseGold);
                AddParameter(command, "@points_lawful", soul.pointsLawful);
                AddParameter(command, "@points_neutral", soul.pointsNeutral);
                AddParameter(command, "@points_chaos", soul.pointsChaos);
                AddParameter(command, "@criminal_level", soul.criminalLevel);
                AddParameter(command, "@points_current", soul.pointsCurrent);
                AddParameter(command, "@material_life", soul.materialLife);
                AddParameter(command, "@material_reincarnation", soul.materialReincarnation);
                AddParameter(command, "@material_lawful", soul.materialLawful);
                AddParameter(command, "@material_chaos", soul.materialChaos);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteSoul(int soulId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_SOUL, command => { AddParameter(command, "@id", soulId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private Soul ReadSoul(DbDataReader reader)
        {
            {
                Soul soul = new Soul();
                soul.id = GetInt32(reader, "id");
                soul.accountId = GetInt32(reader, "account_id");
                soul.name = GetString(reader, "name");
                soul.level = GetByte(reader, "level");
                soul.created = GetDateTime(reader, "created");
                soul.password = GetStringNullable(reader, "password");
                soul.experienceCurrent = GetUInt64(reader, "experience_current");
                soul.warehouseGold = GetUInt64(reader, "warehouse_gold");
                soul.pointsLawful = GetInt32(reader, "points_lawful");
                soul.pointsNeutral = GetInt32(reader, "points_neutral");
                soul.pointsChaos = GetInt32(reader, "points_chaos");
                soul.criminalLevel = GetByte(reader, "criminal_level");
                soul.pointsCurrent = GetInt32(reader, "points_current");
                soul.materialLife = GetInt32(reader, "material_life");
                soul.materialReincarnation = GetInt32(reader, "material_reincarnation");
                soul.materialLawful = GetInt32(reader, "material_lawful");
                soul.materialChaos = GetInt32(reader, "material_chaos");

                return soul;
            }
        }
    }
}
