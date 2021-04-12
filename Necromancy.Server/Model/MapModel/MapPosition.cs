namespace Necromancy.Server.Model
{
    public class MapPosition
    {
        public MapPosition(float xpos = 0, float ypos = 0, float zpos = 0, byte heading = 0)
        {
            x = xpos;
            y = ypos;
            z = zpos;
            this.heading = heading;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public byte heading { get; set; }
    }
}
