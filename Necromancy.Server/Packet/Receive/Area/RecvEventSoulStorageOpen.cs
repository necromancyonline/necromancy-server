using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSoulStorageOpen : PacketResponse
    {
        private readonly NecClient _client;

        public RecvEventSoulStorageOpen(NecClient client)
            : base((ushort)AreaPacketId.recv_event_soul_storage_open, ServerType.Area)
        {
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_client.soul.warehouseGold); // Gold in the storage
            int numEntries = 0x1A;
            res.WriteInt32(numEntries); //Less than or equal to 0x1A; new
            for (int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 0x19; j++)
                    res.WriteByte(0); //new
                res.WriteByte(0); //new
            }

            res.WriteByte(0); //Bool; new
            return res;
        }
    }
}
