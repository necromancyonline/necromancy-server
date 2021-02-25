using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_self_salvage_notify_r : ClientHandler
    {
        public send_charabody_self_salvage_notify_r(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_charabody_self_salvage_notify_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int response = packet.Data.ReadInt32(); //0 for yes, -1 for no;

            //send response _r
            RecvCharaBodySalvageRequest recvCharaBodySalvageRequest = new RecvCharaBodySalvageRequest(response);
            Router.Send(client.Map, recvCharaBodySalvageRequest.ToPacket());

            RecvCharaBodySalvageNotifySalvager recvCharaBodySalvageNotifySalvager = new RecvCharaBodySalvageNotifySalvager(200000002, client.Character.Name, client.Soul.Name);
            //Router.Send(client, recvCharaBodySalvageNotifySalvager.ToPacket());


        }


    }



}

