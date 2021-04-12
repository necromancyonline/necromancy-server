namespace Necromancy.Server.Data.Setting
{
    public class ModelAtrCsvReader : CsvReader<ModelAtrSetting>
    {
        protected override int numExpectedItems => 7;

        protected override ModelAtrSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) return null;

            if (!float.TryParse(properties[1], out float normal)) return null;

            if (!float.TryParse(properties[2], out float crouching)) return null;

            if (!float.TryParse(properties[3], out float sitting)) return null;

            if (!float.TryParse(properties[4], out float rolling)) return null;

            if (!float.TryParse(properties[5], out float death)) return null;

            if (!float.TryParse(properties[6], out float motion)) return null;

            return new ModelAtrSetting
            {
                id = id,
                normalMagnification = normal,
                crouchingMagnification = crouching,
                sittingMagnification = sitting,
                rollingMagnification = rolling,
                deathMagnification = death,
                motionMagnification = motion
            };
        }
    }
}
