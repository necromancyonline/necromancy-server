using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    //public class RecvEventQuestOrder : PacketResponse
    //{
    //    public RecvEventQuestOrder()
    //        : base((ushort) AreaPacketId.recv_event_quest_order, ServerType.Area)
    //    {
    //    }

    //    protected override IBuffer ToBuffer()
    //    {
    //        IBuffer res = BufferProvider.Provide();
    //        res.WriteInt32(1);
    //        res.WriteByte(1);
    //        res.WriteFixedString("", 0x61);
    //        res.WriteInt32(1);
    //        res.WriteInt32(1);
    //        res.WriteFixedString("", 0x61);
    //        res.WriteByte(1);
    //        res.WriteByte(1);
    //        res.WriteInt32(1);
    //        res.WriteInt32(1);
    //        res.WriteInt32(1);
    //        int numEntries4 = 0xA;
    //        res.WriteInt32(numEntries4);
    //        for (int i = 0; i < numEntries4; i++)
    //        {
    //            res.WriteInt32(0x10); //size of string
    //            res.WriteFixedString("", 0x10);
    //            res.WriteInt16(1);
    //            res.WriteInt32(1);
    //        }
    //        res.WriteByte(1);
    //        int numEntries5 = 0xC;
    //        for (int k = 0; k < numEntries5; k++)
    //        {
    //            res.WriteInt32(0x10); //size of string
    //            res.WriteFixedString("", 0x10);
    //            res.WriteInt16(1);
    //            res.WriteInt32(1);
    //        }
    //        res.WriteByte(1);
    //        //??res.WriteByte(1);
    //        res.WriteFixedString("", 0x181);
    //        res.WriteFixedString("", 0x181);
    //        for (int m = 0; m < 0x5; m++)
    //        {
    //            res.WriteByte(0);
    //            res.WriteInt32(0);
    //            res.WriteInt32(0);
    //            res.WriteInt32(0);
    //            res.WriteInt32(0);
    //        }
    //        res.WriteByte(0);
    //        return res;
    //    }
    //}
}
