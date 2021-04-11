using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_chara_pose_ladder : ClientHandler
    {
        public send_chara_pose_ladder(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_chara_pose_ladder;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

           
            res.WriteInt32(0);  

            Router.Send(client, (ushort) AreaPacketId.recv_chara_pose_ladder_r, res, ServerType.Area);
        }
    }
}
