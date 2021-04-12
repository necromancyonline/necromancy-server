using System;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Necromancy.Server.Database.Sql
{
    public class ScriptRunner
    {
        private const string _Delimiter = ";";
        private const bool _FullLineDelimiter = false;

        private static readonly ILogger _Logger = LogProvider.Logger(typeof(ScriptRunner));

        private readonly IDatabase _database;

        /**
         * Default constructor
         */
        public ScriptRunner(IDatabase database)
        {
            _database = database;
        }

        public void Run(string path)
        {
            int index = 0;
            try
            {
                string[] file = File.ReadAllLines(path);
                StringBuilder command = null;
                for (; index < file.Length; index++)
                {
                    string line = file[index];
                    if (command == null) command = new StringBuilder();

                    string trimmedLine = line.Trim();

                    if (trimmedLine.Length < 1)
                    {
                        // Do nothing
                    }
                    else if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("--"))
                    {
                        // Print comment
                    }
                    else if (!_FullLineDelimiter && trimmedLine.EndsWith(_Delimiter)
                             || _FullLineDelimiter && trimmedLine == _Delimiter)
                    {
                        command.Append(
                            line.Substring(0, line.LastIndexOf(_Delimiter, StringComparison.InvariantCulture)));
                        command.Append(" ");
                        _database.Execute(command.ToString());
                        command = null;
                    }
                    else
                    {
                        command.Append(line);
                        command.Append("\n");
                    }
                }

                if (command != null)
                {
                    string cmd = command.ToString();
                    if (string.IsNullOrWhiteSpace(cmd))
                    {
                        //do nothing;
                    }
                    else
                    {
                        _database.Execute(cmd);
                    }
                }
            }
            catch (Exception exception)
            {
                _Logger.Error($"Sql error at Line: {index}");
                _Logger.Exception(exception);
            }
        }
    }
}
