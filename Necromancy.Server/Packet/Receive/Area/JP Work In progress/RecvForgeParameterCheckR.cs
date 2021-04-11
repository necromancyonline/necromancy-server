using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_forge_parameter_check_r : PacketResponse
    {
        public recv_forge_parameter_check_r()
            : base((ushort) AreaPacketId.recv_forge_parameter_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            int numEntries2 = 0x5; //Must be 5.  not a loop in assm.
            for (int i = 0; i < numEntries2; i++)
            {
                //492810
                res.WriteInt64(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt16(0);

                int numEntries3 = 0x5; //must be 0x5
                for (int k = 0; k < numEntries3; k++)
                {
                    res.WriteInt32(0);
                }
                int numEntries4 = 0x5; //must be 0x5
                for (int k = 0; k < numEntries4; k++)
                {
                    res.WriteInt32(0);
                }
                int numEntries5 = 0x5; //must be 0x5
                for (int k = 0; k < numEntries5; k++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("Ok", 0x2);
                    res.WriteInt16(0);
                    res.WriteInt16(0);
                }
            }
            return res;
        }
    }
}
