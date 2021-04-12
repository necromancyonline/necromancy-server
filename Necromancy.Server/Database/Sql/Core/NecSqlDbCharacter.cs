using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_CHARACTER = @"
            INSERT INTO nec_character(account_id,soul_id,slot,map_id,x,y,z,name,race_id,sex_id,hair_id,hair_color_id,face_id,strength,vitality,dexterity,agility,intelligence,piety,luck,class_id,level,created,hp_current,mp_current,gold,condition_current,channel,face_arrange_id,voice_id,experience_current,skill_points)
            VALUES(@account_id,@soul_id,@slot,@map_id,@x,@y,@z,@name,@race_id,@sex_id,@hair_id,@hair_color_id,@face_id,@strength,@vitality,@dexterity,@agility,@intelligence,@piety,@luck,@class_id,@level,@created,@hp_current,@mp_current,@gold,@condition_current,@channel,@face_arrange_id,@voice_id,@experience_current,@skill_points)";

        private const string SQL_SELECT_CHARACTER_BY_ID = @"
            SELECT * FROM nec_character WHERE id=@id";

        private const string SQL_SELECT_CHARACTERS_BY_ACCOUNT_ID = @"
            SELECT * FROM nec_character WHERE account_id=@account_id";

        private const string SQL_SELECT_CHARACTERS_BY_SOUL_ID = @"
            SELECT * FROM nec_character WHERE soul_id=@soul_id";

        private const string SQL_SELECT_CHARACTER_BY_SLOT = @"
            SELECT * FROM nec_character WHERE soul_id=@soul_id AND slot=@slot";

        private const string SQL_UPDATE_CHARACTER =
            "UPDATE `nec_character` SET `account_id`=@account_id, `soul_id`=@soul_id, `slot`=@slot, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `name`=@name, `race_id`=@race_id, `sex_id`=@sex_id, `hair_id`=@hair_id, `hair_color_id`=@hair_color_id, `face_id`=@face_id, `strength`=@strength, `vitality`=@vitality, `dexterity`=@dexterity, `agility`=@agility, `intelligence`=@intelligence, `piety`=@piety, `luck`=@luck, `class_id`=@class_id, `level`=@level, `created`=@created, `hp_current`=@hp_current, `mp_current`=@mp_current, `gold`=@gold, `condition_current`=@condition_current, `channel`=@channel, `face_arrange_id`=@face_arrange_id, `voice_id`=@voice_id, `experience_current`=@experience_current, `skill_points`=@skill_points WHERE `id`=@id;";

        private const string SQL_DELETE_CHARACTER =
            "DELETE FROM `nec_character` WHERE `id`=@id;";

        private const string SQL_SELECT_CHARACTERS =
            "SELECT * FROM `nec_character`;";

        public bool InsertCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_CHARACTER, command =>
            {
                AddParameter(command, "@account_id", character.accountId);
                AddParameter(command, "@soul_id", character.soulId);
                AddParameter(command, "@slot", character.slot);
                AddParameter(command, "@map_id", character.mapId);
                AddParameter(command, "@x", character.x);
                AddParameter(command, "@y", character.y);
                AddParameter(command, "@z", character.z);
                AddParameter(command, "@name", character.name);
                AddParameter(command, "@race_id", character.raceId);
                AddParameter(command, "@sex_id", character.sexId);
                AddParameter(command, "@hair_id", character.hairId);
                AddParameter(command, "@hair_color_id", character.hairColorId);
                AddParameter(command, "@face_id", character.faceId);
                AddParameter(command, "@strength", character.strength);
                AddParameter(command, "@vitality", character.vitality);
                AddParameter(command, "@dexterity", character.dexterity);
                AddParameter(command, "@agility", character.agility);
                AddParameter(command, "@intelligence", character.intelligence);
                AddParameter(command, "@piety", character.piety);
                AddParameter(command, "@luck", character.luck);
                AddParameter(command, "@class_id", character.classId);
                AddParameter(command, "@level", character.level);
                AddParameter(command, "@created", character.created);
                AddParameter(command, "@hp_current", character.hp.current);
                AddParameter(command, "@mp_current", character.mp.current);
                AddParameter(command, "@gold", character.adventureBagGold);
                AddParameter(command, "@condition_current", character.condition.current);
                AddParameter(command, "@channel", character.channel);
                AddParameter(command, "@face_arrange_id", character.faceArrangeId);
                AddParameter(command, "@voice_id", character.voiceId);
                AddParameter(command, "@experience_current", character.experienceCurrent);
                AddParameter(command, "@skill_points", character.skillPoints);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            character.id = (int)autoIncrement;
            return true;
        }

        public Character SelectCharacterById(int characterId)
        {
            Character character = null;
            ExecuteReader(SQL_SELECT_CHARACTER_BY_ID,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read()) character = ReadCharacter(reader);
                });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SQL_SELECT_CHARACTERS_BY_ACCOUNT_ID,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public List<Character> SelectCharactersBySoulId(int soulId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SQL_SELECT_CHARACTERS_BY_SOUL_ID,
                command => { AddParameter(command, "@soul_id", soulId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public Character SelectCharacterBySlot(int soulId, int slot)
        {
            Character characters = null;
            ExecuteReader(SQL_SELECT_CHARACTER_BY_SLOT,
                command =>
                {
                    AddParameter(command, "@soul_id", soulId);
                    AddParameter(command, "@slot", slot);
                }, reader =>
                {
                    if (reader.Read()) characters = ReadCharacter(reader);
                });
            return characters;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_CHARACTER, command =>
            {
                AddParameter(command, "@account_id", character.accountId);
                AddParameter(command, "@soul_id", character.soulId);
                AddParameter(command, "@slot", character.slot);
                AddParameter(command, "@map_id", character.mapId);
                AddParameter(command, "@x", character.x);
                AddParameter(command, "@y", character.y);
                AddParameter(command, "@z", character.z);
                AddParameter(command, "@name", character.name);
                AddParameter(command, "@race_id", character.raceId);
                AddParameter(command, "@sex_id", character.sexId);
                AddParameter(command, "@hair_id", character.hairId);
                AddParameter(command, "@hair_color_id", character.hairColorId);
                AddParameter(command, "@face_id", character.faceId);
                AddParameter(command, "@strength", character.strength);
                AddParameter(command, "@vitality", character.vitality);
                AddParameter(command, "@dexterity", character.dexterity);
                AddParameter(command, "@agility", character.agility);
                AddParameter(command, "@intelligence", character.intelligence);
                AddParameter(command, "@piety", character.piety);
                AddParameter(command, "@luck", character.luck);
                AddParameter(command, "@class_id", character.classId);
                AddParameter(command, "@level", character.level);
                AddParameter(command, "@created", character.created);
                AddParameter(command, "@hp_current", character.hp.current);
                AddParameter(command, "@mp_current", character.mp.current);
                AddParameter(command, "@gold", character.adventureBagGold);
                AddParameter(command, "@condition_current", character.condition.current);
                AddParameter(command, "@channel", character.channel);
                AddParameter(command, "@face_arrange_id", character.faceArrangeId);
                AddParameter(command, "@voice_id", character.voiceId);
                AddParameter(command, "@experience_current", character.experienceCurrent);
                AddParameter(command, "@skill_points", character.skillPoints);
                AddParameter(command, "@id", character.id);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteCharacter(int characterId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_CHARACTER,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public List<Character> SelectCharacters()
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SQL_SELECT_CHARACTERS,
                command => { }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();
            character.id = GetInt32(reader, "id");
            character.accountId = GetInt32(reader, "account_id");
            character.soulId = GetInt32(reader, "soul_id");
            character.created = GetDateTime(reader, "created");
            character.slot = GetByte(reader, "slot");
            character.mapId = GetInt32(reader, "map_id");
            character.x = GetFloat(reader, "x");
            character.y = GetFloat(reader, "y");
            character.z = GetFloat(reader, "z");
            character.name = GetString(reader, "name");
            character.raceId = GetByte(reader, "race_id");
            character.sexId = GetByte(reader, "sex_id");
            character.hairId = GetByte(reader, "hair_id");
            character.hairColorId = GetByte(reader, "hair_color_id");
            character.faceId = GetByte(reader, "face_id");
            character.strength = GetByte(reader, "strength");
            character.vitality = GetByte(reader, "vitality");
            character.dexterity = GetByte(reader, "dexterity");
            character.agility = GetByte(reader, "agility");
            character.intelligence = GetByte(reader, "intelligence");
            character.piety = GetByte(reader, "piety");
            character.luck = GetByte(reader, "luck");
            character.classId = GetByte(reader, "class_id");
            character.level = GetByte(reader, "level");
            character.hp.SetMax(GetInt32(reader, "hp_current")); //Temporary until Max HP calc is created
            character.mp.SetMax(GetInt32(reader, "mp_current")); //Temporary until Max HP calc is created
            character.hp.SetCurrent(GetInt32(reader, "hp_current"));
            character.mp.SetCurrent(GetInt32(reader, "mp_current"));
            character.adventureBagGold = GetUInt64(reader, "gold");
            character.condition.SetCurrent(GetInt32(reader, "condition_current"));
            character.channel = GetInt32(reader, "channel");
            character.faceArrangeId = GetByte(reader, "face_arrange_id");
            character.voiceId = GetByte(reader, "voice_id");
            character.experienceCurrent = GetUInt64(reader, "experience_current");
            character.skillPoints = GetUInt32(reader, "skill_points");
            return character;
        }
    }
}
