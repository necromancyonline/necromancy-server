﻿/*
 * This file is part of Necromancy.Cli
 *
 * Necromancy.Cli is a server implementation for the game "Wizardry Online".
 * Copyright (C) 2019-2020 Necromancy Team
 *
 * Github: https://github.com/necromancyonline/necromancy-server
 *
 * Necromancy.Cli is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Necromancy.Cli is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Necromancy.Cli. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Arrowgene.Services.Logging;
using Necromancy.Cli.Command;
using Necromancy.Cli.Command.Commands;
using Necromancy.Server.Common;

namespace Necromancy.Cli
{
    internal class Program
    {
        public const char CliSeparator = ' ';
        public const char CliValueSeparator = '=';

        private static void Main(string[] args)
        {
            Console.WriteLine("Program started");
            Program program = new Program();
            if (args.Length > 0)
            {
                program.RunArguments(args);
            }
            else
            {
                program.RunInteractive();
            }

            Console.WriteLine("Program ended");
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<string> _inputQueue;
        private readonly Thread _consoleThread;
        private readonly Dictionary<string, IConsoleCommand> _commands;
        private readonly ILogger _logger;
        private readonly LogWriter _logWriter;

        private Program()
        {
            _logger = LogProvider.Logger(this);
            _commands = new Dictionary<string, IConsoleCommand>();
            _inputQueue = new BlockingCollection<string>();
            _cancellationTokenSource = new CancellationTokenSource();
            _consoleThread = new Thread(ReadConsoleThread);
            _logWriter = new LogWriter();
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
        }

        private void LoadCommands()
        {
            AddCommand(new ShowCommand());
            AddCommand(new UnpackCommand());
            AddCommand(new ServerCommand(_logWriter));
            AddCommand(new HelpCommand(_commands));
            AddCommand(new ExitCommand());
        }

        private void RunArguments(string[] arguments)
        {
            if (arguments.Length <= 0)
            {
                _logger.Error("Invalid input");
                return;
            }

            LoadCommands();
            ShowCopyright();
            _logger.Info("Argument Mode");
            _logger.Info("Press `e'-key to exit.");

            ProcessArguments(arguments);
            _logger.Info("Command Completed");
            _logger.Info("Press `e'-key to exit.");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            while (keyInfo.Key != ConsoleKey.E)
            {
                keyInfo = Console.ReadKey();
            }

            ShutdownCommands();
        }

        private void RunInteractive()
        {
            LoadCommands();
            ShowCopyright();

            _logger.Info("Interactive Mode");

            _consoleThread.IsBackground = true;
            _consoleThread.Name = "Console Thread";
            _consoleThread.Start();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                string line;
                try
                {
                    line = _inputQueue.Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    line = null;
                }

                if (line == null)
                {
                    // Ctrl+Z, Ctrl+C or error
                    break;
                }

                string[] arguments = Util.ParseTextArguments(line, CliSeparator, '"');
                if (arguments.Length <= 0)
                {
                    _logger.Error("Invalid input: '{line}'. Type 'help' for a list of available commands.");
                    continue;
                }

                CommandResultType result = ProcessArguments(arguments);
                if (result == CommandResultType.Exit)
                {
                    break;
                }

                if (result == CommandResultType.Continue)
                {
                    continue;
                }

                if (result == CommandResultType.Completed)
                {
                    _logger.Info("Command Completed");
                    continue;
                }
            }

            StopReadConsoleThread();
            ShutdownCommands();
        }

        private CommandResultType ProcessArguments(string[] arguments)
        {
            ConsoleParameter parameter = ParseParameter(arguments);

            if (!_commands.ContainsKey(parameter.Key))
            {
                _logger.Error(
                    $"Command: '{parameter.Key}' not available. Type `help' for a list of available commands.");
                return CommandResultType.Continue;
            }

            IConsoleCommand consoleCommand = _commands[parameter.Key];
            return consoleCommand.Handle(parameter);
        }

        /// <summary>
        /// Parses the input text into arguments and switches.
        /// </summary>
        private ConsoleParameter ParseParameter(string[] args)
        {
            if (args.Length <= 0)
            {
                _logger.Error("Invalid input. Type 'help' for a list of available commands.");
                return null;
            }

            string paramKey = args[0];
            int cmdLength = args.Length - 1;
            string[] newArguments = new string[cmdLength];
            if (cmdLength > 0)
            {
                Array.Copy(args, 1, newArguments, 0, cmdLength);
            }

            args = newArguments;

            ConsoleParameter parameter = new ConsoleParameter(paramKey);
            foreach (string arg in args)
            {
                int count = CountChar(arg, CliValueSeparator);
                if (count == 1)
                {
                    string[] keyValue = arg.Split(CliValueSeparator);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string value = keyValue[1];
                        if (key.StartsWith('-'))
                        {
                            key = key.Substring(1);
                            if (key.Length <= 0 || parameter.SwitchMap.ContainsKey(key))
                            {
                                _logger.Error($"Invalid switch key: '{key}' is empty or duplicated.");
                                continue;
                            }

                            parameter.SwitchMap.Add(key, value);
                            continue;
                        }

                        if (key.Length <= 0 || parameter.ArgumentMap.ContainsKey(key))
                        {
                            _logger.Error($"Invalid argument key: '{key}' is empty or duplicated.");
                            continue;
                        }

                        parameter.ArgumentMap.Add(key, value);
                        continue;
                    }
                }

                if (arg.StartsWith('-'))
                {
                    string switchStr = arg.Substring(1);
                    if (switchStr.Length <= 0 || parameter.Switches.Contains(switchStr))
                    {
                        _logger.Error($"Invalid switch: '{switchStr}' is empty or duplicated.");
                        continue;
                    }

                    parameter.Switches.Add(switchStr);
                    continue;
                }

                if (arg.Length <= 0 || parameter.Switches.Contains(arg))
                {
                    _logger.Error($"Invalid argument: '{arg}' is empty or duplicated.");
                    continue;
                }

                parameter.Arguments.Add(arg);
            }

            return parameter;
        }

        private int CountChar(string str, char chr)
        {
            int count = 0;
            foreach (char c in str)
            {
                if (c == chr)
                {
                    count++;
                }
            }

            return count;
        }


        private void ReadConsoleThread()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                string line = Console.ReadLine();
                try
                {
                    _inputQueue.Add(line, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Ignored
                }
            }
        }

        private void StopReadConsoleThread()
        {
            if (_consoleThread != null
                && _consoleThread.IsAlive
                && Thread.CurrentThread != _consoleThread
            )
            {
                try
                {
                    _consoleThread.Interrupt();
                }
                catch (Exception)
                {
                    // Ignored
                }

                if (!_consoleThread.Join(TimeSpan.FromMilliseconds(500)))
                {
                    try
                    {
                        _consoleThread.Abort();
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
            }
        }

        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void AddCommand(IConsoleCommand command)
        {
            _commands.Add(command.Key, command);
        }

        private void ShutdownCommands()
        {
            foreach (IConsoleCommand command in _commands.Values)
            {
                command.Shutdown();
            }
        }


        private void ShowCopyright()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("Necromancy.Cli Copyright (C) 2019-2020 Necromancy Team");
            sb.Append(Environment.NewLine);
            sb.Append("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
            sb.Append(Environment.NewLine);
            sb.Append("This is free software, and you are welcome to redistribute it");
            sb.Append(Environment.NewLine);
            sb.Append("under certain conditions; type `show c' for details.");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            _logger.Info(sb.ToString());
        }
    }
}
