using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Buffer = System.Buffer;

namespace Necromancy.Server.Data
{
    public class FpmfArchiveIo
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(FpmfArchiveIo));

        private static readonly byte[] _MagicBytes = { 0x46, 0x50, 0x4D, 0x46 };
        private static readonly byte[] _MagicBytesWoitm = { 0x57, 0x4F, 0x49, 0x54, 0x4D };

        public FpmfArchive Open(string hedFilePath)
        {
            FileInfo hedFile = new FileInfo(hedFilePath);
            if (!hedFile.Exists) throw new FileNotFoundException($"File: {hedFilePath} not found.");

            IBuffer hedBuffer = new StreamBuffer(hedFile.FullName);

            if (hedBuffer.Size < 12) throw new Exception("File to small");

            hedBuffer.SetPositionStart();
            byte[] magicBytes = hedBuffer.ReadBytes(4);
            for (int i = 0; i < 4; i++)
                if (magicBytes[i] != _MagicBytes[i])
                    throw new Exception("Invalid File");

            FpmfArchive archive = new FpmfArchive();
            archive.size = hedBuffer.ReadUInt32();
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
            archive.datPath = hedBuffer.ReadString(strLen);
            uint unknown7 = hedBuffer.ReadUInt32();
            uint unknown8 = hedBuffer.ReadUInt32();
            uint unknown9 = hedBuffer.ReadUInt32();
            uint unknown10 = hedBuffer.ReadUInt32();
            uint keyLen = hedBuffer.ReadUInt32();
            archive.key = hedBuffer.ReadBytes((int)keyLen);
            uint unknown11 = hedBuffer.ReadUInt32();
            uint unknown12 = hedBuffer.ReadUInt32();
            uint numFiles = hedBuffer.ReadUInt32();

            string relativeArchiveDir = archive.datPath
                .Replace("/%08x.dat", "")
                .Replace("./", "")
                .Replace('/', Path.DirectorySeparatorChar);
            string hedPath = hedFile.FullName.Replace(".hed", "");
            string rootPath = hedPath.Replace(relativeArchiveDir, "");
            DirectoryInfo rootDirectory = new DirectoryInfo(rootPath);
            if (!rootDirectory.Exists)
                throw new FileNotFoundException(
                    $"Could not determinate root path. (Rel:{relativeArchiveDir} Hed:{hedPath}  Root:{rootPath}");

            _Logger.Info($"Using Root:{rootPath}");
            Dictionary<uint, IBuffer> datBufferPool = new Dictionary<uint, IBuffer>();
            for (int i = 0; i < numFiles; i++)
            {
                FpmfArchiveFile archiveFile = new FpmfArchiveFile();
                strLen = hedBuffer.ReadByte();
                archiveFile.directoryPath = hedBuffer.ReadString(strLen);
                strLen = hedBuffer.ReadByte();
                archiveFile.filePath = hedBuffer.ReadString(strLen);
                archiveFile.datNumber = hedBuffer.ReadUInt32();
                archiveFile.offset = hedBuffer.ReadUInt32();
                archiveFile.size = hedBuffer.ReadUInt32();
                uint unknown13 = hedBuffer.ReadUInt32();
                uint unknown14 = hedBuffer.ReadUInt32();
                _Logger.Info($"Processing: {archiveFile.filePath}");
                IBuffer datBuffer;
                if (datBufferPool.ContainsKey(archiveFile.datNumber))
                {
                    datBuffer = datBufferPool[archiveFile.datNumber];
                }
                else
                {
                    string datFileName = archive.datPath
                        .Replace("%08x", $"{archiveFile.datNumber:X8}")
                        .Replace("./", "")
                        .Replace('/', Path.DirectorySeparatorChar);
                    string datFilePath = Path.Combine(rootDirectory.FullName, datFileName);
                    FileInfo datFile = new FileInfo(datFilePath);
                    if (!datFile.Exists) throw new FileNotFoundException($"File: {datFilePath} not found.");

                    datBuffer = new StreamBuffer(datFile.FullName);
                    datBufferPool.Add(archiveFile.datNumber, datBuffer);
                }

                IBuffer decrypted = DecryptDat(datBuffer, archiveFile.offset, archiveFile.size, archive.key);
                archiveFile.data = decrypted.GetAllBytes();
                archive.AddFile(archiveFile);
            }

            return archive;
        }

        public void Save(FpmfArchive archive, string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists) throw new FileNotFoundException($"Directory: {directoryPath} not found.");

            string relativeArchiveDir = archive.datPath
                .Replace("/%08x.dat", "")
                .Replace("./", "")
                .Replace('/', Path.DirectorySeparatorChar);

            string rootPath = Path.Combine(directory.FullName, relativeArchiveDir);

            List<FpmfArchiveFile> files = archive.GetFiles();
            foreach (FpmfArchiveFile file in files)
            {
                string relativeFilePath = file.filePath
                    .Replace(".\\", "")
                    .Replace('\\', Path.DirectorySeparatorChar);
                string filePath = Path.Combine(rootPath, relativeFilePath);

                FileInfo fileInfo = new FileInfo(filePath);
                if (!Directory.Exists(fileInfo.DirectoryName)) Directory.CreateDirectory(fileInfo.DirectoryName);

                File.WriteAllBytes(filePath, file.data);
            }
        }

        public void Header(string hedFilePath, string outPath)
        {
            if (outPath.LastIndexOf("\\") != outPath.Length - 1) outPath += "\\";

            string type = hedFilePath.Substring(hedFilePath.LastIndexOf("\\") + 1,
                hedFilePath.LastIndexOf(".") - hedFilePath.LastIndexOf("\\") - 1).Replace("\\data", "");
            FileInfo hedFile = new FileInfo(hedFilePath);
            if (!hedFile.Exists) throw new FileNotFoundException($"File: {hedFilePath} not found.");

            IBuffer hedBuffer = new StreamBuffer(hedFile.FullName);

            if (hedBuffer.Size < 12) throw new Exception("File to small");

            hedBuffer.SetPositionStart();
            byte[] magicBytes = hedBuffer.ReadBytes(4);
            for (int i = 0; i < 4; i++)
                if (magicBytes[i] != _MagicBytes[i])
                    throw new Exception("Invalid File");

            FpmfArchive archive = new FpmfArchive();
            archive.size = hedBuffer.ReadUInt32();
            uint unknown0 = hedBuffer.ReadUInt32();

            hedBuffer = DecryptHed(hedBuffer);
            BinaryWriter tmpwriter = new BinaryWriter(File.Open(outPath + type + "_header.bin", FileMode.Create));
            tmpwriter.Write(magicBytes);
            tmpwriter.Write(archive.size);
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
            if (inPath.EndsWith("\\")) inPath = inPath.Substring(0, inPath.Length - 1);

            uint currentOffset = 0;
            string baseArchivePath = inPath + "\\" + archiveName + archivePath;
            string[] inFiles = Directory.GetFiles(baseArchivePath, "*", SearchOption.AllDirectories);
            archive.numFiles = (uint)inFiles.Length;
            archive.datPath = dirPath;
            archive.datPathLen = dirPath.Length;

            foreach (string inFile in inFiles)
            {
                IBuffer inReader = new StreamBuffer(inFile);
                FpmfArchiveFile datFile = new FpmfArchiveFile();
                datFile.size = (uint)inReader.Size;
                datFile.datNumber = 0;
                datFile.offset = currentOffset;
                IBuffer encryptedBuff = EncryptDat(inReader, archive.key);
                datFile.data = encryptedBuff.GetAllBytes();
                datFile.filePath = inFile.Replace(inPath + "\\" + archiveName, ".");
                datFile.filePathSize = (uint)datFile.filePath.Length;
                datFile.directoryPath = ".\\" + archiveName + "\\";
                datFile.directoryPathSize = (uint)datFile.directoryPath.Length;
                datFile.unknown0 = fileTime;
                datFile.unknown1 = 0;
                archive.AddFile(datFile);
                currentOffset += datFile.size;
            }

            if (archivePath.Length > 0)
                outPath = outPath + "\\" + archiveName + archivePath;
            else
                outPath = outPath + "\\" + archiveName + "\\";

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
                fileBuff.WriteByte((byte)archiveFile.directoryPathSize);
                fileBuff.WriteCString(archiveFile.directoryPath);
                fileBuff.Position = fileBuff.Position - 1;
                fileBuff.WriteByte((byte)archiveFile.filePathSize);
                fileBuff.WriteCString(archiveFile.filePath);
                fileBuff.Position = fileBuff.Position - 1;
                fileBuff.WriteUInt32(archiveFile.datNumber);
                fileBuff.WriteUInt32(archiveFile.offset);
                fileBuff.WriteUInt32(archiveFile.size);
                fileBuff.WriteUInt32(archiveFile.unknown0);
                fileBuff.WriteUInt32(archiveFile.unknown1);
            }

            headerBuff.WriteBytes(_MagicBytes);
            headerBuff.WriteInt32(0);
            headerBuff.WriteUInt32(archive.unknown0);
            headerBuff.WriteUInt32(archive.unknown1);
            headerBuff.WriteUInt32(archive.unknown2);
            headerBuff.WriteByte(archive.unknown3);
            headerBuff.WriteByte(archive.unknown4);
            headerBuff.WriteUInt32(archive.unknown5);
            headerBuff.WriteInt32(archive.datPath.Length + 9);
            headerBuff.WriteByte((byte)archive.datPath.Length);
            headerBuff.WriteCString(archive.datPath);
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
            headerBuff.WriteUInt32(archive.unknown8);
            headerBuff.WriteUInt32(archive.unknown9);
            headerBuff.WriteUInt32(archive.unknown10);
            headerBuff.WriteInt32(archive.key.Length);
            headerBuff.WriteBytes(archive.key);
            headerBuff.WriteUInt32(archive.unknown11);
            headerBuff.WriteInt32(fileBuff.Size + 4);
            headerBuff.WriteUInt32(archive.numFiles);
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
                string inputFile = inPath + "\\" + archiveName + archiveFile.filePath.Substring(1);
                IBuffer datFileReader = new StreamBuffer(inputFile);
                datFileReader = EncryptDat(datFileReader, archive.key);
                outBuff.WriteBytes(datFileReader.GetAllBytes());
            }

            datWriter.Write(outBuff.GetAllBytes(), 0, outBuff.Size);
            datWriter.Flush();
            datWriter.Close();
        }

        public void EncryptWoItm()
        {
            string keys = "AABBCCDDEEFFGGHH";
            string csv =
                "100101,HELMET,NORMAL,,0,,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,0,0,0,,,0,0,0,0,0,0,0,0,1,,,40,40,20,,0,0,0,0,0,0,,,,0,0,0,0,0,0,,,,,,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,,,0,,NONE,HEAD,,,,0,0,0,0,0,0,1,0,,100101,,,,,,,,,,,,,,,,,,,,,,,,,0,0,5,1,0";
            string csv1 =
                "100101, HELMET, NORMAL, , 0, , 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, , , 0, 0, 0, 0, 0, 0, 0, 0, 1, , , 40, 40, 20, , 0, 0, 0, 0, 0, 0, , , , 0, 0, 0, 0, 0, 0, , , , , , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, , , 0, , NONE, HEAD, , , , 0, 0, 0, 0, 0, 0, 1, 0, , 100101, , , , , , , , , , , , , , , , , , , , , , , , , 0, 0, 5, 1, 0";
            Camellia c = new Camellia();
            byte[] key = Encoding.UTF8.GetBytes(keys);
            uint keyLength = (uint)key.Length * 8;
            byte[][] subkey = new byte[34][];
            byte[] oinput = Encoding.UTF8.GetBytes(csv1);
            int inlen = oinput.Length;
            int padding = 16 - inlen % 16;
            int totalLength = inlen + padding;

            byte[] input = new byte[totalLength];
            int length = input.Length;
            Buffer.BlockCopy(oinput, 0, input, 0, inlen);
            byte[] output = new byte[length];

            Span<byte> inPtr = input;
            Span<byte> outPtr = output;

            c.KeySchedule(keyLength, key, subkey);

            int current = 0;
            while (current < length)
            {
                int xorLen = current + 16 < length ? 16 : length - current;
                // for (int i = 0; i < xorLen; i++)
                //  {
                //    input[current + i] = (byte) (input[current + i] ^ prv[i]);
                //  }

                c.CryptBlock(
                    false,
                    keyLength,
                    inPtr.Slice(current, 16),
                    subkey,
                    outPtr.Slice(current, 16)
                );
                // for (int i = 0; i < xorLen; i++)
                //  {
                //    prv[i] = output[current + i];
                // }
                current += xorLen;
                // Console.WriteLine($"{current} {xorLen} {length}");
            }

            Console.WriteLine(HexDump(output));
            Console.WriteLine(Util.ToHexString(output));
            Console.WriteLine($"ActualSize:{output.Length} A:{output.Length + 4:X8} B:{inlen:X8}");

            // 416 = actualt data suze
            // 420
            // 412
            bool done = true;
        }


        /// <summary>
        ///     0x403700
        /// </summary>
        public void OpenWoItm(string itemPath)
        {
            FileInfo itemFile = new FileInfo(itemPath);
            if (!itemFile.Exists) throw new FileNotFoundException($"File: {itemPath} not found.");

            IBuffer buffer = new StreamBuffer(itemFile.FullName);
            buffer.SetPositionStart();
            byte[] magicBytes = buffer.ReadBytes(5);
            for (int i = 0; i < 5; i++)
                if (magicBytes[i] != _MagicBytesWoitm[i])
                    throw new Exception("Invalid WOITM File");

            short version = buffer.ReadInt16(); // cmp to 1
            List<WoItm> woItems = new List<WoItm>();
            while (buffer.Position < buffer.Size)
            {
                int itemId = buffer.ReadInt32();
                int chunkSize = buffer.ReadInt32();
                int chunkLen = buffer.ReadInt32();
                byte[] data = buffer.ReadBytes(chunkSize - 4);

                WoItm woItm = new WoItm();
                woItm.id = itemId;
                woItm.size = chunkSize;
                woItm.size2 = chunkLen;
                woItm.data = data;
                woItems.Add(woItm);
            }

            foreach (WoItm woItem in woItems)
            {
                IBuffer itemBuffer = new StreamBuffer(woItem.data);
                itemBuffer.SetPositionStart();

                IBuffer outBuffer = new StreamBuffer();

                uint[] xor =
                {
                    0xA522C3ED,
                    0x482E64B9,
                    0x0E52712B,
                    0x3ABC1D26
                };

                for (int i = 0; i < 4; i++)
                {
                    uint a = itemBuffer.ReadUInt32();
                    uint b = RotateRight(a, 8); // 00403035 | C1CE 08 | ror esi,8
                    uint c = b & 0xFF00FF00;
                    uint d = RotateLeft(a, 8); // 0040303E | C1C0 08 | rol eax,8
                    uint e = d & 0xFF00FF;
                    uint f = c | e;
                    outBuffer.WriteUInt32(f);
                }

                _Logger.Debug(outBuffer.ToHexString(" "));


                _Logger.Info("done");


                /*              These 4 words are from the previous function after xor of xor[] above
                                uint word1 = 0x6B9306F7;
                                uint word2 = 0xFE7D4F35;
                                uint word3 = 0x406D7743;
                                uint word4 = 0x9C07F4C0;

                                uint seed = 0;
                                uint seed1 = 0xFFFFFFFE;
                                word1 = word1 ^ seed;
                                uint a = ((word1 >> 16) & 0xFF) * 4;
                                uint b = ((word1 >> 8) & 0xFF) * 4;
                                uint c = (word1 >> 24) * 4;
                                uint e = table1[c];
                                uint f = table3[a];
                                uint g = e ^ f;
                                uint i = table3[b];
                                uint j = g ^ i;

                                uint k = (word2 & 0xFF) * 4;
                                uint l = j ^ table2[k];
                                uint m = (((word2 ^ seed) >> 16) & 0xFF) * 4;
                                uint n = ((word2 >> 24) * 4;
                                uint o = table3[n];
                                uint p = table4[m];
                                uint q = o ^ p;
                                uint r = ((word2 >> 8) & 0xFF) * 4;
                                uint s = (q ^ table2[r]);
                                uint t = (word2 & 0xFF) * 4;
                                uint u = table1[t] ^ s;
                                uint v = u ^ l;

                                word3 = word3 ^ v;
                                uint w = ((l & 0xFF) << 18) | ((l >> 8) & 0x00FFFFFF);
                                uint x = w ^ word4;
                                uint y = u ^ x;
                                word4 = l ^ y;
                                uint z = (seed1 * 4) + ???? (1628EEC8);   // Missed a push earlier
                                uint aa = word3 ^ z;
                                uint ab = ((aa >> 16) & 0xFF) * 4;
                                uint ac = (aa >> 24) * 4;
                                uint ad = table1[ac];
                                uint ae = (ad ^ table3[ab]);
                                */
            }

            _Logger.Info("done");
        }

        private uint RotateLeft(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }

        private uint RotateRight(uint x, int n)
        {
            return (x >> n) | (x << (32 - n));
        }

        /// <summary>
        ///     0xA7E480
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
                bl = (byte)(bl + dl);
                bl = (byte)(bl ^ cl);
                bl = (byte)(bl - sub);
                al = (byte)(al + 1);
                dl = cl;
                outBuffer.WriteByte(bl);
            }

            return outBuffer;
        }

        /// <summary>
        ///     0xA7E3F0
        /// </summary>
        private IBuffer DecryptDat(IBuffer buffer, uint fileOffset, uint fileLength, byte[] key)
        {
            if (key == null) throw new Exception("Invalid Key");

            if (key.Length <= 0) return buffer;

            uint endPosition = fileOffset + fileLength;
            if (endPosition > buffer.Size) throw new Exception("Buffer to small");

            int rotKeyIndex = 0;
            buffer.Position = (int)fileOffset;
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < endPosition)
            {
                byte al = buffer.ReadByte();
                al = (byte)(al - key[rotKeyIndex]);
                outBuffer.WriteByte(al);
                rotKeyIndex++;
                if (rotKeyIndex >= key.Length) rotKeyIndex = 0;
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
                cl = (byte)(cl + add);
                bl = (byte)(dl - al);
                bl = (byte)(bl ^ cl);
                al = (byte)(al - 1);
                dl = bl;
                outBuffer.WriteByte(bl);
            }

            return outBuffer;
        }

        private IBuffer EncryptDat(IBuffer buffer, byte[] key)
        {
            int rotKeyIndex = 0;
            buffer.SetPositionStart();
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < buffer.Size)
            {
                byte al = buffer.ReadByte();
                al = (byte)(al + key[rotKeyIndex]);
                outBuffer.WriteByte(al);
                rotKeyIndex++;
                if (rotKeyIndex >= key.Length) rotKeyIndex = 0;
            }

            return outBuffer;
        }


        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] hexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                8 // 8 characters for the address
                + 3; // 3 spaces

            int firstCharColumn = firstHexColumn
                                  + bytesPerLine * 3 // - 2 digit for the hexadecimal value and 1 space
                                  + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                                  + 2; // 2 spaces

            int lineLength = firstCharColumn
                             + bytesPerLine // - characters to show the ascii value
                             + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new string(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine)
                .ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = hexChars[(i >> 28) & 0xF];
                line[1] = hexChars[(i >> 24) & 0xF];
                line[2] = hexChars[(i >> 20) & 0xF];
                line[3] = hexChars[(i >> 16) & 0xF];
                line[4] = hexChars[(i >> 12) & 0xF];
                line[5] = hexChars[(i >> 8) & 0xF];
                line[6] = hexChars[(i >> 4) & 0xF];
                line[7] = hexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = hexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = hexChars[b & 0xF];
                        line[charColumn] = b < 32 ? '·' : (char)b;
                    }

                    hexColumn += 3;
                    charColumn++;
                }

                result.Append(line);
            }

            return result.ToString();
        }
    }
}
