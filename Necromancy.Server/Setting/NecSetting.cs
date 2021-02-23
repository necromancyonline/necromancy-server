using System;
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
        /// Warning:
        /// Changing while having existing accounts requires to rehash all passwords.
        /// The number is log2, so adding +1 doubles the time it takes.
        /// https://wildlyinaccurate.com/bcrypt-choosing-a-work-factor/
        /// </summary>
        public const int BCryptWorkFactor = 10;

        // Connection Info
        [IgnoreDataMember] public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 0)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [IgnoreDataMember] public IPAddress AuthIpAddress { get; set; }

        [DataMember(Name = "AuthIpAddress", Order = 1)]
        public string DataAuthIpAddress
        {
            get => AuthIpAddress.ToString();
            set => AuthIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)] public ushort AuthPort { get; set; }
        [IgnoreDataMember] public IPAddress MsgIpAddress { get; set; }

        [DataMember(Name = "MsgIpAddress", Order = 3)]
        public string DataMsgIpAddress
        {
            get => MsgIpAddress.ToString();
            set => MsgIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 4)] public ushort MsgPort { get; set; }
        [IgnoreDataMember] public IPAddress AreaIpAddress { get; set; }

        [DataMember(Name = "AreaIpAddress", Order = 5)]
        public string DataAreaIpAddress
        {
            get => AreaIpAddress.ToString();
            set => AreaIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 6)] public ushort AreaPort { get; set; }

        // Server Config
        [DataMember(Order = 10)] public bool RequireRegistration { get; set; }
        [DataMember(Order = 11)] public bool RequirePin { get; set; }

        // Logging
        [DataMember(Order = 20)] public int LogLevel { get; set; }
        [DataMember(Order = 21)] public bool LogUnknownIncomingPackets { get; set; }
        [DataMember(Order = 22)] public bool LogOutgoingPackets { get; set; }
        [DataMember(Order = 23)] public bool LogIncomingPackets { get; set; }

        // Discord
        [DataMember(Order = 30)] public string DiscordBotToken { get; set; }
        [DataMember(Order = 31)] public ulong DiscordGuild { get; set; }
        [DataMember(Order = 32)] public ulong DiscordBotChannel_ServerStatus { get; set; }

        // Instance Id Pools
        [DataMember(Order = 40)] public uint PoolCharacterIdLowerBound { get; set; }
        [DataMember(Order = 41)] public uint PoolCharacterIdSize { get; set; }
        [DataMember(Order = 42)] public uint PoolNpcLowerBound { get; set; }
        [DataMember(Order = 43)] public uint PoolNpcIdSize { get; set; }
        [DataMember(Order = 44)] public uint PoolMonsterIdLowerBound { get; set; }
        [DataMember(Order = 45)] public uint PoolMonsterIdSize { get; set; }
        [DataMember(Order = 46)] public uint PoolDeadBodyIdLowerBound { get; set; }
        [DataMember(Order = 47)] public uint PoolDeadBodyIdSize { get; set; }
        [DataMember(Order = 48)] public uint PoolDynamicIdLowerBound { get; set; }
        [DataMember(Order = 49)] public uint PoolDynamicIdSize { get; set; }

        // Folder
        [DataMember(Order = 60)] public string RepositoryFolder { get; set; }
        [DataMember(Order = 61)] public string SecretsFolder { get; set; }

        // Database
        [DataMember(Order = 70)] public DatabaseSettings DatabaseSettings { get; set; }

        // Socket
        [DataMember(Order = 100)] public AsyncEventSettings AuthSocketSettings { get; set; }
        [DataMember(Order = 101)] public AsyncEventSettings MsgSocketSettings { get; set; }
        [DataMember(Order = 102)] public AsyncEventSettings AreaSocketSettings { get; set; }

        public NecSetting()
        {
            ListenIpAddress = IPAddress.Any;
            AuthIpAddress = IPAddress.Loopback;
            AuthPort = 60000;
            MsgIpAddress = IPAddress.Loopback;
            MsgPort = 60001;
            AreaIpAddress = IPAddress.Loopback;
            AreaPort = 60002;
            RequireRegistration = false;
            RequirePin = false;
            PoolDynamicIdLowerBound = 1;
            PoolDynamicIdSize = 1000000;
            PoolCharacterIdLowerBound = 200000000;
            PoolCharacterIdSize = 100000000;
            PoolNpcLowerBound = 400000000;
            PoolNpcIdSize = 100000000;
            PoolMonsterIdLowerBound = 600000000;
            PoolMonsterIdSize = 100000000;
            PoolDeadBodyIdLowerBound = 800000000;
            PoolDeadBodyIdSize = 100000000;
            LogLevel = 0;
            LogUnknownIncomingPackets = true;
            LogOutgoingPackets = true;
            LogIncomingPackets = true;
            DiscordBotToken = "";
            DiscordGuild = 541789394873352203;
            DiscordBotChannel_ServerStatus = 710367511824171019;
            RepositoryFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Client/Data/Settings");
            SecretsFolder = Path.Combine(Util.RelativeExecutingDirectory(), "Client/Data/Secrets");
            DatabaseSettings = new DatabaseSettings();
            AuthSocketSettings = new AsyncEventSettings();
            AuthSocketSettings.MaxUnitOfOrder = 2;
            MsgSocketSettings = new AsyncEventSettings();
            MsgSocketSettings.MaxUnitOfOrder = 2;
            AreaSocketSettings = new AsyncEventSettings();
            AreaSocketSettings.MaxUnitOfOrder = 2;
        }

        public NecSetting(NecSetting setting)
        {
            ListenIpAddress = setting.ListenIpAddress;
            AuthIpAddress = setting.AuthIpAddress;
            AuthPort = setting.AuthPort;
            MsgIpAddress = setting.MsgIpAddress;
            MsgPort = setting.MsgPort;
            AreaIpAddress = setting.AreaIpAddress;
            AreaPort = setting.AreaPort;
            RequireRegistration = setting.RequireRegistration;
            RequirePin = setting.RequirePin;
            PoolCharacterIdLowerBound = setting.PoolCharacterIdLowerBound;
            PoolCharacterIdSize = setting.PoolCharacterIdSize;
            PoolNpcLowerBound = setting.PoolNpcLowerBound;
            PoolNpcIdSize = setting.PoolNpcIdSize;
            PoolMonsterIdLowerBound = setting.PoolMonsterIdLowerBound;
            PoolMonsterIdSize = setting.PoolMonsterIdSize;
            PoolDeadBodyIdLowerBound = setting.PoolDeadBodyIdLowerBound;
            PoolDeadBodyIdSize = setting.PoolDeadBodyIdSize;
            PoolDynamicIdLowerBound = setting.PoolDynamicIdLowerBound;
            PoolDynamicIdSize = setting.PoolDynamicIdSize;
            LogLevel = setting.LogLevel;
            LogUnknownIncomingPackets = setting.LogUnknownIncomingPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogIncomingPackets = setting.LogIncomingPackets;
            DiscordBotToken = setting.DiscordBotToken;
            DiscordGuild = setting.DiscordGuild;
            DiscordBotChannel_ServerStatus = setting.DiscordBotChannel_ServerStatus;
            RepositoryFolder = setting.RepositoryFolder;
            SecretsFolder = setting.SecretsFolder;
            DatabaseSettings = new DatabaseSettings(setting.DatabaseSettings);
            AuthSocketSettings = new AsyncEventSettings(setting.AuthSocketSettings);
            MsgSocketSettings = new AsyncEventSettings(setting.MsgSocketSettings);
            AreaSocketSettings = new AsyncEventSettings(setting.AreaSocketSettings);
        }
    }
}
