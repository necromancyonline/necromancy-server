using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public interface IClientHandler : IHandler
    {
        void Handle(NecClient client, NecPacket packet);
    }
}
