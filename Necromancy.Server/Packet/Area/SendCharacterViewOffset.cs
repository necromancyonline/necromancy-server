using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharacterViewOffset : ClientHandler
    {
        public SendCharacterViewOffset(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_character_view_offset;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte view = packet.data.ReadByte();

            //(client.Character != null)
            client.character.heading = view;

            //This is all Position and Orientation Related.
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId); //Character Instance ID. Set to movementId for a rigged dead body takeover.
            res.WriteFloat(client.character.x); //might need to change to Target X Y Z
            res.WriteFloat(client.character.y);
            res.WriteFloat(client.character.z);
            res.WriteByte(client.character.heading); //View offset / Head Rotation
            res.WriteByte(client.character.movementPose); //Character state? body rotation? TBD. should be character state, but not sure where to read that from

            //Router.Send(client.Map, (ushort)AreaPacketId.recv_self_dragon_pos_notify, res, ServerType.Area, client);

            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area, client);
        }
    }
}
