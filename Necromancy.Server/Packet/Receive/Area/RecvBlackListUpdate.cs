using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBlackListUpdate : PacketResponse
    {
        public RecvBlackListUpdate()
            : base((ushort)AreaPacketId.recv_blacklist_update, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x5;
            res.WriteInt32(numEntries); //less than or equal to 0x5
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);

                res.WriteByte(0);

                res.WriteByte(0);

                int numEntries2 = 0x31;
                res.WriteInt32(numEntries2); //less than or equal to 31.  Probably fixed string for Character Name
                for (int j = 0; j < numEntries2; j++) res.WriteByte(0);

                int numEntries3 = 0x5B;
                res.WriteInt32(numEntries3); //less than or equal to 5B.  maybe fixed string for Character Name
                for (int k = 0; k < numEntries3; k++) res.WriteByte(0);

                res.WriteByte(0);
                res.WriteInt32(0);
                int numEntries4 = 0x61;
                res.WriteInt32(numEntries4); //less than or equal to 5B.  maybe fixed string for Character Name
                for (int l = 0; l < numEntries4; l++) res.WriteByte(0);
            }

            return res;
        }
    }
}
