using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    //displays message that you died
    public class Died : ServerChatCommand
    {
        public Died(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.Character.HasDied == true)
            {
                IBuffer res4 = BufferProvider.Provide();
                Router.Send(client.Map, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
            }

            if (client.Character.HasDied == false)
            {
                client.Character.HasDied =
                    true; // setting before the Sleep so other monsters can't "kill you" while you're dieing
                client.Character.Hp.Modify(-client.Character.Hp.current);
                client.Character.State = CharacterState.SoulForm;
                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
                RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(client.Character.InstanceId, 1);
                RecvBattleReportNoactDead cDead2 = new RecvBattleReportNoactDead(client.Character.InstanceId, 2);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

                brList.Add(brStart);
                brList.Add(cDead1); //animate the death of your living body
                brList.Add(brEnd);
                Router.Send(client.Map, brList, client); // send death animation to other players


                brList[1] = cDead2;
                Router.Send(client, brList); // send death animaton to player 1

                DeadBody deadBody =
                    Server.Instances.GetInstance((uint) client.Character.DeadBodyInstanceId) as DeadBody;

                deadBody.X = client.Character.X;
                deadBody.Y = client.Character.Y;
                deadBody.Z = client.Character.Z;
                deadBody.Heading = client.Character.Heading;
                client.Character.movementId = client.Character.DeadBodyInstanceId;

                Task.Delay(TimeSpan.FromMilliseconds((int) (5 * 1000))).ContinueWith
                (t1 =>
                    {
                        client.Character.State = CharacterState.SoulForm;
                        //load your dead body on the map for looting
                        RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody, client.Character, client);
                        Server.Router.Send(client, cBodyData.ToPacket());

                    }
                );

                Task.Delay(TimeSpan.FromMilliseconds((int) (15 * 1000))).ContinueWith
                (t1 =>
                    {
                        //load your soul so you can run around and do soul stuff
                        RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                        Server.Router.Send(client, cData.ToPacket());
                    }
                );
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "Died";
    }
}
