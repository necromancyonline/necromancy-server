using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertCharacter =@"
            INSERT INTO nec_character(id,account_id,soul_id,slot,map_id,x,y,z,name,race_id,sex_id,hair_id,hair_color_id,face_id,strength,vitality,dexterity,agility,intelligence,piety,luck,class_id,level,created,hp_current,mp_current,gold,condition_current,channel,face_arrange_id,voice_id,experience_current,skill_points)
            VALUES(@id,@account_id,@soul_id,@slot,@map_id,@x,@y,@z,@name,@race_id,@sex_id,@hair_id,@hair_color_id,@face_id,@strength,@vitality,@dexterity,@agility,@intelligence,@piety,@luck,@class_id,@level,@created,@hp_current,@mp_current,@gold,@condition_current,@channel,@face_arrange_id,@voice_id,@experience_current,@skill_points)";

        private const string SqlSelectCharacterById =@"
            SELECT * FROM nec_character WHERE id=@id";

        private const string SqlSelectCharactersByAccountId =@"
            SELECT * FROM nec_character WHERE account_id=@account_id";

        private const string SqlSelectCharactersBySoulId =@"
            SELECT * FROM nec_character WHERE soul_id=@soul_id";

        private const string SqlSelectCharacterBySlot =@"
            SELECT * FROM nec_character WHERE soul_id=@soul_id AND slot=@slot";

        private const string SqlUpdateCharacter =
            "UPDATE `nec_character` SET `account_id`=@account_id, `soul_id`=@soul_id, `slot`=@slot, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `name`=@name, `race_id`=@race_id, `sex_id`=@sex_id, `hair_id`=@hair_id, `hair_color_id`=@hair_color_id, `face_id`=@face_id, `strength`=@strength, `vitality`=@vitality, `dexterity`=@dexterity, `agility`=@agility, `intelligence`=@intelligence, `piety`=@piety, `luck`=@luck, `class_id`=@class_id, `level`=@level, `created`=@created, `hp_current`=@hp_current, `mp_current`=@mp_current, `gold`=@gold, `condition_current`=@condition_current, `channel`=@channel, `face_arrange_id`=@face_arrange_id, `voice_id`=@voice_id, `experience_current`=@experience_current, `skill_points`=@skill_points WHERE `id`=@id;";

        private const string SqlDeleteCharacter =
            "DELETE FROM `nec_character` WHERE `id`=@id;";
        private const string SqlSelectCharacters =
            "SELECT * FROM `nec_character`;";

        public bool InsertCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.SoulId);
                AddParameter(command, "@slot", character.Slot);
                AddParameter(command, "@map_id", character.MapId);
                AddParameter(command, "@x", character.X);
                AddParameter(command, "@y", character.Y);
                AddParameter(command, "@z", character.Z);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.RaceId);
                AddParameter(command, "@sex_id", character.SexId);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@strength", character.Strength);
                AddParameter(command, "@vitality", character.Vitality);
                AddParameter(command, "@dexterity", character.Dexterity);
                AddParameter(command, "@agility", character.Agility);
                AddParameter(command, "@intelligence", character.Intelligence);
                AddParameter(command, "@piety", character.Piety);
                AddParameter(command, "@luck", character.Luck);
                AddParameter(command, "@class_id", character.ClassId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@hp_current", character.Hp.current);
                AddParameter(command, "@mp_current", character.Mp.current);
                AddParameter(command, "@gold", character.AdventureBagGold);
                AddParameter(command, "@condition_current", character.Condition.current);
                AddParameter(command, "@channel", character.Channel);
                AddParameter(command, "@face_arrange_id", character.FaceArrangeId);
                AddParameter(command, "@voice_id", character.VoiceId);
                AddParameter(command, "@experience_current", character.ExperienceCurrent);
                AddParameter(command, "@skill_points", character.SkillPoints);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            character.Id = (int) autoIncrement;
            return true;
        }

        public Character SelectCharacterById(int characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacterById,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadCharacter(reader);
                    }
                });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersByAccountId,
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
            ExecuteReader(SqlSelectCharactersBySoulId,
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
            ExecuteReader(SqlSelectCharacterBySlot,
                command =>
                {
                    AddParameter(command, "@soul_id", soulId);
                    AddParameter(command, "@slot", slot);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        characters = ReadCharacter(reader);
                    }
                });
            return characters;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.SoulId);
                AddParameter(command, "@slot", character.Slot);
                AddParameter(command, "@map_id", character.MapId);
                AddParameter(command, "@x", character.X);
                AddParameter(command, "@y", character.Y);
                AddParameter(command, "@z", character.Z);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.RaceId);
                AddParameter(command, "@sex_id", character.SexId);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@strength", character.Strength);
                AddParameter(command, "@vitality", character.Vitality);
                AddParameter(command, "@dexterity", character.Dexterity);
                AddParameter(command, "@agility", character.Agility);
                AddParameter(command, "@intelligence", character.Intelligence);
                AddParameter(command, "@piety", character.Piety);
                AddParameter(command, "@luck", character.Luck);
                AddParameter(command, "@class_id", character.ClassId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@hp_current", character.Hp.current);
                AddParameter(command, "@mp_current", character.Mp.current);
                AddParameter(command, "@gold", character.AdventureBagGold);
                AddParameter(command, "@condition_current", character.Condition.current);
                AddParameter(command, "@channel", character.Channel);
                AddParameter(command, "@face_arrange_id", character.FaceArrangeId);
                AddParameter(command, "@voice_id", character.VoiceId);
                AddParameter(command, "@experience_current", character.ExperienceCurrent);
                AddParameter(command, "@skill_points", character.SkillPoints);
                AddParameter(command, "@id", character.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteCharacter(int characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        public List<Character> SelectCharacters()
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharacters,
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
            character.Id = GetInt32(reader, "id");
            character.AccountId = GetInt32(reader, "account_id");
            character.SoulId = GetInt32(reader, "soul_id");
            character.Created = GetDateTime(reader, "created");
            character.Slot = GetByte(reader, "slot");
            character.MapId = GetInt32(reader, "map_id");
            character.X = GetFloat(reader, "x");
            character.Y = GetFloat(reader, "y");
            character.Z = GetFloat(reader, "z");
            character.Name = GetString(reader, "name");
            character.RaceId = GetByte(reader, "race_id");
            character.SexId = GetByte(reader, "sex_id");
            character.HairId = GetByte(reader, "hair_id");
            character.HairColorId = GetByte(reader, "hair_color_id");
            character.FaceId = GetByte(reader, "face_id");
            character.Strength = GetByte(reader, "strength");
            character.Vitality = GetByte(reader, "vitality");
            character.Dexterity = GetByte(reader, "dexterity");
            character.Agility = GetByte(reader, "agility");
            character.Intelligence = GetByte(reader, "intelligence");
            character.Piety = GetByte(reader, "piety");
            character.Luck = GetByte(reader, "luck");
            character.ClassId = GetByte(reader, "class_id");
            character.Level = GetByte(reader, "level");
            character.Hp.setCurrent(GetInt32(reader, "hp_current"));
            character.Mp.setCurrent(GetInt32(reader, "mp_current"));
            character.AdventureBagGold = GetUInt64(reader, "gold");
            character.Condition.setCurrent(GetInt32(reader, "condition_current"));
            character.Channel = GetInt32(reader, "channel");
            character.FaceArrangeId = GetByte(reader, "face_arrange_id");
            character.VoiceId = GetByte(reader, "voice_id");
            character.ExperienceCurrent = GetUInt64(reader, "experience_current");
            character.SkillPoints = GetUInt32(reader, "skill_points");
            return character;
        }
    }
}
