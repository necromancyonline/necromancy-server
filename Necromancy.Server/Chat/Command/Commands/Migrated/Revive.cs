using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class Revive : ServerChatCommand
    {
        public Revive(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //if (client.Character.soulFormState == 1)
            {
                client.Character.Hp.toMax();
                client.Character.State = CharacterState.InvulnerableForm;
                client.Character.HasDied = false;
                client.Character.deadType = 0;

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(0); // 0 = sucess to revive, 1 = failed to revive
                Router.Send(client, (ushort)AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area); //responsible for camera movement


                RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(client.Character.Hp.current);
                Router.Send(client, cHpUpdate.ToPacket());

                IBuffer res4 = BufferProvider.Provide();
                res4.WriteUInt32(client.Character.InstanceId);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                res5.WriteUInt32(client.Character.InstanceId);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_notify_raise, res5, ServerType.Area);

                IBuffer res6 = BufferProvider.Provide();
                Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_end_notify, res6, ServerType.Area);
                //

                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(0); //Has to be 0 or else you DC
                res1.WriteUInt32(client.Character.DeadBodyInstanceId);
                res1.WriteUInt32(client.Character.InstanceId);
                Router.Send(client, (ushort)AreaPacketId.recv_revive_init_r, res1, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.Character.DeadBodyInstanceId);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);

                res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.Character.InstanceId);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area, client);

                client.Character.HasDied = false;
                client.Character.Hp.depleted = false;
                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                Router.Send(client.Map, cData.ToPacket());

                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(0); // Error code, 0 = success
                Router.Send(client, (ushort)AreaPacketId.recv_revive_execute_r, res2, ServerType.Area);

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
                foreach (NecClient client2 in client.Map.ClientLookup.GetAll())
                {
                    if (client2 == client) continue; //Don't dissapear yourself ! that'd be bad news bears.
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client2.Character.InstanceId);
                    Router.Send(client, recvObjectDisappearNotify.ToPacket());
                }

                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                (t1 =>
                    {
                        RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(client.Character.InstanceId, (uint)CharacterState.NormalForm);
                        //Router.Send(client.Map, recvCharaNotifyStateflag.ToPacket()); //grab structure from xdbg

                        //if you are not dead, do normal stuff.  else...  do dead person stuff
                        if (client.Character.State != Model.CharacterModel.CharacterState.SoulForm)
                        {
                            foreach (NecClient otherClient in client.Map.ClientLookup.GetAll())
                            {
                                if (otherClient == client)
                                {
                                    // skip myself
                                    continue;
                                }
                                if (otherClient.Character.State != Model.CharacterModel.CharacterState.SoulForm)
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
                                if (client.Character.State != Model.CharacterModel.CharacterState.SoulForm)
                                {
                                    RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                                    Router.Send(deadBodyData, client);
                                }
                            }
                        }
                    }
                );
            }

            /*else if (client.Character.soulFormState == 0)
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(client.Character.InstanceId); // ID
                res1.WriteInt32(100101); //100101, its the id to get the tombstone
                Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_stateflag, res1, ServerType.Area);

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

        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "revi";
    }
}
