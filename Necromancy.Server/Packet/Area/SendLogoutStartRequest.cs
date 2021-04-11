using System;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_logout_start_request : ClientHandler
    {
        public send_logout_start_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_logout_start_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int CastingTime = 10;

            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();

            res.WriteInt32(0); //0 = nothing happens, 1 = logout failed:1
            Router.Send(client, (ushort) AreaPacketId.recv_logout_start_request_r, res, ServerType.Area);


            res2.WriteInt32(10);

            Router.Send(client, (ushort) AreaPacketId.recv_logout_start, res2, ServerType.Area);

            //Task.Delay(TimeSpan.FromMilliseconds((int) (CastingTime * 1000)))
            //.ContinueWith(t1 => { LogOutRequest(client, packet); });
            byte logOutType = packet.Data.ReadByte();
            byte x = packet.Data.ReadByte();
            DateTime logoutTime = DateTime.Now.AddSeconds(CastingTime);
            client.Character.characterTask.Logout(logoutTime , logOutType);
        }
    }
}
