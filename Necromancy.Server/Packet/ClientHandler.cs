using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class ClientHandler : Handler, IClientHandler
    {
        protected ClientHandler(NecServer server) : base(server)
        {
        }

        public abstract void Handle(NecClient client, NecPacket packet);
    }
}
