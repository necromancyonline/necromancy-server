using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguemapEntryOpen : PacketResponse
    {
        public RecvRoguemapEntryOpen()
            : base((ushort)AreaPacketId.recv_roguemap_entry_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0xA
            for (int k = 0; k < numEntries; k++)
            {
                //SUB 496800
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteFixedString("Xeno", 0x301);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0xA; j++) //must be 0xA
                    res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                //End Sub 496800
            }

            for (int k = 0; k < 0x2; k++)
            {
                //sub 496AB0
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x5B);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0xA; j++) res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0); //suspect
            }

            return res;
        }
    }
}
