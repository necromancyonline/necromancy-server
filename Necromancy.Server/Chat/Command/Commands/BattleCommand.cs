using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Does Battle Report stuff
    /// </summary>
    public class BattleCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(UnionCommand));

        public BattleCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }
            NecClient _client = client;

            List<PacketResponse> brList = new List<PacketResponse>();
            //always start your battle reports
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            brList.Add(brStart);

            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();



            IBuffer res36 = BufferProvider.Provide();

            switch (command[0])
            {
                case "itemuse":
                    RecvBattleReportActionItemUse itemUse = new RecvBattleReportActionItemUse(50100302/*camp*/);
                    brList.Add(itemUse);
                    break;

                case "stealunidentified":
                    RecvBattleReportActionStealUnidentified stealUnidentified = new RecvBattleReportActionStealUnidentified(50100302/*camp*/);
                    brList.Add(stealUnidentified);
                    break;

                case "stealmoney":
                    RecvBattleReportActionStealMoney stealMoney = new RecvBattleReportActionStealMoney(50100302/*camp*/);
                    brList.Add(stealMoney);
                    break;

                case "97d9":
                    RecvBattleReportNoactNotifyHealAp o97d9 = new RecvBattleReportNoactNotifyHealAp();
                    brList.Add(o97d9);
                    break;

                case "partygetitem":
                    RecvPartyNotifyGetItem recvPartyNotifyGetItem = new RecvPartyNotifyGetItem(client.Character.InstanceId);
                    Router.Send(recvPartyNotifyGetItem, client);
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(200101);
                    res.WriteCString("Dagger");
                    res.WriteByte(20);
                    Router.Send(client.Map, (ushort)MsgPacketId.recv_party_notify_get_item, res, ServerType.Msg);
                    break;

                case "partygetmoney":
                    RecvPartyNotifyGetMoney recvPartyNotifyGetMoney = new RecvPartyNotifyGetMoney(client.Character.InstanceId);
                    Router.Send(client.Map, recvPartyNotifyGetMoney);
                    break;

                case "ac":
                    RecvBattleReportNotifyDamageAc recvBattleReportNotifyDamageAc = new RecvBattleReportNotifyDamageAc(client.Character.InstanceId, Util.GetRandomNumber(0,250));
                    brList.Add(recvBattleReportNotifyDamageAc);
                    break;



                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    Task.Delay(TimeSpan.FromMilliseconds((int) (10 * 1000))).ContinueWith
                    (t1 =>
                        {

                        }
                    );
                    break;
            }
            //always end your battle reports
            //brList.Add(brEnd);
            Router.Send(_client, brList);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "battle";
        public override string HelpText => "usage: `/battle [command]` - Does something battle related.";
    }
}
