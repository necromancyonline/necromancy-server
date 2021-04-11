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
            clients = new List<NecClient>();
            this.id = id;
            this.serverType = serverType;
        }

        public readonly List<NecClient> clients;
        public ServerType serverType { get; }
        public ushort id { get; }

        protected abstract IBuffer ToBuffer();

        public NecPacket ToPacket()
        {
            if (_packet == null)
            {
                _packet = new NecPacket(id, ToBuffer(), serverType);
            }

            return _packet;
        }
    }
}
