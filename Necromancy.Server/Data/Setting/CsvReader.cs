using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public abstract class CsvReader<T>
    {
        private const int BufferSize = 128;
        //public static readonly Encoding encodingShiftJis = Encoding.GetEncoding(932);
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CsvReader<T>));

        public CsvReader()
        {
        }

        public List<T> Read(string path)
        {
            Logger.Info($"Reading {path}");
            List<T> items = new List<T>();
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            ProcessLine(line, items);
                        }
                    }
                }
            }

            return items;
        }

        protected abstract T CreateInstance(string[] properties);

        /// <summary>
        /// Defines the minimum number of items to expect.
        /// If the entry has less items it will be discarded without processing.
        /// </summary>
        protected abstract int NumExpectedItems { get; }

        private void ProcessLine(string line, ICollection<T> items)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            if (line.StartsWith('#'))
            {
                // Ignoring Comment
                return;
            }
            if (line.StartsWith(','))
            {
                // Ignoring null ID
                return;
            }

            string[] properties = line.Split(",");
            if (properties.Length <= 0)
            {
                return;
            }

            if (properties.Length < NumExpectedItems)
            {
                Logger.Error(
                    $"Skipping Line: '{line}' expected {NumExpectedItems} values but got {properties.Length}");
                return;
            }

            T item = default;
            try
            {
                item = CreateInstance(properties);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            if (item == null)
            {
                Logger.Error($"Skipping Line: '{line}' could not be converted");
                return;
            }

            items.Add(item);
        }

        /// <summary>
        /// Only produce null value if the string is null or empty.
        /// Otherwise try to parse the content and return the number on success.
        /// Return false if the string contains an invalid number.
        /// </summary>
        protected bool TryParseNullableInt(string str, out int? value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = null;
                return true;
            }

            if (int.TryParse(str, out int val))
            {
                value = val;
                return true;
            }

            value = null;
            return false;
        }
    }
}
