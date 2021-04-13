using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class ConnectionHandler : Handler, IConnectionHandler
    {
        protected ConnectionHandler(NecServer server) : base(server)
        {
        }

        public abstract void Handle(NecConnection client, NecPacket packet);
    }
}
