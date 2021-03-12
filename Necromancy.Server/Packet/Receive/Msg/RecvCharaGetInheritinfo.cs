using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaGetInheritinfo : PacketResponse
    {
        public RecvCharaGetInheritinfo()
            : base((ushort) MsgPacketId.recv_chara_get_inheritinfo_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x64;
            res.WriteInt32(numEntries);//less than or equal to 0x64
            for (int i = 0; i < numEntries; i++) //limit is the int32 above
            {
                res.WriteInt32(0);
                res.WriteFixedString("127.0.0.1", 0x10); //size is 0x10
            }
            res.WriteInt32(0);
            res.WriteFixedString("127.0.0.1", 0x10); //size is 0x10
            res.WriteByte(0);
            return res;
        }
    }
}
