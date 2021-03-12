using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_script_play_r : ClientHandler
    {
        public send_event_script_play_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_script_play_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
                (t1 =>
                    {
                                IBuffer res = BufferProvider.Provide();
                                res.WriteByte(0);
                                Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

                    }
                );

        }

    }
}
