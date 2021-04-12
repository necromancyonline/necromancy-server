using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Common;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class NecSetting
    {
        /// <summary>
        ///     Warning:
        ///     Changing while having existing accounts requires to rehash all passwords.
        ///     The number is log2, so adding +1 doubles the time it takes.
        ///     https://wildlyinaccurate.com/bcrypt-choosing-a-work-factor/
        /// </summary>
        public const int B_CRYPT_WORK_FACTOR = 10;

        public NecSetting()
        {
            listenIpAddress = IPAddress.Any;
            authIpAddress = IPAddress.Loopback;
            authPort = 60000;
            msgIpAddress = IPAddress.Loopback;
            msgPort = 60001;
            areaIpAddress = IPAddress.Loopback;
            areaPort = 60002;
            requireRegistration = false;
            requirePin = false;
            poolDynamicIdLowerBound = 1;
            poolDynamicIdSize = 1000000;
            poolCharacterIdLowerBound = 200000000;
            poolCharacterIdSize = 100000000;
            poolNpcLowerBound = 400000000;
            poolNpcIdSize = 100000000;
            poolMonsterIdLowerBound = 600000000;
            poolMonsterIdSize = 100000000;
            poolDeadBodyIdLowerBound = 800000000;
            poolDeadBodyIdSize = 100000000;
            logLevel = 0;
            logUnknownIncomingPackets = true;
            logOutgoingPackets = true;
            logIncomingPackets = true;
            discordBotToken = "";
            discordGuild = 541789394873352203;
            discordBotChannelServerStatus = 710367511824171019;
            repositoryFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Client/Data/Settings");
            secretsFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Client/Data/Secrets");
            databaseSettings = new DatabaseSettings();
            authSocketSettings = new AsyncEventSettings();
            authSocketSettings.MaxUnitOfOrder = 2;
            msgSocketSettings = new AsyncEventSettings();
            msgSocketSettings.MaxUnitOfOrder = 2;
            areaSocketSettings = new AsyncEventSettings();
            areaSocketSettings.MaxUnitOfOrder = 2;
        }

        public NecSetting(NecSetting setting)
        {
            listenIpAddress = setting.listenIpAddress;
            authIpAddress = setting.authIpAddress;
            authPort = setting.authPort;
            msgIpAddress = setting.msgIpAddress;
            msgPort = setting.msgPort;
            areaIpAddress = setting.areaIpAddress;
            areaPort = setting.areaPort;
            requireRegistration = setting.requireRegistration;
            requirePin = setting.requirePin;
            poolCharacterIdLowerBound = setting.poolCharacterIdLowerBound;
            poolCharacterIdSize = setting.poolCharacterIdSize;
            poolNpcLowerBound = setting.poolNpcLowerBound;
            poolNpcIdSize = setting.poolNpcIdSize;
            poolMonsterIdLowerBound = setting.poolMonsterIdLowerBound;
            poolMonsterIdSize = setting.poolMonsterIdSize;
            poolDeadBodyIdLowerBound = setting.poolDeadBodyIdLowerBound;
            poolDeadBodyIdSize = setting.poolDeadBodyIdSize;
            poolDynamicIdLowerBound = setting.poolDynamicIdLowerBound;
            poolDynamicIdSize = setting.poolDynamicIdSize;
            logLevel = setting.logLevel;
            logUnknownIncomingPackets = setting.logUnknownIncomingPackets;
            logOutgoingPackets = setting.logOutgoingPackets;
            logIncomingPackets = setting.logIncomingPackets;
            discordBotToken = setting.discordBotToken;
            discordGuild = setting.discordGuild;
            discordBotChannelServerStatus = setting.discordBotChannelServerStatus;
            repositoryFolder = setting.repositoryFolder;
            secretsFolder = setting.secretsFolder;
            databaseSettings = new DatabaseSettings(setting.databaseSettings);
            authSocketSettings = new AsyncEventSettings(setting.authSocketSettings);
            msgSocketSettings = new AsyncEventSettings(setting.msgSocketSettings);
            areaSocketSettings = new AsyncEventSettings(setting.areaSocketSettings);
        }

        // Connection Info
        [IgnoreDataMember] public IPAddress listenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 0)]
        public string dataListenIpAddress
        {
            get => listenIpAddress.ToString();
            set => listenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [IgnoreDataMember] public IPAddress authIpAddress { get; set; }

        [DataMember(Name = "AuthIpAddress", Order = 1)]
        public string dataAuthIpAddress
        {
            get => authIpAddress.ToString();
            set => authIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)] public ushort authPort { get; set; }
        [IgnoreDataMember] public IPAddress msgIpAddress { get; set; }

        [DataMember(Name = "MsgIpAddress", Order = 3)]
        public string dataMsgIpAddress
        {
            get => msgIpAddress.ToString();
            set => msgIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 4)] public ushort msgPort { get; set; }
        [IgnoreDataMember] public IPAddress areaIpAddress { get; set; }

        [DataMember(Name = "AreaIpAddress", Order = 5)]
        public string dataAreaIpAddress
        {
            get => areaIpAddress.ToString();
            set => areaIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 6)] public ushort areaPort { get; set; }

        // Server Config
        [DataMember(Order = 10)] public bool requireRegistration { get; set; }
        [DataMember(Order = 11)] public bool requirePin { get; set; }

        // Logging
        [DataMember(Order = 20)] public int logLevel { get; set; }
        [DataMember(Order = 21)] public bool logUnknownIncomingPackets { get; set; }
        [DataMember(Order = 22)] public bool logOutgoingPackets { get; set; }
        [DataMember(Order = 23)] public bool logIncomingPackets { get; set; }

        // Discord
        [DataMember(Order = 30)] public string discordBotToken { get; set; }
        [DataMember(Order = 31)] public ulong discordGuild { get; set; }
        [DataMember(Order = 32)] public ulong discordBotChannelServerStatus { get; set; }

        // Instance Id Pools
        [DataMember(Order = 40)] public uint poolCharacterIdLowerBound { get; set; }
        [DataMember(Order = 41)] public uint poolCharacterIdSize { get; set; }
        [DataMember(Order = 42)] public uint poolNpcLowerBound { get; set; }
        [DataMember(Order = 43)] public uint poolNpcIdSize { get; set; }
        [DataMember(Order = 44)] public uint poolMonsterIdLowerBound { get; set; }
        [DataMember(Order = 45)] public uint poolMonsterIdSize { get; set; }
        [DataMember(Order = 46)] public uint poolDeadBodyIdLowerBound { get; set; }
        [DataMember(Order = 47)] public uint poolDeadBodyIdSize { get; set; }
        [DataMember(Order = 48)] public uint poolDynamicIdLowerBound { get; set; }
        [DataMember(Order = 49)] public uint poolDynamicIdSize { get; set; }

        // Folder
        [DataMember(Order = 60)] public string repositoryFolder { get; set; }
        [DataMember(Order = 61)] public string secretsFolder { get; set; }

        // Database
        [DataMember(Order = 70)] public DatabaseSettings databaseSettings { get; set; }

        // Socket
        [DataMember(Order = 100)] public AsyncEventSettings authSocketSettings { get; set; }
        [DataMember(Order = 101)] public AsyncEventSettings msgSocketSettings { get; set; }
        [DataMember(Order = 102)] public AsyncEventSettings areaSocketSettings { get; set; }
    }
}
