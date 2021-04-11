namespace Necromancy.Server.Data
{
    public class FpmfArchiveFile
    {
        private uint _size;
        private uint _offset;
        private uint _datNumber;
        private string _filePath;
        private uint _filePathSize;
        private string _directoryPath;
        private uint _directoryPathSize;
        private byte[] _data;
        private uint _fileTime;
        private uint _unknown1;

        public uint size
        {
            get => _size;
            set => _size = value;
        }

        public uint offset
        {
            get => _offset;
            set => _offset = value;
        }

        public string filePath
        {
            get => _filePath;
            set => _filePath = value;
        }

        public uint filePathSize
        {
            get => _filePathSize;
            set => _filePathSize = value;
        }
        public string directoryPath
        {
            get => _directoryPath;
            set => _directoryPath = value;
        }

        public uint directoryPathSize
        {
            get => _directoryPathSize;
            set => _directoryPathSize = value;
        }
        public uint datNumber
        {
            get => _datNumber;
            set => _datNumber = value;
        }

        public byte[] data
        {
            get => _data;
            set => _data = value;
        }
        public uint unknown0
        {
            get => _fileTime;
            set => _fileTime = value;
        }
        public uint unknown1
        {
            get => _unknown1;
            set => _unknown1 = value;
        }

        public FpmfArchiveFile()
        {
        }
    }
}
