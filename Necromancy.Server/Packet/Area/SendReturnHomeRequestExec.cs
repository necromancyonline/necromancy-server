using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_return_home_request_exec : ClientHandler
    {
        public send_return_home_request_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_return_home_request_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);//Error lookup(I think) 0 - no error, 1 - Unable to use return command
            res.WriteInt32(0);//Stores locally the amount of time before you can use the command again. (Can't get it to tick down.)

            Router.Send(client, (ushort)AreaPacketId.recv_return_home_request_exec_r, res, ServerType.Area);
        }
    }
}

