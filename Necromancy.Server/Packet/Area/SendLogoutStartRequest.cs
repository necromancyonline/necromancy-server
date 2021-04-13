using System;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendLogoutStartRequest : ClientHandler
    {
        public SendLogoutStartRequest(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_logout_start_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int castingTime = 10;

            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();

            res.WriteInt32(0); //0 = nothing happens, 1 = logout failed:1
            router.Send(client, (ushort)AreaPacketId.recv_logout_start_request_r, res, ServerType.Area);


            res2.WriteInt32(10);

            router.Send(client, (ushort)AreaPacketId.recv_logout_start, res2, ServerType.Area);

            //Task.Delay(TimeSpan.FromMilliseconds((int) (CastingTime * 1000)))
            //.ContinueWith(t1 => { LogOutRequest(client, packet); });
            byte logOutType = packet.data.ReadByte();
            byte x = packet.data.ReadByte();
            DateTime logoutTime = DateTime.Now.AddSeconds(castingTime);
            client.character.characterTask.Logout(logoutTime, logOutType);
        }
    }
}
