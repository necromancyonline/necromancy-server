using System;
using System.Threading.Tasks;
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
            client.logoutCancelationCheck = false;
            int logoutCountDownInSeconds = 10;
            byte returnToCharacterSelect = packet.data.ReadByte();
            byte returnToSoulSelecct = packet.data.ReadByte();
            DateTime logoutTime = DateTime.Now.AddSeconds(logoutCountDownInSeconds);
            //client.character.characterTask.Logout(logoutTime, logOutType);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(logoutCountDownInSeconds); //time in seconds to display 
            router.Send(client, (ushort)AreaPacketId.recv_logout_start, res, ServerType.Area);

            Task.Delay(TimeSpan.FromSeconds(logoutCountDownInSeconds)).ContinueWith
            (t1 =>
                {
                    if (client.logoutCancelationCheck == false)
                    {
                        res = BufferProvider.Provide();

                        if (returnToSoulSelecct == 1) // Return to soul Select
                        {
                            server.router.Send(client, (ushort)AreaPacketId.recv_base_exit_r, res, ServerType.Area); //does nothing

                            res.WriteInt32(0);
                            server.router.Send(client, (ushort)MsgPacketId.recv_chara_select_back_soul_select_r, res, ServerType.Msg); //works, but doesnt disconnect area
                        }
                        else if (returnToCharacterSelect == 1) //return to character select.
                        {
                            server.router.Send(client, (ushort)AreaPacketId.recv_base_exit_r, res, ServerType.Area); //does nothing

                            res.WriteInt32(0);
                            server.router.Send(client, (ushort)MsgPacketId.recv_chara_select_back_r, res, ServerType.Msg); //crashes if int32 = 0
                        }
                        else  // Return to Title   also   Exit Game
                        {
                            res.WriteInt32(0);
                            server.router.Send(client, (ushort)AreaPacketId.recv_escape_start, res, ServerType.Area); //not the right recv
                        }
                    }

                    res = BufferProvider.Provide();
                    res.WriteInt32(0); //0 = nothing happens, 1 = logout failed:1
                    router.Send(client, (ushort)AreaPacketId.recv_logout_start_request_r, res, ServerType.Area); //End of response to send.

                }
            );

        }
    }
}
