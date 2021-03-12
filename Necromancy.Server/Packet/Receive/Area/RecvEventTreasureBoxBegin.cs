using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    //public class RecvEventTreasureBoxBegin : PacketResponse
    //{
    //    public RecvEventTreasureBoxBegin()
    //        : base((ushort) AreaPacketId.recv_event_tresurebox_begin, ServerType.Area)
    //    {
    //    }

    //    protected override IBuffer ToBuffer()
    //    {
    //        IBuffer res = BufferProvider.Provide();
    //        int numEntries = 0x10;
    //        res.WriteInt32(numEntries);
    //        for (int i = 0; i < numEntries; i++)
    //        {
    //            res.WriteInt32(0);
    //        }
    //        return res;
    //    }
    //}
}
