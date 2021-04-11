using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertSoul =@"
            INSERT INTO
                nec_soul(account_id,name,level,created,password,experience_current,warehouse_gold,points_lawful,points_neutral,points_chaos,criminal_level,points_current,material_life,material_reincarnation,material_lawful,material_chaos)
            VALUES(@account_id,@name,@level,@created,@password,@experience_current,@warehouse_gold,@points_lawful,@points_neutral,@points_chaos,@criminal_level,@points_current,@material_life,@material_reincarnation,@material_lawful,@material_chaos)";

        private const string _SqlSelectSoulById =@"
            SELECT * FROM nec_soul WHERE id=@id";

        private const string _SqlSelectSoulByName = @"
            SELECT * FROM nec_soul WHERE name=@name";

        private const string _SqlSelectSoulsByAccountId = @"
            SELECT * FROM nec_soul WHERE account_id=@account_id";

        private const string _SqlUpdateSoul =
            "UPDATE `nec_soul` SET `account_id`=@account_id, `name`=@name, `level`=@level, `created`=@created, `password`=@password WHERE `id`=@id;";

        private const string _SqlDeleteSoul =
            "DELETE FROM `nec_soul` WHERE `id`=@id;";

        public bool InsertSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertSoul, command =>
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
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            soul.id = (int) autoIncrement;
            return true;
        }

        public Soul SelectSoulById(int soulId)
        {
            Soul soul = null;
            ExecuteReader(_SqlSelectSoulById,
                command => { AddParameter(command, "@id", soulId); }, reader =>
                {
                    if (reader.Read())
                    {
                        soul = ReadSoul(reader);
                    }
                });
            return soul;
        }

        public Soul SelectSoulByName(string soulName)
        {
            Soul soul = null;
            ExecuteReader(_SqlSelectSoulByName,
                command => { AddParameter(command, "@name", soulName); }, reader =>
                {
                    if (reader.Read())
                    {
                        soul = ReadSoul(reader);
                    }
                });
            return soul;
        }

        public List<Soul> SelectSoulsByAccountId(int accountId)
        {
            List<Soul> souls = new List<Soul>();
            ExecuteReader(_SqlSelectSoulsByAccountId,
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
            int rowsAffected = ExecuteNonQuery(_SqlUpdateSoul, command =>
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
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteSoul(int soulId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteSoul, command => { AddParameter(command, "@id", soulId); });
            return rowsAffected > NoRowsAffected;
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
