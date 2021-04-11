using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class ItemLibrarySettingCsvReader : CsvReader<ItemLibrarySetting>
    {
        protected override int numExpectedItems => 102;
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(ItemLibrarySettingCsvReader));


        protected override ItemLibrarySetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) { _Logger.Debug("failed here [  0] ");return null; }

            //Equipment slot Settings
            string itemType = properties[2];
            string equipmentType = properties[3];

            string name = properties[5];

            //Core Attributes
            string rarity = properties[6];
            if (!int.TryParse(properties[8], out int physicalAttack)) { _Logger.Debug("failed here [8  ] ");return null; }
            if (!int.TryParse(properties[9], out int magicalAttack)) { _Logger.Debug("failed here [  9] ");return null; }
            if (!int.TryParse(properties[10], out int rangeDistance)) { _Logger.Debug("failed here [  10] ");return null; }
            if (!int.TryParse(properties[11], out int specialPerformance)) { _Logger.Debug("failed here [11  ] ");return null; }
            if (!int.TryParse(properties[12], out int durability)) { _Logger.Debug("failed here [ 12 ] ");return null; }
            if (!int.TryParse(properties[13], out int hardness)) { _Logger.Debug("failed here [ 13 ] ");return null; }
            if (!float.TryParse(properties[14], out float weight)) { _Logger.Debug("failed here [ 14 ] ");return null; }

            //Attack Type
            if (!int.TryParse(properties[15], out int slash)) { _Logger.Debug("failed here [ 15 ] ");return null; }
            if (!int.TryParse(properties[16], out int strike)) { _Logger.Debug("failed here [  16] ");return null; }
            if (!int.TryParse(properties[17], out int pierce)) { _Logger.Debug("failed here [  17] ");return null; }

            //UnKnown
            if (!int.TryParse(properties[19], out int bonusTolerance)) { _Logger.Debug("failed here [19  ] ");return null; }

            //Equip restrictions
            if (!int.TryParse(properties[20], out int fig)) { _Logger.Debug("failed here [  20] ");return null; }
            if (!int.TryParse(properties[21], out int thi)) { _Logger.Debug("failed here [  21] ");return null; }
            if (!int.TryParse(properties[22], out int mag)) { _Logger.Debug("failed here [  22] ");return null; }
            if (!int.TryParse(properties[23], out int pri)) { _Logger.Debug("failed here [  23] ");return null; }
            if (!int.TryParse(properties[24], out int sam)) { _Logger.Debug("failed here [  24] ");return null; }
            if (!int.TryParse(properties[25], out int nin)) { _Logger.Debug("failed here [  25] ");return null; }
            if (!int.TryParse(properties[26], out int bis)) { _Logger.Debug("failed here [  26] ");return null; }
            if (!int.TryParse(properties[27], out int lor)) { _Logger.Debug("failed here [  27] ");return null; }
            if (!int.TryParse(properties[28], out int clo)) { _Logger.Debug("failed here [  28] ");return null; }
            if (!int.TryParse(properties[29], out int alc)) { _Logger.Debug("failed here [  29] ");return null; }

            //Occupation Restrictions
            if (!int.TryParse(properties[30], out int occupation)) { _Logger.Debug("failed here [  30] ");return null; }

            //Bonus Stat gains
            if (!int.TryParse(properties[31], out int hp)) { _Logger.Debug("failed here [  31] ");return null; }
            if (!int.TryParse(properties[32], out int mp)) { _Logger.Debug("failed here [  32] ");return null; }
            if (!int.TryParse(properties[33], out int str)) { _Logger.Debug("failed here [  33] ");return null; }
            if (!int.TryParse(properties[34], out int vit)) { _Logger.Debug("failed here [  34] ");return null; }
            if (!int.TryParse(properties[35], out int dex)) { _Logger.Debug("failed here [  35] ");return null; }
            if (!int.TryParse(properties[36], out int agi)) { _Logger.Debug("failed here [  36] ");return null; }
            if (!int.TryParse(properties[37], out int iNt)) { _Logger.Debug("failed here [  37] ");return null; }
            if (!int.TryParse(properties[38], out int pie)) { _Logger.Debug("failed here [  38] ");return null; }
            if (!int.TryParse(properties[39], out int luk)) { _Logger.Debug("failed here [  39] ");return null; }

            //Bonus Skills on Attack
            if (!int.TryParse(properties[40], out int poison)) { _Logger.Debug("failed here [ 40 ] ");return null; }
            if (!int.TryParse(properties[41], out int paralysis)) { _Logger.Debug("failed here [41  ] ");return null; }
            if (!int.TryParse(properties[42], out int stone)) { _Logger.Debug("failed here [  42] ");return null; }
            if (!int.TryParse(properties[43], out int faint)) { _Logger.Debug("failed here [  43] ");return null; }
            if (!int.TryParse(properties[44], out int blind)) { _Logger.Debug("failed here [  44] ");return null; }
            if (!int.TryParse(properties[45], out int sleep)) { _Logger.Debug("failed here [  45] ");return null; }
            if (!int.TryParse(properties[46], out int charm)) { _Logger.Debug("failed here [  46] ");return null; }
            if (!int.TryParse(properties[47], out int confusion)) { _Logger.Debug("failed here [  47] ");return null; }
            if (!int.TryParse(properties[48], out int fear)) { _Logger.Debug("failed here [  48] ");return null; }

            //Bonus Elemental Defence
            if (!int.TryParse(properties[50], out int fireDef)) { _Logger.Debug("failed here [  50] ");return null; }
            if (!int.TryParse(properties[51], out int waterDef)) { _Logger.Debug("failed here [  51] ");return null; }
            if (!int.TryParse(properties[52], out int windDef)) { _Logger.Debug("failed here [  52] ");return null; }
            if (!int.TryParse(properties[53], out int earthDef)) { _Logger.Debug("failed here [  53] ");return null; }
            if (!int.TryParse(properties[54], out int lightDef)) { _Logger.Debug("failed here [  54] ");return null; }
            if (!int.TryParse(properties[55], out int darkDef)) { _Logger.Debug("failed here [  55] ");return null; }

            //Bonus Elemental Attack
            if (!int.TryParse(properties[56], out int fireAtk)) { _Logger.Debug("failed here [  56] ");return null; }
            if (!int.TryParse(properties[57], out int waterAtk)) { _Logger.Debug("failed here [  57] ");return null; }
            if (!int.TryParse(properties[58], out int windAtk)) { _Logger.Debug("failed here [  58] ");return null; }
            if (!int.TryParse(properties[59], out int earthAtk)) { _Logger.Debug("failed here [  59] ");return null; }
            if (!int.TryParse(properties[60], out int lightAtk)) { _Logger.Debug("failed here [  60] ");return null; }
            if (!int.TryParse(properties[61], out int darkAtk)) { _Logger.Debug("failed here [  61] ");return null; }

            //Transfer Restrictions
            if (!bool.TryParse(properties[62], out bool sellable)) { _Logger.Debug("failed here [  62] ");return null; }
            if (!bool.TryParse(properties[63], out bool tradeable)) { _Logger.Debug("failed here [  63] ");return null; }
            if (!bool.TryParse(properties[64], out bool newItem)) { _Logger.Debug("failed here [  64] ");return null; }
            if (!bool.TryParse(properties[65], out bool lootable)) { _Logger.Debug("failed here [  65] ");return null; }
            if (!bool.TryParse(properties[66], out bool blessable)) { _Logger.Debug("failed here [  66] ");return null; }
            if (!bool.TryParse(properties[67], out bool curseable)) { _Logger.Debug("failed here [  67] ");return null; }

            //Character Level Restrictions
            if (!int.TryParse(properties[68], out int lowerLimit)) { _Logger.Debug("failed here [  68] ");return null; }
            if (!int.TryParse(properties[69], out int upperLimit)) { _Logger.Debug("failed here [  69] ");return null; }

            //Minimum Stat requirements
            if (!int.TryParse(properties[70], out int requiredStr)) { _Logger.Debug("failed here [  70] ");return null; }
            if (!int.TryParse(properties[71], out int requiredVit)) { _Logger.Debug("failed here [  71] ");return null; }
            if (!int.TryParse(properties[72], out int requiredDex)) { _Logger.Debug("failed here [  72] ");return null; }
            if (!int.TryParse(properties[73], out int requiredAgi)) { _Logger.Debug("failed here [  73] ");return null; }
            if (!int.TryParse(properties[74], out int requiredInt)) { _Logger.Debug("failed here [  74] ");return null; }
            if (!int.TryParse(properties[75], out int requiredPie)) { _Logger.Debug("failed here [  75] ");return null; }
            if (!int.TryParse(properties[76], out int requiredLuk)) { _Logger.Debug("failed here [  76] ");return null; }

            //Soul Level Requirement
            if (!int.TryParse(properties[77], out int requiredSoulLevel)) { _Logger.Debug("failed here [  77] ");return null; }

            //Allignment Requirement
            string requiredAlignment = properties[78];
            //Race Requirement
            if (!int.TryParse(properties[79], out int requiredHuman)) { _Logger.Debug("failed here [  79] ");return null; }
            if (!int.TryParse(properties[80], out int requiredElf)) { _Logger.Debug("failed here [  80] ");return null; }
            if (!int.TryParse(properties[81], out int requiredDwarf)) { _Logger.Debug("failed here [  81] ");return null; }
            if (!int.TryParse(properties[82], out int requiredGnome)) { _Logger.Debug("failed here [  82] ");return null; }
            if (!int.TryParse(properties[83], out int requiredPorkul)) { _Logger.Debug("failed here [  83] ");return null; }

            //Gender Requirement
            string requiredGender = properties[84];

            //Special Description Text
            string whenEquippedText = properties[86];


            return new ItemLibrarySetting
            {
                id = id,
                name = name,
                itemType = itemType,
                equipmentType = equipmentType,
                rarity = rarity,
                physicalAttack = physicalAttack,
                magicalAttack = magicalAttack,
                rangeDistance = rangeDistance,
                specialPerformance = specialPerformance,
                durability = durability,
                hardness = hardness,
                weight = weight,
                slash = slash,
                strike = strike,
                pierce = pierce,
                bonusTolerance = bonusTolerance,
                fig = fig,
                thi = thi,
                mag = mag,
                pri = pri,
                sam = sam,
                nin = nin,
                bis = bis,
                lor = lor,
                clo = clo,
                occupation = occupation,
                hp = hp,
                mp = mp,
                str = str,
                vit = vit,
                dex = dex,
                agi = agi,
                @int = iNt,
                pie = pie,
                luk = luk,
                poison = poison,
                paralysis = paralysis,
                stone = stone,
                faint = faint,
                blind = blind,
                sleep = sleep,
                charm = charm,
                confusion = confusion,
                fear = fear,
                fireDef = fireDef,
                waterDef = waterDef,
                windDef = windDef,
                earthDef = earthDef,
                lightDef = lightDef,
                darkDef = darkDef,
                fireAtk = fireAtk,
                waterAtk = waterAtk,
                windAtk = windAtk,
                earthAtk = earthAtk,
                lightAtk = lightAtk,
                darkAtk = darkAtk,
                sellable = sellable,
                tradeable = tradeable,
                newItem = newItem,
                lootable = lootable,
                blessable = blessable,
                curseable = curseable,
                lowerLimit = lowerLimit,
                upperLimit = upperLimit,
                requiredStr = requiredStr,
                requiredVit = requiredVit,
                requiredDex = requiredDex,
                requiredAgi = requiredAgi,
                requiredInt = requiredInt,
                requiredPie = requiredPie,
                requiredLuk = requiredLuk,
                requiredSoulLevel = requiredSoulLevel,
                requiredAlignment = requiredAlignment,
                requiredHuman = requiredHuman,
                requiredElf = requiredElf,
                requiredDwarf = requiredDwarf,
                requiredGnome = requiredGnome,
                requiredPorkul = requiredPorkul,
                requiredGender = requiredGender,
                whenEquippedText = whenEquippedText
            };
        }
    }
}
