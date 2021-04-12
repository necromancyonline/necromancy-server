using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleRequestRevive : ClientHandler
    {
        private List<NecClient> _necClients;

        public SendRaisescaleRequestRevive(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_raisescale_request_revive;

        public override void Handle(NecClient client, NecPacket packet)
        {
            _necClients = client.map.clientLookup.GetAll();
            //if (client.Character.soulFormState == 1)
            {
                client.character.state = CharacterState.InvulnerableForm;
                client.character.hasDied = false;
                client.character.Hp.depleted = false;
                client.character.deadType = 0;
                client.character.Hp.ToMax();


                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(0); //Has to be 0 or else you DC
                res1.WriteUInt32(client.character.deadBodyInstanceId);
                res1.WriteUInt32(client.character.instanceId);
                router.Send(client, (ushort)AreaPacketId.recv_revive_init_r, res1, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.character.deadBodyInstanceId);
                router.Send(client.map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);

                res3 = BufferProvider.Provide();
                res3.WriteUInt32(client.character.instanceId);
                router.Send(client.map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area, client);


                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.character, client.soul.name);
                router.Send(client.map, cData.ToPacket());


                //Disappear .. all the monsters, NPCs, and characters.  welcome to Life! it's less lonely
                foreach (NpcSpawn npcSpawn in client.map.npcSpawns.Values)
                {
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(npcSpawn.instanceId);
                    router.Send(client, recvObjectDisappearNotify.ToPacket());
                }

                foreach (MonsterSpawn monsterSpawn in client.map.monsterSpawns.Values)
                {
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(monsterSpawn.instanceId);
                    router.Send(client, recvObjectDisappearNotify.ToPacket());
                }

                foreach (NecClient client2 in _necClients)
                {
                    if (client2 == client) continue; //Don't dissapear yourself ! that'd be bad news bears.
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client2.character.instanceId);
                    router.Send(client, recvObjectDisappearNotify.ToPacket());
                }

                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.character.instanceId);
                RecvBattleReportNotifyRaise recvBattleReportNotifyRaise = new RecvBattleReportNotifyRaise(client.character.instanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

                brList.Add(brStart);
                brList.Add(recvBattleReportNotifyRaise);
                brList.Add(brEnd);
                router.Send(client.map, brList);

                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                (t1 =>
                    {
                        RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(client.character.Hp.max);
                        router.Send(client, cHpUpdate.ToPacket());

                        //if you are not dead, do normal stuff.  else...  do dead person stuff
                        if (client.character.state != CharacterState.SoulForm)
                        {
                            foreach (NecClient otherClient in _necClients)
                            {
                                if (otherClient == client)
                                    // skip myself
                                    continue;
                                if (otherClient.character.state != CharacterState.SoulForm)
                                {
                                    RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.character, otherClient.soul.name);
                                    router.Send(otherCharacterData, client);
                                }

                                if (otherClient.union != null)
                                {
                                    RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.character, otherClient.union.name);
                                    router.Send(otherUnionData, client);
                                }
                            }

                            foreach (MonsterSpawn monsterSpawn in client.map.monsterSpawns.Values)
                            {
                                RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                                router.Send(monsterData, client);
                            }

                            foreach (NpcSpawn npcSpawn in client.map.npcSpawns.Values)
                                if (npcSpawn.visibility != 2) //2 is the magic number for soul state only.  make it an Enum some day
                                {
                                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                                    router.Send(npcData, client);
                                }

                            foreach (DeadBody deadBody in client.map.deadBodies.Values)
                                if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                                {
                                    RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                                    router.Send(deadBodyData, client);
                                }
                        }
                    }
                );
                Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
                (t1 =>
                    {
                        client.character.ClearStateBit(CharacterState.InvulnerableForm);
                        RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(client.character.instanceId, (ulong)client.character.state);
                        router.Send(client.map, recvCharaNotifyStateflag.ToPacket());
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

            if (client.map.deadBodies.ContainsKey(client.character.deadBodyInstanceId)) client.map.deadBodies.Remove(client.character.deadBodyInstanceId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = sucess to revive, 1 = failed to revive
            router.Send(client, (ushort)AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area); //responsible for camera movement

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
