using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Receive.Area;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive : ClientHandler
    {
        private List<NecClient> _necClients;
        public send_raisescale_request_revive(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive;

        public override void Handle(NecClient client, NecPacket packet)
        {
            _necClients = client.Map.ClientLookup.GetAll();
            //if (client.Character.soulFormState == 1)
            {
                client.Character.State = CharacterState.InvulnerableForm;
                client.Character.HasDied = false;
                client.Character.Hp.depleted = false;
                client.Character.deadType = 0;
                client.Character.Hp.toMax();


                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(0); //Has to be 0 or else you DC
                res1.WriteUInt32(client.Character.DeadBodyInstanceId);
                res1.WriteUInt32(client.Character.InstanceId);
                Router.Send(client, (ushort)AreaPacketId.recv_revive_init_r, res1, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.Character.DeadBodyInstanceId);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);

                res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.Character.InstanceId);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area, client);


                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                Router.Send(client.Map, cData.ToPacket());


                //Disappear .. all the monsters, NPCs, and characters.  welcome to Life! it's less lonely
                foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
                {
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(npcSpawn.InstanceId);
                    Router.Send(client, recvObjectDisappearNotify.ToPacket());
                }
                foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
                {
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(monsterSpawn.InstanceId);
                    Router.Send(client, recvObjectDisappearNotify.ToPacket());
                }
                foreach (NecClient client2 in _necClients)
                {
                    if (client2 == client) continue; //Don't dissapear yourself ! that'd be bad news bears.
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client2.Character.InstanceId);
                    Router.Send(client, recvObjectDisappearNotify.ToPacket());
                }

                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
                RecvBattleReportNotifyRaise recvBattleReportNotifyRaise = new RecvBattleReportNotifyRaise(client.Character.InstanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

                brList.Add(brStart);
                brList.Add(recvBattleReportNotifyRaise); 
                brList.Add(brEnd);
                Router.Send(client.Map, brList); 

                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                (t1 =>
                {
                    RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(client.Character.Hp.max);
                    Router.Send(client, cHpUpdate.ToPacket());

                    //if you are not dead, do normal stuff.  else...  do dead person stuff
                    if (client.Character.State != CharacterState.SoulForm)
                    {
                        foreach (NecClient otherClient in _necClients)
                        {
                            if (otherClient == client)
                            {
                                // skip myself
                                continue;
                            }
                            if (otherClient.Character.State != CharacterState.SoulForm)
                            {
                                RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                                Router.Send(otherCharacterData, client);
                            }
                            if (otherClient.Union != null)
                            {
                                RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                                Router.Send(otherUnionData, client);
                            }
                        }

                        foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
                        {
                            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                            Router.Send(monsterData, client);
                        }

                        foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
                        {
                            if (npcSpawn.Visibility != 2) //2 is the magic number for soul state only.  make it an Enum some day
                            {
                                RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                                Router.Send(npcData, client);
                            }
                        }

                        foreach (DeadBody deadBody in client.Map.DeadBodies.Values)
                        {
                            if (client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                            {
                                RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                                Router.Send(deadBodyData, client);
                            }
                        }
                    }
                }
                );
                Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
                (t1 =>
                {
                    client.Character.ClearStateBit(CharacterState.InvulnerableForm);
                    RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(client.Character.InstanceId, (ulong)client.Character.State);
                    Router.Send(client.Map, recvCharaNotifyStateflag.ToPacket()); 
                }
                );
            }

            /*else if (client.Character.soulFormState == 0)
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(client.Character.InstanceId); // ID
                res1.WriteInt32(100101); //100101, its the id to get the tombstone
                Router.Send(client, (ushort) AreaPacketId.recv_chara_notify_stateflag, res1, ServerType.Area);

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(1); // 0 = sucess to revive, 1 = failed to revive
                Router.Send(client, (ushort) AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                Router.Send(client, (ushort) AreaPacketId.recv_self_lost_notify, res5, ServerType.Area);
            }*/

            if (client.Map.DeadBodies.ContainsKey(client.Character.DeadBodyInstanceId))
            {
                client.Map.DeadBodies.Remove(client.Character.DeadBodyInstanceId);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = sucess to revive, 1 = failed to revive
            Router.Send(client, (ushort)AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area); //responsible for camera movement

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
