using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_wanted_jail_payment : ClientHandler
    {
        public send_wanted_jail_payment(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_wanted_jail_payment;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_wanted_jail_payment_r, res, ServerType.Area);
        }
    }
}
