using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_character_view_offset : ClientHandler
    {
        public send_character_view_offset(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_character_view_offset;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte view = packet.Data.ReadByte();

            //(client.Character != null)
                client.Character.Heading = view;

            //This is all Position and Orientation Related.
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId);//Character Instance ID. Set to movementId for a rigged dead body takeover.
            res.WriteFloat(client.Character.X); //might need to change to Target X Y Z
            res.WriteFloat(client.Character.Y);
            res.WriteFloat(client.Character.Z);
            res.WriteByte(client.Character.Heading);//View offset / Head Rotation
            res.WriteByte(client.Character.movementPose);//Character state? body rotation? TBD. should be character state, but not sure where to read that from

            //Router.Send(client.Map, (ushort)AreaPacketId.recv_self_dragon_pos_notify, res, ServerType.Area, client);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area, client);
        }
    }
}
