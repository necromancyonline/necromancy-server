using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestGetRogueMissionQuestWorksR : PacketResponse
    {
        public RecvQuestGetRogueMissionQuestWorksR()
            : base((ushort) AreaPacketId.recv_quest_get_rogue_mission_quest_works_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);

            res.WriteInt32(numEntries); //less than 0x14
            for (int k = 0; k < numEntries; k++)
            {
                //sub_493990
                //>sub_4936D0
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0); //sub_491980
                res.WriteInt32(0); //sub_491980
                res.WriteInt32(0); //sub_491980
                res.WriteInt32(0); //sub_491980
                res.WriteInt32(0); //sub_491980
                res.WriteInt32(0); //sub_491980
                                   //suspicious Ret
                for (int j = 0; j < 0xA; j++)
                {
                    //sub_493640
                    res.WriteInt32(0);
                    res.WriteFixedString("Xeno", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }
                res.WriteByte(0);
                for (int j = 0; j < 0xc; j++)
                {
                    //sub_493640
                    res.WriteInt32(0);
                    res.WriteFixedString("Xeno", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }
                res.WriteByte(0);
                res.WriteInt32(0);
                //end>sub_4936D0
                res.WriteFixedString("Xeno", 0x181);
                res.WriteInt64(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x181);
                for (int j = 0; j < 0x5; j++)
                {
                    //sub_493910
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);//sub_491980
                    res.WriteInt32(0);//sub_491980
                    //end_sub_493910
                }
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteFloat(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
                //end_sub_493990

            }
            return res;
        }
    }
}
