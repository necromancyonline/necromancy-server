using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertShortcutBar =
            "INSERT INTO `nec_shortcut_bar` (`slot0`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot9`) VALUES (@slot0, @slot1, @slot2, @slot3, @slot4, @slot5, @slot6, @slot7, @slot8, @slot9 );";

        private const string SqlSelectShortcutBarById =
            "SELECT `id`, `slot0`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot9` FROM `nec_shortcut_bar` WHERE `id`=@id;";
        private const string SqlUpdateShortcutBar =
            "UPDATE `nec_shortcut_bar` SET `slot0`=@slot0, `slot1`=@slot1, `slot2`=@slot2, `slot3`=@slot3, `slot4`=@slot4, `slot5`=@slot5, `slot6`=@slot6, `slot7`=@slot7, `slot8`=@slot8, `slot9`=@slot9, WHERE `id`=@id;";

        private const string SqlDeleteShortcutBar =
            "DELETE FROM `nec_shortcut_bar` WHERE `id`=@id;";

        public bool InsertShortcutBar(ShortcutBar shortcutBar)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertShortcutBar, command =>
            {
                AddParameter(command, "@slot0", shortcutBar.Slot0);
                AddParameter(command, "@slot1", shortcutBar.Slot1);
                AddParameter(command, "@slot2", shortcutBar.Slot2);
                AddParameter(command, "@slot3", shortcutBar.Slot3);
                AddParameter(command, "@slot4", shortcutBar.Slot4);
                AddParameter(command, "@slot5", shortcutBar.Slot5);
                AddParameter(command, "@slot6", shortcutBar.Slot6);
                AddParameter(command, "@slot7", shortcutBar.Slot7);
                AddParameter(command, "@slot8", shortcutBar.Slot8);
                AddParameter(command, "@slot9", shortcutBar.Slot9);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            shortcutBar.Id = (int) autoIncrement;
            return true;
        }
        
        public ShortcutBar SelectShortcutBarById(int shortcutBarId)
        {
            ShortcutBar shortcutBar = null;
            ExecuteReader(SqlSelectShortcutBarById,
                command => { AddParameter(command, "@id", shortcutBarId); }, reader =>
                {
                    if (reader.Read())
                    {
                        shortcutBar = ReadShortcutBar(reader);
                    }
                });
            return shortcutBar;
        }
        public bool UpdateShortcutBar(ShortcutBar shortcutBar)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateShortcutBar, command =>
            {
                AddParameter(command, "@id", shortcutBar.Id);
                AddParameter(command, "@slot0", shortcutBar.Slot0);
                AddParameter(command, "@slot1", shortcutBar.Slot1);
                AddParameter(command, "@slot2", shortcutBar.Slot2);
                AddParameter(command, "@slot3", shortcutBar.Slot3);
                AddParameter(command, "@slot4", shortcutBar.Slot4);
                AddParameter(command, "@slot5", shortcutBar.Slot5);
                AddParameter(command, "@slot6", shortcutBar.Slot6);
                AddParameter(command, "@slot7", shortcutBar.Slot7);
                AddParameter(command, "@slot8", shortcutBar.Slot8);
                AddParameter(command, "@slot9", shortcutBar.Slot9);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteShortcutBar(int shortcutBarId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteShortcutBar, command => { AddParameter(command, "@id", shortcutBarId); });
            return rowsAffected > NoRowsAffected;
        }

        private ShortcutBar ReadShortcutBar(DbDataReader reader)
        {
            {
                ShortcutBar shortcutBar = new ShortcutBar();
                shortcutBar.Id = GetInt32(reader, "id");
                shortcutBar.Slot0 = GetInt32(reader, "slot0");
                shortcutBar.Slot1 = GetInt32(reader, "slot1");
                shortcutBar.Slot2 = GetInt32(reader, "slot2");
                shortcutBar.Slot3 = GetInt32(reader, "slot3");
                shortcutBar.Slot4 = GetInt32(reader, "slot4");
                shortcutBar.Slot5 = GetInt32(reader, "slot5");
                shortcutBar.Slot6 = GetInt32(reader, "slot6");
                shortcutBar.Slot7 = GetInt32(reader, "slot7");
                shortcutBar.Slot8 = GetInt32(reader, "slot8");
                shortcutBar.Slot9 = GetInt32(reader, "slot9");
                return shortcutBar;
            }
        }
    }
}
