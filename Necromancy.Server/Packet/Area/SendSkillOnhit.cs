using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendSkillOnhit : ClientHandler
    {
        public SendSkillOnhit(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_skill_onhit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            //ToDo,  find an appropriate recv.   Recv_skill_exec?
        }
    }
}
