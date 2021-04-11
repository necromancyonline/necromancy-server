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
    public class send_event_request_int_r : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_event_request_int_r));


        private readonly NecServer _server;

        public send_event_request_int_r(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_request_int_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.Character.currentEvent == null)
            {
                Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with no current event saved.");
                SendEventEnd(client);
                return;
            }

            switch (client.Character.currentEvent)
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
                    int newModelId = packet.Data.ReadInt32();
                    Logger.Debug($"Entered ModelID [{newModelId}]");


                    if (!Server.SettingRepository.ModelCommon.TryGetValue(newModelId,
                        out ModelCommonSetting modelSetting))
                    {
                        IBuffer res12 = BufferProvider.Provide();
                        res12.WriteCString($"Invalid model ID {newModelId}. please try again"); // Length 0xC01
                        Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                            ServerType.Area); // show system message on middle of the screen.
                        DelayedEventEnd(client);
                        client.Character.currentEvent = null;
                        return;
                    }

                    npcModelUpdate.npcSpawn.ModelId = newModelId;
                    npcModelUpdate.npcSpawn.Updated = DateTime.Now;
                    if (!Server.Database.UpdateNpcSpawn(npcModelUpdate.npcSpawn))
                    {
                        IBuffer res12 = BufferProvider.Provide();
                        res12.WriteCString("Could not update the database"); // Length 0xC01
                        Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                            ServerType.Area); // show system message on middle of the screen.
                        DelayedEventEnd(client);
                        client.Character.currentEvent = null;
                        return;
                    }

                    IBuffer res13 = BufferProvider.Provide();
                    res13.WriteCString(
                        $"NPC {npcModelUpdate.npcSpawn.Name} Updated. Model {newModelId}"); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res13,
                        ServerType.Area); // show system message on middle of the screen.

                    DelayedEventEnd(client);
                    client.Character.currentEvent = null;
                    break;

                default:
                    Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with undefined event type.");
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
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void DelayedEventEnd(NecClient client)
        {
            Task.Delay(TimeSpan.FromMilliseconds((int) (2 * 1000))).ContinueWith
            (t1 =>
                {
                    IBuffer res = BufferProvider.Provide();
                    res.WriteByte(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
                }
            );
        }
    }
}
