using Necromancy.Server.Packet;

namespace Necromancy.Server.Chat.Command
{
    public abstract class ServerChatCommand : ChatCommand
    {
        protected ServerChatCommand(NecServer server)
        {
            this.server = server;
            router = this.server.router;
        }

        protected NecServer server { get; }
        protected PacketRouter router { get; }
    }
}
