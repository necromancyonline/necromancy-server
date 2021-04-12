using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyRequestDrawItemList : ClientHandler
    {
        public SendPartyRequestDrawItemList(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_request_draw_item_list;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            res.WriteInt32(0); //Error?
            res.WriteInt32(0); //Map ID?
            res.WriteFixedString(settings.dataAreaIpAddress, 65);
            res.WriteUInt16(settings.areaPort);

            //sub_484420
            res.WriteFloat(1);
            res.WriteFloat(2);
            res.WriteFloat(3);
            res.WriteByte(1);
            //

            //Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res, ServerType.Area);
        }
    }
}
