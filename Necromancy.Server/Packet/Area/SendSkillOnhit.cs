using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_onhit : ClientHandler
    {
        public send_skill_onhit(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_skill_onhit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            //ToDo,  find an appropriate recv.   Recv_skill_exec?
        }
    }
}
