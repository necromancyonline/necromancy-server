using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public interface IConnectionHandler : IHandler
    {
        void Handle(NecConnection connection, NecPacket packet);
    }
}
