using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class PacketResponse
    {
        public readonly List<NecClient> Clients;
        private NecPacket _packet;

        public PacketResponse(ushort id, ServerType serverType)
        {
            Clients = new List<NecClient>();
            this.id = id;
            this.serverType = serverType;
        }

        public ServerType serverType { get; }
        public ushort id { get; }

        protected abstract IBuffer ToBuffer();

        public NecPacket ToPacket()
        {
            if (_packet == null) _packet = new NecPacket(id, ToBuffer(), serverType);

            return _packet;
        }
    }
}
