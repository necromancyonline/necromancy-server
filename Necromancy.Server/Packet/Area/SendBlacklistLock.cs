using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBlacklistLock : ClientHandler
    {
        public SendBlacklistLock(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_blacklist_lock;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);


            router.Send(client, (ushort)AreaPacketId.recv_blacklist_lock_r, res, ServerType.Area);
        }
    }
}
