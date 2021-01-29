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
    public class WoItemIo
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(WoItemIo));
        private static readonly byte[] WoitmMagicBytes = {0x57, 0x4F, 0x49, 0x54, 0x4D};

        public static readonly WoItemIo Instance = new WoItemIo();

        /// <summary>
        /// 0x403700
        /// </summary>
        public Dictionary<uint, string> OpenWoItm(string itemPath, FpmfArchiveIo.WoItmKeyResolver keyResolver)
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
                if (magicBytes[i] != WoitmMagicBytes[i])
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

                byte[] key = keyResolver(itemId);
                if (key == null || key.Length < 16)
                {
                    Logger.Error($"OpenWoItm::ItemId: {itemId} skipped, key is null or to short");
                    continue;
                }

                byte[] decryptedCsv = DecryptWoItm(encryptedCsv, key);
                string csv = Encoding.UTF8.GetString(decryptedCsv);
                items.Add(itemId, csv);
            }

            return items;
        }

        public void SaveWoItm(string itemPath, Dictionary<uint, string> items,
            FpmfArchiveIo.WoItmKeyResolver keyResolver)
        {
            FileInfo itemFile = new FileInfo(itemPath);
            if (!itemFile.Exists)
            {
                // TODO create
            }

            IBuffer buffer = new StreamBuffer();
            buffer.WriteBytes(WoitmMagicBytes);
            buffer.WriteInt16(1); // version 1

            foreach (uint itemId in items.Keys)
            {
                string csv = items[itemId];
                byte[] decryptedCsv = Encoding.UTF8.GetBytes(csv);
                byte[] key = keyResolver(itemId);
                if (key == null || key.Length < 16)
                {
                    Logger.Error($"SaveWoItm::ItemId: {itemId} skipped, key is null or to short");
                    continue;
                }

                byte[] encryptedCsv = EncryptWoItm(decryptedCsv, key);

                buffer.WriteUInt32(itemId);
                buffer.WriteInt32(0);
                buffer.WriteBytes(encryptedCsv);
            }
        }

        public byte[] EncryptWoItm(byte[] decryptedCsv, byte[] key)
        {
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
    }
}
