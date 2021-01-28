using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Buffer = System.Buffer;

namespace Necromancy.Server.Data
{
    public class FpmfArchiveIO
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(FpmfArchiveIO));

        private static byte[] MagicBytes = {0x46, 0x50, 0x4D, 0x46};
        private static byte[] MagicBytes_WOITM = {0x57, 0x4F, 0x49, 0x54, 0x4D};

        public FpmfArchive Open(string hedFilePath)
        {
            FileInfo hedFile = new FileInfo(hedFilePath);
            if (!hedFile.Exists)
            {
                throw new FileNotFoundException($"File: {hedFilePath} not found.");
            }

            IBuffer hedBuffer = new StreamBuffer(hedFile.FullName);

            if (hedBuffer.Size < 12)
            {
                throw new Exception("File to small");
            }

            hedBuffer.SetPositionStart();
            byte[] magicBytes = hedBuffer.ReadBytes(4);
            for (int i = 0; i < 4; i++)
            {
                if (magicBytes[i] != MagicBytes[i])
                {
                    throw new Exception("Invalid File");
                }
            }

            FpmfArchive archive = new FpmfArchive();
            archive.Size = hedBuffer.ReadUInt32();
            uint unknown0 = hedBuffer.ReadUInt32();
            hedBuffer = DecryptHed(hedBuffer);
            hedBuffer.SetPositionStart();
            uint unknown1 = hedBuffer.ReadUInt32();
            uint unknown2 = hedBuffer.ReadUInt32();
            byte unknown3 = hedBuffer.ReadByte();
            byte unknown4 = hedBuffer.ReadByte();
            uint unknown5 = hedBuffer.ReadUInt32();
            uint unknown6 = hedBuffer.ReadUInt32();
            int strLen = hedBuffer.ReadByte();
            archive.DatPath = hedBuffer.ReadString(strLen);
            uint unknown7 = hedBuffer.ReadUInt32();
            uint unknown8 = hedBuffer.ReadUInt32();
            uint unknown9 = hedBuffer.ReadUInt32();
            uint unknown10 = hedBuffer.ReadUInt32();
            uint keyLen = hedBuffer.ReadUInt32();
            archive.Key = hedBuffer.ReadBytes((int) keyLen);
            uint unknown11 = hedBuffer.ReadUInt32();
            uint unknown12 = hedBuffer.ReadUInt32();
            uint numFiles = hedBuffer.ReadUInt32();

            string relativeArchiveDir = archive.DatPath
                .Replace("/%08x.dat", "")
                .Replace("./", "")
                .Replace('/', Path.DirectorySeparatorChar);
            string hedPath = hedFile.FullName.Replace(".hed", "");
            string rootPath = hedPath.Replace(relativeArchiveDir, "");
            DirectoryInfo rootDirectory = new DirectoryInfo(rootPath);
            if (!rootDirectory.Exists)
            {
                throw new FileNotFoundException(
                    $"Could not determinate root path. (Rel:{relativeArchiveDir} Hed:{hedPath}  Root:{rootPath}");
            }

            Logger.Info($"Using Root:{rootPath}");
            Dictionary<uint, IBuffer> datBufferPool = new Dictionary<uint, IBuffer>();
            for (int i = 0; i < numFiles; i++)
            {
                FpmfArchiveFile archiveFile = new FpmfArchiveFile();
                strLen = hedBuffer.ReadByte();
                archiveFile.DirectoryPath = hedBuffer.ReadString(strLen);
                strLen = hedBuffer.ReadByte();
                archiveFile.FilePath = hedBuffer.ReadString(strLen);
                archiveFile.DatNumber = hedBuffer.ReadUInt32();
                archiveFile.Offset = hedBuffer.ReadUInt32();
                archiveFile.Size = hedBuffer.ReadUInt32();
                uint unknown13 = hedBuffer.ReadUInt32();
                uint unknown14 = hedBuffer.ReadUInt32();
                Logger.Info($"Processing: {archiveFile.FilePath}");
                IBuffer datBuffer;
                if (datBufferPool.ContainsKey(archiveFile.DatNumber))
                {
                    datBuffer = datBufferPool[archiveFile.DatNumber];
                }
                else
                {
                    string datFileName = archive.DatPath
                        .Replace("%08x", $"{archiveFile.DatNumber:X8}")
                        .Replace("./", "")
                        .Replace('/', Path.DirectorySeparatorChar);
                    string datFilePath = Path.Combine(rootDirectory.FullName, datFileName);
                    FileInfo datFile = new FileInfo(datFilePath);
                    if (!datFile.Exists)
                    {
                        throw new FileNotFoundException($"File: {datFilePath} not found.");
                    }

                    datBuffer = new StreamBuffer(datFile.FullName);
                    datBufferPool.Add(archiveFile.DatNumber, datBuffer);
                }

                IBuffer decrypted = DecryptDat(datBuffer, archiveFile.Offset, archiveFile.Size, archive.Key);
                archiveFile.Data = decrypted.GetAllBytes();
                archive.AddFile(archiveFile);
            }

            return archive;
        }

        public void Save(FpmfArchive archive, string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists)
            {
                throw new FileNotFoundException($"Directory: {directoryPath} not found.");
            }

            string relativeArchiveDir = archive.DatPath
                .Replace("/%08x.dat", "")
                .Replace("./", "")
                .Replace('/', Path.DirectorySeparatorChar);

            string rootPath = Path.Combine(directory.FullName, relativeArchiveDir);

            List<FpmfArchiveFile> files = archive.GetFiles();
            foreach (FpmfArchiveFile file in files)
            {
                string relativeFilePath = file.FilePath
                    .Replace(".\\", "")
                    .Replace('\\', Path.DirectorySeparatorChar);
                string filePath = Path.Combine(rootPath, relativeFilePath);

                FileInfo fileInfo = new FileInfo(filePath);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                }

                File.WriteAllBytes(filePath, file.Data);
            }
        }

        public void Header(string hedFilePath, string outPath)
        {
            if (outPath.LastIndexOf("\\") != outPath.Length - 1)
            {
                outPath += "\\";
            }

            string type = hedFilePath.Substring(hedFilePath.LastIndexOf("\\") + 1,
                (hedFilePath.LastIndexOf(".") - hedFilePath.LastIndexOf("\\")) - 1).Replace("\\data", "");
            FileInfo hedFile = new FileInfo(hedFilePath);
            if (!hedFile.Exists)
            {
                throw new FileNotFoundException($"File: {hedFilePath} not found.");
            }

            IBuffer hedBuffer = new StreamBuffer(hedFile.FullName);

            if (hedBuffer.Size < 12)
            {
                throw new Exception("File to small");
            }

            hedBuffer.SetPositionStart();
            byte[] magicBytes = hedBuffer.ReadBytes(4);
            for (int i = 0; i < 4; i++)
            {
                if (magicBytes[i] != MagicBytes[i])
                {
                    throw new Exception("Invalid File");
                }
            }

            FpmfArchive archive = new FpmfArchive();
            archive.Size = hedBuffer.ReadUInt32();
            uint unknown0 = hedBuffer.ReadUInt32();

            hedBuffer = DecryptHed(hedBuffer);
            BinaryWriter tmpwriter = new BinaryWriter(File.Open(outPath + type + "_header.bin", FileMode.Create));
            tmpwriter.Write(magicBytes);
            tmpwriter.Write(archive.Size);
            tmpwriter.Write(unknown0);
            tmpwriter.Write(hedBuffer.GetAllBytes());
            tmpwriter.Flush();
            tmpwriter.Close();
        }

        public void Pack(string inPath, string outPath, string archiveName, string archivePath = "")
        {
            uint fileTime = 0x506fa78e;
            string dirPath = archivePath;
            if (archivePath.Length > 0)
            {
                if (!dirPath.StartsWith("\\"))
                {
                    dirPath = "\\" + dirPath;
                    archivePath = "\\" + archivePath;
                }

                if (!dirPath.EndsWith("\\"))
                {
                    dirPath = dirPath + "\\";
                    archivePath = archivePath + "\\";
                }

                dirPath = ".\\" + archiveName + dirPath + "%08x.dat";
            }
            else
            {
                dirPath = ".\\" + archiveName + "\\" + "%08x.dat";
            }

            dirPath = dirPath.Replace("\\", "/");
            FpmfArchive archive = new FpmfArchive();
            if (inPath.EndsWith("\\"))
            {
                inPath = inPath.Substring(0, inPath.Length - 1);
            }

            uint currentOffset = 0;
            string baseArchivePath = inPath + "\\" + archiveName + archivePath;
            string[] inFiles = Directory.GetFiles(baseArchivePath, "*", SearchOption.AllDirectories);
            archive.NumFiles = (uint) inFiles.Length;
            archive.DatPath = dirPath;
            archive.DatPathLen = dirPath.Length;

            foreach (string inFile in inFiles)
            {
                IBuffer inReader = new StreamBuffer(inFile);
                FpmfArchiveFile datFile = new FpmfArchiveFile();
                datFile.Size = (uint) inReader.Size;
                datFile.DatNumber = 0;
                datFile.Offset = currentOffset;
                IBuffer encryptedBuff = EncryptDat(inReader, archive.Key);
                datFile.Data = encryptedBuff.GetAllBytes();
                datFile.FilePath = inFile.Replace(inPath + "\\" + archiveName, ".");
                datFile.FilePathSize = (uint) datFile.FilePath.Length;
                datFile.DirectoryPath = ".\\" + archiveName + "\\";
                datFile.DirectoryPathSize = (uint) datFile.DirectoryPath.Length;
                datFile.Unknown0 = fileTime;
                datFile.Unknown1 = 0;
                archive.AddFile(datFile);
                currentOffset += datFile.Size;
            }

            if (archivePath.Length > 0)
            {
                outPath = outPath + "\\" + archiveName + archivePath;
            }
            else
            {
                outPath = outPath + "\\" + archiveName + "\\";
            }

            SavePack(archive, inPath, outPath, archiveName);
        }

        private void SavePack(FpmfArchive archive, string inPath, string outPath, string archiveName)
        {
            Directory.CreateDirectory(outPath);
            IBuffer fileBuff = new StreamBuffer();
            IBuffer headerBuff = new StreamBuffer();
            List<FpmfArchiveFile> archiveFiles = archive.GetFiles();
            foreach (FpmfArchiveFile archiveFile in archiveFiles)
            {
                fileBuff.WriteByte((byte) archiveFile.DirectoryPathSize);
                fileBuff.WriteCString(archiveFile.DirectoryPath);
                fileBuff.Position = fileBuff.Position - 1;
                fileBuff.WriteByte((byte) archiveFile.FilePathSize);
                fileBuff.WriteCString(archiveFile.FilePath);
                fileBuff.Position = fileBuff.Position - 1;
                fileBuff.WriteUInt32(archiveFile.DatNumber);
                fileBuff.WriteUInt32(archiveFile.Offset);
                fileBuff.WriteUInt32(archiveFile.Size);
                fileBuff.WriteUInt32(archiveFile.Unknown0);
                fileBuff.WriteUInt32(archiveFile.Unknown1);
            }

            headerBuff.WriteBytes(MagicBytes);
            headerBuff.WriteInt32(0);
            headerBuff.WriteUInt32(archive.Unknown0);
            headerBuff.WriteUInt32(archive.Unknown1);
            headerBuff.WriteUInt32(archive.Unknown2);
            headerBuff.WriteByte(archive.Unknown3);
            headerBuff.WriteByte(archive.Unknown4);
            headerBuff.WriteUInt32(archive.Unknown5);
            headerBuff.WriteInt32(archive.DatPath.Length + 9);
            headerBuff.WriteByte((byte) archive.DatPath.Length);
            headerBuff.WriteCString(archive.DatPath);
            headerBuff.Position = headerBuff.Position - 1;
            uint type = 0;
            switch (archiveName)
            {
                case "script":
                case "settings":
                case "item":
                case "interface":
                    type = 1;
                    break;
                case "help_end":
                    type = 2;
                    break;
            }

            headerBuff.WriteUInt32(type);
            headerBuff.WriteUInt32(archive.Unknown8);
            headerBuff.WriteUInt32(archive.Unknown9);
            headerBuff.WriteUInt32(archive.Unknown10);
            headerBuff.WriteInt32(archive.Key.Length);
            headerBuff.WriteBytes(archive.Key);
            headerBuff.WriteUInt32(archive.Unknown11);
            headerBuff.WriteInt32(fileBuff.Size + 4);
            headerBuff.WriteUInt32(archive.NumFiles);
            headerBuff.WriteBytes(fileBuff.GetAllBytes());

            headerBuff = EncryptHed(headerBuff);

            string hedPath = outPath.Substring(0, outPath.LastIndexOf("\\")) + ".hed";
            BinaryWriter headerWriter = new BinaryWriter(File.Open(hedPath, FileMode.Create));
            headerBuff.Position = 4;
            headerBuff.WriteInt32(headerBuff.Size - 12);
            headerWriter.Write(headerBuff.GetAllBytes(), 0, headerBuff.Size);
            headerWriter.Flush();
            headerWriter.Close();

            BinaryWriter datWriter = new BinaryWriter(File.Open(outPath + "\\" + "00000000.dat", FileMode.Create));
            IBuffer outBuff = new StreamBuffer();
            foreach (FpmfArchiveFile archiveFile in archiveFiles)
            {
                string inputFile = inPath + "\\" + archiveName + archiveFile.FilePath.Substring(1);
                IBuffer datFileReader = new StreamBuffer(inputFile);
                datFileReader = EncryptDat(datFileReader, archive.Key);
                outBuff.WriteBytes(datFileReader.GetAllBytes());
            }

            datWriter.Write(outBuff.GetAllBytes(), 0, outBuff.Size);
            datWriter.Flush();
            datWriter.Close();
        }
        
        public byte[] EncryptWoItm(byte[] decryptedCsv, byte[] key)
        {
            //  string keys = "AABBCCDDEEFFGGHH";
            //  string csv =
            //      "100101,HELMET,NORMAL,,0,,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,0,0,0,,,0,0,0,0,0,0,0,0,1,,,40,40,20,,0,0,0,0,0,0,,,,0,0,0,0,0,0,,,,,,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,,,0,,NONE,HEAD,,,,0,0,0,0,0,0,1,0,,100101,,,,,,,,,,,,,,,,,,,,,,,,,0,0,5,1,0";
            //  string csv1 =
            //      "100101, HELMET, NORMAL, , 0, , 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, , , 0, 0, 0, 0, 0, 0, 0, 0, 1, , , 40, 40, 20, , 0, 0, 0, 0, 0, 0, , , , 0, 0, 0, 0, 0, 0, , , , , , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, , , 0, , NONE, HEAD, , , , 0, 0, 0, 0, 0, 0, 1, 0, , 100101, , , , , , , , , , , , , , , , , , , , , , , , , 0, 0, 5, 1, 0";

            //      byte[] key = Encoding.UTF8.GetBytes(keys);
            // Console.WriteLine(Util.HexDump(output));
            // Console.WriteLine(Util.ToHexString(output));
            // Console.WriteLine($"ActualSize:{output.Length} A:{output.Length + 4:X8} B:{inlen:X8}");
//
            // // 416 = actualt data suze
            // // 420
            // // 412
            // bool done = true;

            int blockSize = 16;

            // inflate key
            byte[][] subKey = new byte[34][];
            uint keyLength = (uint) key.Length * 8;
            Util.Camellia.KeySchedule(keyLength, key, subKey);

            // prepare buffer
            int padding = blockSize - (decryptedCsv.Length % blockSize);
            int length = decryptedCsv.Length + padding;
            byte[] input = new byte[length];
            // TODO could avoid copy by applying padding in last loop
            Buffer.BlockCopy(decryptedCsv, 0, input, 0, decryptedCsv.Length);
            byte[] output = new byte[length];

            // encrypt
            Span<byte> inPtr = input;
            Span<byte> outPtr = output;
            int current = 0;
            while (current < length)
            {
                Util.Camellia.CryptBlock(
                    false,
                    keyLength,
                    inPtr.Slice(current, blockSize),
                    subKey,
                    outPtr.Slice(current, blockSize)
                );
                current += blockSize;
            }

            return output;
        }

        public byte[] DecryptWoItm(byte[] encryptedCsv, byte[] key)
        {
            int blockSize = 16;

            // inflate key
            byte[][] subKey = new byte[34][];
            uint keyLength = (uint) key.Length * 8;
            Util.Camellia.KeySchedule(keyLength, key, subKey);

            // prepare buffer
            int length = encryptedCsv.Length;
            byte[] output = new byte[length];

            // decrypt
            Span<byte> inPtr = encryptedCsv;
            Span<byte> outPtr = output;
            int current = 0;
            while (current < length)
            {
                Util.Camellia.CryptBlock(
                    true,
                    keyLength,
                    inPtr.Slice(current, blockSize),
                    subKey,
                    outPtr.Slice(current, blockSize)
                );
                current += blockSize;
            }

            return output;
        }

        /// <summary>
        /// 0x403700
        /// </summary>
        public Dictionary<uint, string> OpenWoItm(string itemPath)
        {
            FileInfo itemFile = new FileInfo(itemPath);
            if (!itemFile.Exists)
            {
                throw new FileNotFoundException($"File: {itemPath} not found.");
            }

            IBuffer buffer = new StreamBuffer(itemFile.FullName);
            buffer.SetPositionStart();
            byte[] magicBytes = buffer.ReadBytes(5);
            for (int i = 0; i < 5; i++)
            {
                if (magicBytes[i] != MagicBytes_WOITM[i])
                {
                    throw new Exception("Invalid WOITM File");
                }
            }

            short version = buffer.ReadInt16(); // cmp to 1

            Dictionary<uint, string> items = new Dictionary<uint, string>();
            while (buffer.Position < buffer.Size)
            {
                uint itemId = buffer.ReadUInt32();
                int chunkSize = buffer.ReadInt32();
                int chunkLen = buffer.ReadInt32();
                byte[] encryptedCsv = buffer.ReadBytes(chunkSize - 4);
                byte[] decryptedCsv = DecryptWoItm(encryptedCsv, new byte[0x10]);
                string csv = Encoding.UTF8.GetString(decryptedCsv);
                items.Add(itemId, csv);
            }

            return items;
        }

        public void SaveWoItm(Dictionary<uint, string> items, string itemPath)
        {
            FileInfo itemFile = new FileInfo(itemPath);
            if (!itemFile.Exists)
            {
                // TODO create
            }

            IBuffer buffer = new StreamBuffer();
            buffer.WriteBytes(MagicBytes_WOITM);
            buffer.WriteInt16(1); // version 1

            foreach (uint itemId in items.Keys)
            {
                string csv = items[itemId];
                byte[] decryptedCsv = Encoding.UTF8.GetBytes(csv);
                byte[] encryptedCsv = EncryptWoItm(decryptedCsv, new byte[0x10]);

                buffer.WriteUInt32(itemId);
                buffer.WriteInt32(0);
                buffer.WriteBytes(encryptedCsv);
            }
        }

        /// <summary>
        /// 0xA7E480
        /// </summary>
        private IBuffer DecryptHed(IBuffer buffer)
        {
            byte bl = 0;
            byte al = 0;

            //Uncomment for US Steam Client
            byte dl = 0xA6;
            byte sub = 0x21;

            //Uncomment for US Sunset Client.
            //byte dl = 0xEA;
            //byte sub = 0x0A;

            //Uncomment for Beta Client
            //byte dl = 0x7D;
            //byte sub = 0xC4;

            // Uncomment for JP client
            dl = 0x67;
            sub = 0xC7;

            buffer.Position = 12;
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < buffer.Size)
            {
                byte cl = buffer.ReadByte();
                bl = al;
                bl = (byte) (bl + dl);
                bl = (byte) (bl ^ cl);
                bl = (byte) (bl - sub);
                al = (byte) (al + 1);
                dl = cl;
                outBuffer.WriteByte(bl);
            }

            return outBuffer;
        }

        /// <summary>
        /// 0xA7E3F0
        /// </summary>
        private IBuffer DecryptDat(IBuffer buffer, uint fileOffset, uint fileLength, byte[] key)
        {
            if (key == null)
            {
                throw new Exception("Invalid Key");
            }

            if (key.Length <= 0)
            {
                return buffer;
            }

            uint endPosition = fileOffset + fileLength;
            if (endPosition > buffer.Size)
            {
                throw new Exception("Buffer to small");
            }

            int rotKeyIndex = 0;
            buffer.Position = (int) fileOffset;
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < endPosition)
            {
                byte al = buffer.ReadByte();
                al = (byte) (al - key[rotKeyIndex]);
                outBuffer.WriteByte(al);
                rotKeyIndex++;
                if (rotKeyIndex >= key.Length)
                {
                    rotKeyIndex = 0;
                }
            }

            return outBuffer;
        }

        private IBuffer EncryptHed(IBuffer inBuff)
        {
            byte bl = 0;
            byte al = 0;

            //Uncomment for US Steam Client
            //byte dl = 0xA6;
            byte add = 0x21;

            //Uncomment for US Sunset Client.
            //byte dl = 0xEA;
            //byte sub = 0x0A;

            //Uncomment for Beta Client
            //byte dl = 0x7D;
            //byte sub = 0xC4;

            // Uncomment for JP client
            byte dl = 0x67;
            byte sub = 0xC7;

            //Uncomment for beta client
            //dl = 0x7D;
            //sub = 0xC4;

            inBuff.SetPositionStart();
            IBuffer outBuffer = new StreamBuffer();
            outBuffer.WriteBytes(inBuff.ReadBytes(4));
            outBuffer.WriteInt32(inBuff.Size - 12);
            inBuff.Position = 8;
            outBuffer.WriteInt32(inBuff.ReadInt32());

            while (inBuff.Position < inBuff.Size)
            {
                byte cl = inBuff.ReadByte();
                cl = (byte) (cl + add);
                bl = (byte) (dl - al);
                bl = (byte) (bl ^ cl);
                al = (byte) (al - 1);
                dl = bl;
                outBuffer.WriteByte(bl);
            }

            return outBuffer;
        }

        private IBuffer EncryptDat(IBuffer buffer, byte[] Key)
        {
            int rotKeyIndex = 0;
            buffer.SetPositionStart();
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < buffer.Size)
            {
                byte al = buffer.ReadByte();
                al = (byte) (al + Key[rotKeyIndex]);
                outBuffer.WriteByte(al);
                rotKeyIndex++;
                if (rotKeyIndex >= Key.Length)
                {
                    rotKeyIndex = 0;
                }
            }

            return outBuffer;
        }
    }
}
