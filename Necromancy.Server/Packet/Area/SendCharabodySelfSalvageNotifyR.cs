using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_self_salvage_notify_r : ClientHandler
    {
        private enum myResponse : int
        {
            AcceptCollection = 0,
            DenyCollection = int.MaxValue
        }
        public send_charabody_self_salvage_notify_r(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_charabody_self_salvage_notify_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int response = packet.Data.ReadInt32(); //0 for yes, -1 for no;
            NecClient necClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode); //Salvager

            //Notify the dead client that it has accepted body collection and it's soul now posseses the salvager.
            RecvCharaBodySelfSalvageResult recvCharaBodySelfSalvageResult = new RecvCharaBodySelfSalvageResult(response);
            Router.Send(client, recvCharaBodySelfSalvageResult.ToPacket());

            //tell the salvager about your choice.
            RecvCharaBodySalvageRequest recvCharaBodySalvageRequest = new RecvCharaBodySalvageRequest(response);
            Router.Send(necClient, recvCharaBodySalvageRequest.ToPacket());


            if (response == (int)myResponse.AcceptCollection)
            {
                //tell the dead client who collected the body
                RecvCharaBodySalvageNotifySalvager recvCharaBodySalvageNotifySalvager = new RecvCharaBodySalvageNotifySalvager(necClient.Character.InstanceId, necClient.Character.Name, necClient.Soul.Name);
                Router.Send(client, recvCharaBodySalvageNotifySalvager.ToPacket());

                //tell the salvager who they collected.
                RecvCharaBodySalvageNotifyBody recvCharaBodySalvageNotifyBody = new RecvCharaBodySalvageNotifyBody(client.Character.DeadBodyInstanceId, client.Character.Name, client.Soul.Name);
                Router.Send(necClient, recvCharaBodySalvageNotifyBody.ToPacket());

                //tell the salvager to close the charabody access window.
                RecvCharaBodyAccessEnd recvCharaBodyAccessEnd = new RecvCharaBodyAccessEnd(0);
                Router.Send(necClient, recvCharaBodyAccessEnd.ToPacket());

                //Tell the deadBody to disappear from the map, as it's being carried.
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.Character.DeadBodyInstanceId);
                Router.Send(client.Map, recvObjectDisappearNotify);

                //Tell the deadClient it can see the salvager even in soul form now.
                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(necClient.Character, necClient.Soul.Name);
                Router.Send(client, cData.ToPacket());

                //remove the deadBody from the map so newly entering players can't see it.
                client.Map.DeadBodies.Remove(client.Character.DeadBodyInstanceId);

                //Add the deadBody to the client that's carrying it for Movement Sends
                necClient.BodyCollection.Add(client.Character.DeadBodyInstanceId, client);

                //move the soulState client to the salvagers position
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteUInt32(client.Character.InstanceId);
                res5.WriteFloat(necClient.Character.X);
                res5.WriteFloat(necClient.Character.Y);
                res5.WriteFloat(necClient.Character.Z);
                res5.WriteByte(necClient.Character.Heading); //Heading
                res5.WriteByte(necClient.Character.movementAnim); //state
                Router.Send(client, (ushort)AreaPacketId.recv_object_point_move_notify, res5, ServerType.Area);
            }



        }


    }



}

