using System;
using System.Runtime.Serialization;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class NecSecret
    {
        [DataMember(Order = 1)] public string databasePassword { get; set; }
        [DataMember(Order = 10)] public string discordBotToken { get; set; }

        public NecSecret()
        {
            string envDbPass = Environment.GetEnvironmentVariable("DB_PASS");
            if (!string.IsNullOrEmpty(envDbPass))
            {
                databasePassword = envDbPass;
            }

            string envDiscordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            if (!string.IsNullOrEmpty(envDiscordBotToken))
            {
                discordBotToken = envDiscordBotToken;
            }
        }
    }
}
