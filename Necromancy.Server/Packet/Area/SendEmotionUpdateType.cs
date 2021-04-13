using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEmotionUpdateType : ClientHandler
    {
        public SendEmotionUpdateType(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_emotion_update_type;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            int emote = packet.data.ReadInt32();


            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_emotion_update_type_r, res, ServerType.Area);

            SendEmotionNotifyType(client, emote);
        }

        public void SendEmotionNotifyType(NecClient client, int emote)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId); //Character ID
            res.WriteInt32(emote); //Emote ID

            router.Send(client.map, (ushort)AreaPacketId.recv_emotion_notify_type, res, ServerType.Area, client);
        }
    }
}
