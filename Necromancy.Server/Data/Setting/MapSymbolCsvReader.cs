namespace Necromancy.Server.Data.Setting
{
    public class MapSymbolCsvReader : CsvReader<MapSymbolSetting>
    {
        protected override int numExpectedItems => 10;


        protected override MapSymbolSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) return null;

            if (!int.TryParse(properties[1], out int map)) return null;

            if (!int.TryParse(properties[2], out int displayConditionflag)) return null;

            if (!int.TryParse(properties[3], out int splitMapNumber)) return null;

            if (!int.TryParse(properties[4], out int dettingTypeFlag)) return null;

            if (!int.TryParse(properties[5], out int settingIdOrText))
                //Catch the strings, and convert them to a unique int for assigment of behavoir later.
                switch (properties[5])
                {
                    case "Recovery Spring":
                        settingIdOrText = 99009901;
                        break;
                    case "Quopaty Temple":
                        settingIdOrText = 99009902;
                        break;
                    case "Adventurer's Inn":
                        settingIdOrText = 99009903;
                        break;
                    case "Gilgamesh's Tavern":
                        settingIdOrText = 99009904;
                        break;
                    case "Adventurer's Guild":
                        settingIdOrText = 99009905;
                        break;
                    case "Change Channels":
                        settingIdOrText = 99009906;
                        break;
                    case "Departure Gate":
                        settingIdOrText = 99009907;
                        break;
                    case "Modamus Arms":
                        settingIdOrText = 99009908;
                        break;
                    case "Vortak's ItemInfo":
                        settingIdOrText = 99009909;
                        break;
                    case "Sagil Armor":
                        settingIdOrText = 99009910;
                        break;
                    case "Magic by Junon":
                        settingIdOrText = 99009911;
                        break;
                    case "Gidol's Forge & Alchemy":
                        settingIdOrText = 99009912;
                        break;

                    default:
                        return null;
                }

            if (!int.TryParse(properties[6], out int iconType)) return null;

            if (!int.TryParse(properties[7], out int displayPositionX)) return null;

            if (!int.TryParse(properties[8], out int displayPositionY)) return null;

            if (!int.TryParse(properties[9], out int displayPositionZ)) return null;

            return new MapSymbolSetting
            {
                id = id,
                map = map,
                displayConditionflag = displayConditionflag,
                splitMapNumber = splitMapNumber,
                settingTypeFlag = dettingTypeFlag,
                settingIdOrText = settingIdOrText,
                iconType = iconType,
                displayPositionX = displayPositionX,
                displayPositionY = displayPositionY,
                displayPositionZ = displayPositionZ
            };
        }
    }
}
