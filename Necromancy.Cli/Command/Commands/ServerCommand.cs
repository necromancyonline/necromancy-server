using System;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server;
using Necromancy.Server.Logging;
using Necromancy.Server.Setting;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand, ISwitchConsumer
    {
        private const string SETTING_FILE = "server_setting.json";
        private const string SECRET_FILE = "server_secret.json";
        public static readonly ILogger Logger = LogProvider.Logger(typeof(ServerCommand));
        private NecServer _server;
        private bool _service;

        public ServerCommand()
        {
            _service = false;
            switches = new List<ISwitchProperty>();
            switches.Add(
                new SwitchProperty<bool>(
                    "--service",
                    "--service=true (true|false)",
                    "Run the server as a dedicated service",
                    bool.TryParse,
                    result => _service = result
                )
            );
        }

        public override string key => "server";


        public override string description =>
            $"Wizardry Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public List<ISwitchProperty> switches { get; }

        public override void Shutdown()
        {
            if (_server != null) _server.Stop();
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (_server == null)
            {
                SettingProvider settingProvider = new SettingProvider();
                NecSetting setting = settingProvider.Load<NecSetting>(SETTING_FILE);
                if (setting == null)
                {
                    Logger.Info($"No `{SETTING_FILE}` file found, creating new");
                    setting = new NecSetting();
                    settingProvider.Save(setting, SETTING_FILE);
                }
                else
                {
                    Logger.Info($"Loaded Setting from: {settingProvider.GetSettingsPath(SETTING_FILE)}");
                }

                SettingProvider secretsProvider = new SettingProvider(setting.secretsFolder);
                NecSecret secret = secretsProvider.Load<NecSecret>(SECRET_FILE);
                if (secret == null)
                {
                    Logger.Info($"No `{SECRET_FILE}` file found, creating new");
                    secret = new NecSecret();
                    secretsProvider.Save(secret, SECRET_FILE);
                }
                else
                {
                    Logger.Info($"Loaded Secrets from: {secretsProvider.GetSettingsPath(SECRET_FILE)}");
                }

                setting.discordBotToken = secret.discordBotToken;
                setting.databaseSettings.password = secret.databasePassword;

                LogProvider.Configure<NecLogger>(setting);
                _server = new NecServer(setting);
            }

            if (parameter.arguments.Contains("start"))
            {
                _server.Start();
                if (_service)
                {
                    while (_server.running) Thread.Sleep(TimeSpan.FromMinutes(5));

                    return CommandResultType.Exit;
                }

                return CommandResultType.Completed;
            }

            if (parameter.arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }
    }
}
