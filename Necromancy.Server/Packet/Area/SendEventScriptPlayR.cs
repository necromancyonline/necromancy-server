using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventScriptPlayR : ClientHandler
    {
        public SendEventScriptPlayR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_script_play_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
            (t1 =>
                {
                    IBuffer res = BufferProvider.Provide();
                    res.WriteByte(0);
                    router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
                }
            );
        }
    }
}
