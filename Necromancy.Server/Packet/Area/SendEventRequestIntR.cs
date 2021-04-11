using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventRequestIntR : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendEventRequestIntR));


        private readonly NecServer _server;

        public SendEventRequestIntR(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort id => (ushort) AreaPacketId.send_event_request_int_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.character.currentEvent == null)
            {
                _Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with no current event saved.");
                SendEventEnd(client);
                return;
            }

            switch (client.character.currentEvent)
            {
         //       case MoveItem moveItem:
         //           IBuffer res = BufferProvider.Provide();
         //           int count = packet.Data.ReadInt32();
         //           Logger.Debug($"Returned [{count}]");
         //           SendEventEnd(client);
         //           MoveItem(client, moveItem, count);
         //           client.Character.currentEvent = null;
         //           break;
                case NpcModelUpdate npcModelUpdate:
                    int newModelId = packet.data.ReadInt32();
                    _Logger.Debug($"Entered ModelID [{newModelId}]");


                    if (!server.settingRepository.modelCommon.TryGetValue(newModelId,
                        out ModelCommonSetting modelSetting))
                    {
                        IBuffer res12 = BufferProvider.Provide();
                        res12.WriteCString($"Invalid model ID {newModelId}. please try again"); // Length 0xC01
                        router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                            ServerType.Area); // show system message on middle of the screen.
                        DelayedEventEnd(client);
                        client.character.currentEvent = null;
                        return;
                    }

                    npcModelUpdate.npcSpawn.modelId = newModelId;
                    npcModelUpdate.npcSpawn.updated = DateTime.Now;
                    if (!server.database.UpdateNpcSpawn(npcModelUpdate.npcSpawn))
                    {
                        IBuffer res12 = BufferProvider.Provide();
                        res12.WriteCString("Could not update the database"); // Length 0xC01
                        router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                            ServerType.Area); // show system message on middle of the screen.
                        DelayedEventEnd(client);
                        client.character.currentEvent = null;
                        return;
                    }

                    IBuffer res13 = BufferProvider.Provide();
                    res13.WriteCString(
                        $"NPC {npcModelUpdate.npcSpawn.name} Updated. Model {newModelId}"); // Length 0xC01
                    router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res13,
                        ServerType.Area); // show system message on middle of the screen.

                    DelayedEventEnd(client);
                    client.character.currentEvent = null;
                    break;

                default:
                    _Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with undefined event type.");
                    break;
            }
        }

    //   private void MoveItem(NecClient client, MoveItem moveItem, int count)
    //   {
    //       if (count <= 0)
    //       {
    //           return;
    //       }

    //       moveItem.itemCount = (byte) count;
    //       moveItem.Move(_server, client);
    //   }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void DelayedEventEnd(NecClient client)
        {
            Task.Delay(TimeSpan.FromMilliseconds((int) (2 * 1000))).ContinueWith
            (t1 =>
                {
                    IBuffer res = BufferProvider.Provide();
                    res.WriteByte(0);
                    router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
                }
            );
        }
    }
}
