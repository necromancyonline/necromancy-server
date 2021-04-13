using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Necromancy.Server.Discord.Modules
{
    public class NecServerModule : ModuleBase<SocketCommandContext>
    {
        private readonly NecServer _server;

        public NecServerModule(NecServer server)
        {
            _server = server;
        }

        [Command("status")]
        [Summary("Current Server Status")]
        public async Task Status([Remainder] string chan = null)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Server Status");
            builder.AddField("Connections", _server.clients.GetCount());
            await Context.Channel.SendMessageAsync(null, false, builder.Build());
        }

        [Command("me")]
        [Summary("Personal Profile")]
        public async Task Me()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{Context.User.Username}");
            builder.WithDescription("Not implemented");
            IDMChannel dmChannel = await Context.Message.Author.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("", false, builder.Build());
            await dmChannel.CloseAsync();
        }
    }
}
