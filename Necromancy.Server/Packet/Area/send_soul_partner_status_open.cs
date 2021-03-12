using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_soul_partner_status_open : ClientHandler
    {
        public send_soul_partner_status_open(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_soul_partner_status_open;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0x5
            for (int k = 0; k < numEntries; k++)
            {
                //sub_495C70
                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteFixedString("Xeno", 0x5B);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);

                for (int i = 0; i < 0x3; i++)
                {
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    for (int j = 0; j < 0x7; j++)
                    {
                        res.WriteInt16(0);
                    }
                }
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteByte(0);

                for (int i = 0; i < 0x3; i++)
                {
                    res.WriteByte(0);
                }

                for (int i = 0; i < 0x5; i++)
                {
                    res.WriteInt32(0);
                }

                for (int i = 0; i < 0x5; i++)
                {
                    res.WriteByte(0);
                }
                //-----endsub
            }

            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt64(0);
            Router.Send(client, (ushort) AreaPacketId.recv_soul_partner_status_open_r, res, ServerType.Area);
            //There is also a notify
        }
    }
}
