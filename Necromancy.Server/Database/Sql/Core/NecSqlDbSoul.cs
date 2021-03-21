using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertSoul =@"
            INSERT INTO 
                nec_soul(account_id,name,level,created,password,experience_current,warehouse_gold,points_lawful,points_neutral,points_chaos,criminal_level,points_current,material_life,material_reincarnation,material_lawful,material_chaos)
            VALUES(@account_id,@name,@level,@created,@password,@experience_current,@warehouse_gold,@points_lawful,@points_neutral,@points_chaos,@criminal_level,@points_current,@material_life,@material_reincarnation,@material_lawful,@material_chaos)";

        private const string SqlSelectSoulById =@"
            SELECT * FROM nec_soul WHERE id=@id";

        private const string SqlSelectSoulByName = @"
            SELECT * FROM nec_soul WHERE name=@name";

        private const string SqlSelectSoulsByAccountId = @"
            SELECT * FROM nec_soul WHERE account_id=@account_id";

        private const string SqlUpdateSoul =
            "UPDATE `nec_soul` SET `account_id`=@account_id, `name`=@name, `level`=@level, `created`=@created, `password`=@password WHERE `id`=@id;";

        private const string SqlDeleteSoul =
            "DELETE FROM `nec_soul` WHERE `id`=@id;";

        public bool InsertSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertSoul, command =>
            {
                AddParameter(command, "@account_id", soul.AccountId);
                AddParameter(command, "@name", soul.Name);
                AddParameter(command, "@level", soul.Level);
                AddParameter(command, "@created", soul.Created);
                AddParameter(command, "@password", soul.Password);
                AddParameter(command, "@experience_current", soul.ExperienceCurrent);
                AddParameter(command, "@warehouse_gold", soul.WarehouseGold);
                AddParameter(command, "@points_lawful", soul.PointsLawful);
                AddParameter(command, "@points_neutral", soul.PointsNeutral);
                AddParameter(command, "@points_chaos", soul.PointsChaos);
                AddParameter(command, "@criminal_level", soul.CriminalLevel);
                AddParameter(command, "@points_current", soul.PointsCurrent);
                AddParameter(command, "@material_life", soul.MaterialLife);
                AddParameter(command, "@material_reincarnation", soul.MaterialReincarnation);
                AddParameter(command, "@material_lawful", soul.MaterialLawful);
                AddParameter(command, "@material_chaos", soul.MaterialChaos);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            soul.Id = (int) autoIncrement;
            return true;
        }
        
        public Soul SelectSoulById(int soulId)
        {
            Soul soul = null;
            ExecuteReader(SqlSelectSoulById,
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
            ExecuteReader(SqlSelectSoulByName,
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
            ExecuteReader(SqlSelectSoulsByAccountId,
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
            int rowsAffected = ExecuteNonQuery(SqlUpdateSoul, command =>
            {
                AddParameter(command, "@account_id", soul.AccountId);
                AddParameter(command, "@name", soul.Name);
                AddParameter(command, "@level", soul.Level);
                AddParameter(command, "@created", soul.Created);
                AddParameter(command, "@id", soul.Id);
                AddParameter(command, "@password", soul.Password);
                AddParameter(command, "@experience_current", soul.ExperienceCurrent);
                AddParameter(command, "@warehouse_gold", soul.WarehouseGold);
                AddParameter(command, "@points_lawful", soul.PointsLawful);
                AddParameter(command, "@points_neutral", soul.PointsNeutral);
                AddParameter(command, "@points_chaos", soul.PointsChaos);
                AddParameter(command, "@criminal_level", soul.CriminalLevel);
                AddParameter(command, "@points_current", soul.PointsCurrent);
                AddParameter(command, "@material_life", soul.MaterialLife);
                AddParameter(command, "@material_reincarnation", soul.MaterialReincarnation);
                AddParameter(command, "@material_lawful", soul.MaterialLawful);
                AddParameter(command, "@material_chaos", soul.MaterialChaos);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteSoul(int soulId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteSoul, command => { AddParameter(command, "@id", soulId); });
            return rowsAffected > NoRowsAffected;
        }

        private Soul ReadSoul(DbDataReader reader)
        {
            {
                Soul soul = new Soul();
                soul.Id = GetInt32(reader, "id");
                soul.AccountId = GetInt32(reader, "account_id");
                soul.Name = GetString(reader, "name");
                soul.Level = GetByte(reader, "level");
                soul.Created = GetDateTime(reader, "created");
                soul.Password = GetStringNullable(reader, "password");
                soul.ExperienceCurrent = GetUInt64(reader, "experience_current");
                soul.WarehouseGold = GetUInt64(reader, "warehouse_gold");
                soul.PointsLawful = GetInt32(reader, "points_lawful");
                soul.PointsNeutral = GetInt32(reader, "points_neutral");
                soul.PointsChaos = GetInt32(reader, "points_chaos");
                soul.CriminalLevel = GetByte(reader, "criminal_level");
                soul.PointsCurrent = GetInt32(reader, "points_current");
                soul.MaterialLife = GetInt32(reader, "material_life");
                soul.MaterialReincarnation = GetInt32(reader, "material_reincarnation");
                soul.MaterialLawful = GetInt32(reader, "material_lawful");
                soul.MaterialChaos = GetInt32(reader, "material_chaos");

                return soul;
            }
        }
    }
}
