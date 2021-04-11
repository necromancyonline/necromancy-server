using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendWantedJailDrawPoint : ClientHandler
    {
        public SendWantedJailDrawPoint(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_wanted_jail_draw_point;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // if 1 Message = Bail Paid
            res.WriteInt32(Util.GetRandomNumber(0, 150)); // Roll number you get when you roll
            res.WriteByte(0); //bool
            router.Send(client.map, (ushort) AreaPacketId.recv_wanted_jail_draw_point_r, res, ServerType.Area);
        }
    }
}
