using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Necromancy.Server.Discord.Services
{
    public class TextHandlingService
    {
        public TextHandlingService(IServiceProvider services)
        {
            DiscordSocketClient discord = services.GetRequiredService<DiscordSocketClient>();
            discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;

            if (message.Source != MessageSource.User) return;

            if (message.Content.Contains("server ready", StringComparison.InvariantCultureIgnoreCase))
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Hello my fond customer, you inquired about the server status?");
                builder.WithImageUrl("https://i.ibb.co/LZv8rGT/Capture.png");
                await message.Channel.SendMessageAsync("", false, builder.Build());
            }
        }
    }
}
