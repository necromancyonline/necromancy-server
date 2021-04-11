using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodySelfSalvageNotifyR : ClientHandler
    {
        private enum MyResponse : int
        {
            AcceptCollection = 0,
            DenyCollection = int.MaxValue
        }
        public SendCharabodySelfSalvageNotifyR(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_charabody_self_salvage_notify_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int response = packet.data.ReadInt32(); //0 for yes, -1 for no;
            NecClient necClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode); //Salvager

            //Notify the dead client that it has accepted body collection and it's soul now posseses the salvager.
            RecvCharaBodySelfSalvageResult recvCharaBodySelfSalvageResult = new RecvCharaBodySelfSalvageResult(response);
            router.Send(client, recvCharaBodySelfSalvageResult.ToPacket());

            //tell the salvager about your choice.
            RecvCharaBodySalvageRequest recvCharaBodySalvageRequest = new RecvCharaBodySalvageRequest(response);
            router.Send(necClient, recvCharaBodySalvageRequest.ToPacket());


            if (response == (int)MyResponse.AcceptCollection)
            {
                //tell the dead client who collected the body
                RecvCharaBodySalvageNotifySalvager recvCharaBodySalvageNotifySalvager = new RecvCharaBodySalvageNotifySalvager(necClient.character.instanceId, necClient.character.name, necClient.soul.name);
                router.Send(client, recvCharaBodySalvageNotifySalvager.ToPacket());

                //tell the salvager who they collected.
                RecvCharaBodySalvageNotifyBody recvCharaBodySalvageNotifyBody = new RecvCharaBodySalvageNotifyBody(client.character.deadBodyInstanceId, client.character.name, client.soul.name);
                router.Send(necClient, recvCharaBodySalvageNotifyBody.ToPacket());

                //tell the salvager to close the charabody access window.
                RecvCharaBodyAccessEnd recvCharaBodyAccessEnd = new RecvCharaBodyAccessEnd(0);
                router.Send(necClient, recvCharaBodyAccessEnd.ToPacket());

                //Tell the deadBody to disappear from the map, as it's being carried.
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.character.deadBodyInstanceId);
                router.Send(client.map, recvObjectDisappearNotify);

                //Tell the deadClient it can see the salvager even in soul form now.
                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(necClient.character, necClient.soul.name);
                router.Send(client, cData.ToPacket());

                //remove the deadBody from the map so newly entering players can't see it.
                client.map.deadBodies.Remove(client.character.deadBodyInstanceId);

                //Add the deadBody to the client that's carrying it for Movement Sends
                necClient.bodyCollection.Add(client.character.deadBodyInstanceId, client);

                //move the soulState client to the salvagers position
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteUInt32(client.character.instanceId);
                res5.WriteFloat(necClient.character.x);
                res5.WriteFloat(necClient.character.y);
                res5.WriteFloat(necClient.character.z);
                res5.WriteByte(necClient.character.heading); //Heading
                res5.WriteByte(necClient.character.movementAnim); //state
                router.Send(client, (ushort)AreaPacketId.recv_object_point_move_notify, res5, ServerType.Area);
            }



        }


    }



}

