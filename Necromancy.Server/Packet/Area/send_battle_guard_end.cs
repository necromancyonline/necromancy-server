using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_end : ClientHandler
    {
        public send_battle_guard_end(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_guard_end;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            Router.Send(client, (ushort)AreaPacketId.recv_battle_guard_end_self, res, ServerType.Area);

            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_battle_guard_end_r, res, ServerType.Area);

            client.Character.ClearStateBit(CharacterState.BlockPose);


            res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_battle_guard_end_notify, res, ServerType.Area);

        }
    }
}
