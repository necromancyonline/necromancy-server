namespace Necromancy.Server.Data
{
    public class FpmfArchiveFile
    {
        public uint size { get; set; }

        public uint offset { get; set; }

        public string filePath { get; set; }

        public uint filePathSize { get; set; }

        public string directoryPath { get; set; }

        public uint directoryPathSize { get; set; }

        public uint datNumber { get; set; }

        public byte[] data { get; set; }

        public uint unknown0 { get; set; }

        public uint unknown1 { get; set; }
    }
}
