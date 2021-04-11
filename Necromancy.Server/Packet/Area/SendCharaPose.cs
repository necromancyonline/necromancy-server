using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharaPose : ClientHandler
    {
        public SendCharaPose(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_chara_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            client.character.charaPose = packet.data.ReadInt32();

            res.WriteInt32(0);

            router.Send(client, (ushort) AreaPacketId.recv_chara_pose_r, res, ServerType.Area);

            SendCharaPoseNotify(client);
        }



        private void SendCharaPoseNotify(NecClient client)
        {


                IBuffer res = BufferProvider.Provide();

                res.WriteUInt32(client.character.instanceId);//Character ID
                res.WriteInt32(client.character.charaPose); //Character pose



                router.Send(client.map, (ushort)AreaPacketId.recv_chara_pose_notify, res, ServerType.Area, client);

        }
    }
}
