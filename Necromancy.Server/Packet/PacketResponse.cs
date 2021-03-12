using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class PacketResponse
    {
        private NecPacket _packet;

        public PacketResponse(ushort id, ServerType serverType)
        {
            Clients = new List<NecClient>();
            Id = id;
            ServerType = serverType;
        }

        public readonly List<NecClient> Clients;
        public ServerType ServerType { get; }
        public ushort Id { get; }

        protected abstract IBuffer ToBuffer();

        public NecPacket ToPacket()
        {
            if (_packet == null)
            {
                _packet = new NecPacket(Id, ToBuffer(), ServerType);
            }

            return _packet;
        }
    }
}
