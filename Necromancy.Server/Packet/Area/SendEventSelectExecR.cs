using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventSelectExecR : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendEventSelectExecR));

        public SendEventSelectExecR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_select_exec_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.character.eventSelectExecCode = packet.data.ReadInt32();
            _Logger.Debug($" The packet contents are :{client.character.eventSelectExecCode}");
            if (client.character.eventSelectExecCode == -1)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteByte(0);
                router.Send(client, (ushort)AreaPacketId.recv_event_end, res2, ServerType.Area);
            }
            else
            {
                //logic to execute different actions based on the event that triggered this select execution.
                IInstance instance = server.instances.GetInstance(client.character.eventSelectReadyCode);

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        client.map.npcSpawns.TryGetValue(npcSpawn.instanceId, out npcSpawn);
                        _Logger.Debug(
                            $"instanceId : {client.character.eventSelectReadyCode} |  npcSpawn.Id: {npcSpawn.id}  |   npcSpawn.NpcId: {npcSpawn.npcId}");

                        Dictionary<Func<int, bool>, Action> eventSwitchPerObjectId = new Dictionary<Func<int, bool>, Action>
                        {
                            {
                                x => x == 10000704, () => ChangeMap(client, npcSpawn.npcId)
                            }, //set to Manaphes in slums for testing.
                            {x => x == 10000012, () => DefaultEvent(client, npcSpawn.npcId)},
                            {x => x == 10000019, () => Abdul(client, npcSpawn)},
                            {
                                x => x == 74000022 || x == 74000024 || x == 74000023,
                                () => RecoverySpring(client, npcSpawn.npcId)
                            },
                            {x => x == 74013071, () => ChangeMap(client, npcSpawn.npcId)},
                            {x => x == 74013161, () => ChangeMap(client, npcSpawn.npcId)},
                            {x => x == 74013271, () => ChangeMap(client, npcSpawn.npcId)},
                            {x => x == 10000912, () => ChangeMap(client, npcSpawn.npcId)},
                            {x => x == 10000002, () => RegularInn(client, npcSpawn.npcId, npcSpawn)},
                            {x => x == 10000703, () => CrimInn(client, npcSpawn.npcId, npcSpawn)},
                            {x => x == 10000004, () => SoulRankNpc(client, npcSpawn.npcId, npcSpawn)},
                            {
                                x => x == 1900002 || x == 1900003,
                                () => RandomItemGuy(client, npcSpawn)
                            },
                            {
                                x => x < 10,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {
                                x => x < 100,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {
                                x => x < 1000,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {
                                x => x < 10000,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {
                                x => x < 100000,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {
                                x => x < 1000000,
                                () => _Logger.Debug($" Event Object switch for NPC ID {npcSpawn.npcId} reached")
                            },
                            {x => x < 900000100, () => UpdateNpc(client, npcSpawn)}
                        };

                        eventSwitchPerObjectId.First(sw => sw.Key(npcSpawn.npcId)).Value();


                        break;
                    case MonsterSpawn monsterSpawn:
                        _Logger.Debug($"MonsterId: {monsterSpawn.id}");
                        break;

                    case GGateSpawn ggateSpawn:
                        client.map.gGateSpawns.TryGetValue(ggateSpawn.instanceId, out ggateSpawn);
                        _Logger.Debug(
                            $"instanceId : {client.character.eventSelectReadyCode} |  ggateSpawn.Id: {ggateSpawn.id}  |   ggateSpawn.NpcId: {ggateSpawn.serialId}");

                        Dictionary<Func<int, bool>, Action> eventSwitchPerObjectId2 = new Dictionary<Func<int, bool>, Action>
                        {
                            {x => x == 74013071, () => ChangeMap(client, ggateSpawn.serialId)},
                            {x => x == 74013161, () => ChangeMap(client, ggateSpawn.serialId)},
                            {x => x == 74013271, () => ChangeMap(client, ggateSpawn.serialId)},
                            {x => x == 7500001, () => ModelLibraryWarp(client, ggateSpawn)},

                            {x => x < 900000100, () => _Logger.Debug("Yea, Work in progress still.")}
                        };

                        eventSwitchPerObjectId2.First(sw => sw.Key(ggateSpawn.serialId)).Value();


                        break;
                    default:
                        _Logger.Error(
                            $"Instance with InstanceId: {client.character.eventSelectReadyCode} does not exist");
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void RecvEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_end, res, ServerType.Area);
            Task.Delay(TimeSpan.FromMilliseconds(2 * 1000)).ContinueWith
            (t1 =>
                {
                    IBuffer res = BufferProvider.Provide();
                    res.WriteByte(0);
                    router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
                }
            );
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void RecoverySpring(NecClient client, int objectId)
        {
            if (client.character.eventSelectExecCode == 0)
            {
                if (client.character.hp.current == client.character.hp.max &&
                    client.character.mp.current == client.character.mp.max)
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString(
                        "You try drinking the water but it doesn't seem to have an effect."); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                }
                else
                {
                    RecvEventScriptPlay recvEventScriptPlay = new RecvEventScriptPlay("etc/heal_fountain", client.character.instanceId);
                    router.Send(recvEventScriptPlay, client);

                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You drink The water and it replenishes you"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.

                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32(client.character.hp.max); //To-Do : Math for Max gain of 50% MaxHp
                    router.Send(client, (ushort)AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);
                    client.character.hp.ToMax();

                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(client.character.mp.max); //To-Do : Math for Max gain of 50% MaxMp
                    router.Send(client, (ushort)AreaPacketId.recv_chara_update_mp, res9, ServerType.Area);
                    client.character.mp.SetCurrent(client.character.mp.max);
                }
            }
            else if (client.character.eventSelectExecCode == 1)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You Say no to random Dungeun water"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }

            IBuffer res13 = BufferProvider.Provide();
            res13.WriteCString("To raise your level, you need 1337 more exp."); // Length 0xC01
            router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13,
                ServerType.Area); // show system message on middle of the screen.

            RecvEventEnd(client); //End The Event
        }


        private void Abdul(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.character.eventSelectExecCode == 0)
            {
                if (client.character.hp.current == client.character.hp.max &&
                    client.character.mp.current == client.character.mp.max)
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("What do you want Adul to say?"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                }
                else
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You drink The water and it replenishes you"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.

                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32(client.character.hp.max); //To-Do : Math for Max gain of 50% MaxHp
                    router.Send(client, (ushort)AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);
                    client.character.hp.ToMax();

                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(client.character.mp.max); //To-Do : Math for Max gain of 50% MaxMp
                    router.Send(client, (ushort)AreaPacketId.recv_chara_update_mp, res9, ServerType.Area);
                    client.character.mp.ToMax();
                }
            }
            else if (client.character.eventSelectExecCode == 1)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You hate Abdul,  He's messed up"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }
            else if (client.character.eventSelectExecCode == 2)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You Stoll hate Abdul,  He's messed up"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }


            RecvEventEnd(client); //End The Event
        }


        private void ChangeMap(NecClient client, int objectId)
        {
            Map map = null;
            switch (objectId)
            {
                case 74013071:
                    _Logger.Debug($"objectId[{objectId}] selected {client.character.eventSelectExecCode}");
                    if (client.character.eventSelectExecCode == 0)
                        map = server.maps.Get(2002105);
                    else if (client.character.eventSelectExecCode == 1) map = server.maps.Get(2002106);

                    break;
                case 74013161:
                    _Logger.Debug($"objectId[{objectId}] selected {client.character.eventSelectExecCode}");
                    if (client.character.eventSelectExecCode == 0)
                        map = server.maps.Get(2002104);
                    else if (client.character.eventSelectExecCode == 1) map = server.maps.Get(2002106);

                    break;
                case 74013271:
                    _Logger.Debug($"objectId[{objectId}] selected {client.character.eventSelectExecCode}");
                    if (client.character.eventSelectExecCode == 0)
                        map = server.maps.Get(2002104);
                    else if (client.character.eventSelectExecCode == 1) map = server.maps.Get(2002105);

                    break;
                default:
                    return;
            }

            map.EnterForce(client);
            SendEventEnd(client);
        }

        private void DefaultEvent(NecClient client, int objectId)
        {
            SendEventEnd(client);
        }

        private void UpdateNpc(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.character.eventSelectExecCode == 0)
            {
                npcSpawn.heading = (byte)(client.character.heading + 90);
                npcSpawn.heading = (byte)(npcSpawn.heading % 180);
                if (npcSpawn.heading < 0) npcSpawn.heading += 180;

                npcSpawn.updated = DateTime.Now;


                if (!server.database.UpdateNpcSpawn(npcSpawn))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("Could not update the database"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                    return;
                }

                IBuffer res13 = BufferProvider.Provide();
                res13.WriteCString("NPC Updated"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13,
                    ServerType.Area); // show system message on middle of the screen.

                RecvEventEnd(client); //End The Event
            }
            else if (client.character.eventSelectExecCode == 1)
            {
                NpcModelUpdate npcModelUpdate = new NpcModelUpdate();
                server.instances.AssignInstance(npcModelUpdate);
                npcModelUpdate.npcSpawn = npcSpawn;

                client.character.currentEvent = npcModelUpdate;

                IBuffer res14 = BufferProvider.Provide();
                RecvEventRequestInt getModelId = new RecvEventRequestInt("Select Model ID from Model_common.csv", 11000,
                    1911105, npcSpawn.modelId);
                router.Send(getModelId, client);
            }
        }

        private void RegularInn(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            if (client.character.secondInnAccess)
            {
                ResolveInn(client, npcSpawn.npcId, npcSpawn);
            }
            else
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7, ServerType.Area); //

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8, ServerType.Area); //

                client.character.secondInnAccess = true;
                IBuffer res9 = BufferProvider.Provide();

                switch (client.character.eventSelectExecCode)
                {
                    case 0:
                        if (client.soul.level > 3)
                        {
                            res9.WriteCString("Our sincerest apologies. That's only for new souls."); // Length 0xC01
                            router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res9, ServerType.Area); // show system message on middle of the screen.
                            SendEventEnd(client);
                            client.character.eventSelectExtraSelectionCode = 0;
                            client.character.eventSelectExecCode = 0;
                            client.character.eventSelectReadyCode = 0;
                            client.character.secondInnAccess = false;
                            break;
                        }
                        else
                        {
                            res9.WriteCString("Effect: Recover all HP, all MP, and 150 Condition"); //
                            res9.WriteUInt32(client.character.instanceId);
                            router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                            client.character.eventSelectExtraSelectionCode = 0;
                            break;
                        }
                    case 1:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 50 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 1;
                        break;
                    case 2:
                        res9.WriteCString("Effect: Recover half HP, half MP, and 100 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 2;
                        break;
                    case 3:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 110 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 3;
                        break;
                    case 4:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 120 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 4;
                        break;
                    case 5:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 160 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 5;
                        break;
                    case 6:
                        client.character.secondInnAccess = false;
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void CrimInn(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            if (client.character.secondInnAccess)
            {
                ResolveInn(client, npcSpawn.npcId, npcSpawn);
            }
            else
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res7,
                    ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res8,
                    ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                client.character.secondInnAccess = true;

                switch (client.character.eventSelectExecCode)
                {
                    case 0:
                        if (client.soul.level > 3)
                        {
                            res9 = BufferProvider.Provide();
                            res9.WriteCString("Sorry big guy. That's only for new souls."); // Length 0xC01
                            router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res9, ServerType.Area); // show system message on middle of the screen.
                            SendEventEnd(client);
                            client.character.eventSelectExtraSelectionCode = 0;
                            client.character.eventSelectExecCode = 0;
                            client.character.eventSelectReadyCode = 0;
                            client.character.secondInnAccess = false;
                            break;
                        }
                        else
                        {
                            res9.WriteCString("Effect: Recover full HP, full MP, and 150 Condition"); //
                            res9.WriteUInt32(client.character.instanceId);
                            router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9,
                                ServerType.Area); //
                            client.character.eventSelectExtraSelectionCode = 6;
                            break;
                        }
                    case 1:
                        res9.WriteCString("Effect: Recover half HP, half MP, and 50 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 7;
                        break;
                    case 2:
                        res9.WriteCString("Effect: Recover 80% HP, 80% MP, and 80 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 8;
                        break;
                    case 3:
                        res9.WriteCString("Effect: Recover full HP, full MP, and 100 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 9;
                        break;
                    case 4:
                        res9.WriteCString("Effect: Recover full HP, full MP, and 120 Condition"); //
                        res9.WriteUInt32(client.character.instanceId);
                        router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.character.eventSelectExtraSelectionCode = 10;
                        break;
                    case 5:
                        client.character.secondInnAccess = false;
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void ResolveInn(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            if (client.character.eventSelectExecCode == 0)
            {
                int[] hPandMPperChoice = { 100, 50, 100, 100, 100, 100, 100, 50, 80, 100, 100 };
                byte[] conditionPerChoice = { 150, 50, 100, 110, 120, 160, 150, 50, 80, 100, 120 };
                ulong[] goldCostPerChoice = { 0, 0, 60, 300, 1200, 3000, 100, 0, 60, 300, 10000 };
                _Logger.Debug($"The selection you have made is {client.character.eventSelectExtraSelectionCode}");

                client.character.hp.SetCurrent((sbyte)hPandMPperChoice[client.character.eventSelectExtraSelectionCode], true);
                client.character.mp.SetCurrent((sbyte)hPandMPperChoice[client.character.eventSelectExtraSelectionCode], true);
                client.character.condition.SetCurrent(conditionPerChoice[client.character.eventSelectExtraSelectionCode]);
                client.character.od.ToMax();
                client.character.gp.ToMax();
                client.character.adventureBagGold -= goldCostPerChoice[client.character.eventSelectExtraSelectionCode];
                if (client.character.hp.current >= client.character.hp.max) client.character.hp.ToMax();
                if (client.character.mp.current >= client.character.mp.current) client.character.mp.ToMax();

                RecvCharaUpdateHp recvCharaUpdateHp = new RecvCharaUpdateHp(client.character.hp.current);
                router.Send(recvCharaUpdateHp, client);
                RecvCharaUpdateMp recvCharaUpdateMp = new RecvCharaUpdateMp(client.character.mp.current);
                router.Send(recvCharaUpdateMp, client);
                RecvCharaUpdateCon recvCharaUpdateCon = new RecvCharaUpdateCon(conditionPerChoice[client.character.eventSelectExtraSelectionCode]);
                router.Send(recvCharaUpdateCon, client);
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, client.character.adventureBagGold);
                router.Send(recvSelfMoneyNotify, client);
                RecvEventScriptPlay recvEventScriptPlay = new RecvEventScriptPlay("inn/fade_bgm", client.character.instanceId);
                router.Send(recvEventScriptPlay, client);
                Experience experience = new Experience();

                //Level up stuff after inn cutscene
                Task.Delay(TimeSpan.FromSeconds(6)).ContinueWith
                (t1 =>
                    {
                        if (client.character.experienceCurrent > experience.CalculateLevelUp((uint)client.character.level + 1).cumulativeExperience)
                        {
                            RecvEventStart recvEventStart = new RecvEventStart(0, 0);
                            router.Send(recvEventStart, client);

                            LevelUpCheck(client);

                            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
                            (t1 =>
                                {
                                    RecvEventEnd recvEventEnd = new RecvEventEnd(0);
                                    router.Send(recvEventEnd, client); //Need a better way to end the event at the right time.
                                }
                            );
                        }
                    }
                );
            }
            else
            {
                SendEventEnd(client);
            }

            client.character.eventSelectExtraSelectionCode = 0;
            client.character.eventSelectExecCode = 0;
            client.character.eventSelectReadyCode = 0;
            client.character.secondInnAccess = false;
        }

        private void LevelUpCheck(NecClient client)
        {
            Experience experience = new Experience();
            Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith
            (t1 =>
                {
                    while (client.character.experienceCurrent > experience.CalculateLevelUp((uint)client.character.level + 1).cumulativeExperience)
                    {
                        client.character.level++;
                        client.character.hp.SetMax(client.character.hp.max + 10);
                        client.character.mp.SetMax(client.character.mp.max + 10);
                        client.character.strength += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.vitality += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.dexterity += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.agility += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.intelligence += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.piety += (ushort)Util.GetRandomNumber(0, 2);
                        client.character.luck += (ushort)Util.GetRandomNumber(0, 2);
                        int luckyShot = Util.GetRandomNumber(0, client.character.luck);
                        if (luckyShot > client.character.luck * .8)
                        {
                            client.character.hp.SetMax(client.character.hp.max + 10);
                            client.character.mp.SetMax(client.character.mp.max + 10);
                            client.character.strength = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.strength);
                            client.character.vitality = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.vitality);
                            client.character.dexterity = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.dexterity);
                            client.character.agility = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.agility);
                            client.character.intelligence = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.intelligence);
                            client.character.piety = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.piety);
                            client.character.luck = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.luck);
                        }

                        RecvCharaUpdateLvDetailStart recvCharaUpdateLvDetailStart = new RecvCharaUpdateLvDetailStart();
                        RecvCharaUpdateLv recvCharaUpdateLv = new RecvCharaUpdateLv(client.character);
                        RecvCharaUpdateLvDetail recvCharaUpdateLvDetail = new RecvCharaUpdateLvDetail(client.character, experience);
                        RecvCharaUpdateLvDetail2 recvCharaUpdateLvDetail2 = new RecvCharaUpdateLvDetail2(client.character, experience);
                        RecvCharaUpdateLvDetailEnd recvCharaUpdateLvDetailEnd = new RecvCharaUpdateLvDetailEnd();

                        RecvCharaUpdateMaxHp recvCharaUpdateMaxHp = new RecvCharaUpdateMaxHp(client.character.hp.max);
                        RecvCharaUpdateMaxMp recvCharaUpdateMaxMp = new RecvCharaUpdateMaxMp(client.character.mp.max);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityStr = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Str, client.character.strength, client.character.battleParam.plusStrength);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityVit = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Vit, client.character.vitality, client.character.battleParam.plusVitality);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityDex = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Dex, client.character.dexterity, client.character.battleParam.plusDexterity);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityAgi = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Agi, client.character.agility, client.character.battleParam.plusAgility);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityInt = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Int, client.character.intelligence, client.character.battleParam.plusIntelligence);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityPie = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Pie, client.character.piety, client.character.battleParam.plusPiety);
                        RecvCharaUpdateAbility recvCharaUpdateAbilityLuk = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Luk, client.character.luck, client.character.battleParam.plusLuck);

                        router.Send(recvCharaUpdateLvDetailStart, client);


                        router.Send(recvCharaUpdateMaxHp, client);
                        router.Send(recvCharaUpdateMaxMp, client);
                        router.Send(recvCharaUpdateAbilityStr, client);
                        router.Send(recvCharaUpdateAbilityVit, client);
                        router.Send(recvCharaUpdateAbilityDex, client);
                        router.Send(recvCharaUpdateAbilityAgi, client);
                        router.Send(recvCharaUpdateAbilityInt, client);
                        router.Send(recvCharaUpdateAbilityPie, client);
                        router.Send(recvCharaUpdateAbilityLuk, client);

                        router.Send(recvCharaUpdateLv, client);
                        router.Send(recvCharaUpdateLvDetail, client);
                        router.Send(recvCharaUpdateLvDetail2, client);
                        router.Send(recvCharaUpdateLvDetailEnd, client);
                    }
                }
            );
        }

        private void SoulRankNpc(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            switch (client.character.eventSelectExecCode)
            {
                case 0:
                    SendEventEnd(client);
                    break;
                case 1:
                    SendEventEnd(client);
                    break;
                case 2:
                    SendEventEnd(client);
                    break;
                case 3:
                    SendEventEnd(client);
                    break;
                case 4:
                    SendEventEnd(client);
                    break;
                case 5:
                    SendEventEnd(client);
                    break;
            }
        }

        private void RandomItemGuy(NecClient client, NpcSpawn npcSpawn)
        {
            ItemSpawnParams spawmParam = new ItemSpawnParams();
            spawmParam.itemStatuses = ItemStatuses.Unidentified;
            ItemService itemService = new ItemService(client.character);
            ItemInstance itemInstance = null;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            ItemLocation nextOpenLocation = client.character.itemLocationVerifier.NextOpenSlot(ItemZoneType.AdventureBag);

            if (nextOpenLocation.zoneType == ItemZoneType.InvalidZone)
            {
                res = BufferProvider.Provide();
                res.WriteCString($"Your Adventure Bag is full.  Go away already! {client.soul.name}"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.
            }
            else
            {
                if (client.character.eventSelectExecCode == 0)
                {
                    List<ItemInfoSetting> weaponlist = new List<ItemInfoSetting>();
                    foreach (ItemInfoSetting weapon in server.settingRepository.itemInfo.Values)
                        if ((weapon.id > 10100101) & (weapon.id < 15300101))
                            weaponlist.Add(weapon);

                    int baseId = weaponlist[Util.GetRandomNumber(0, weaponlist.Count)].id;
                    itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, baseId, spawmParam);

                    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                    router.Send(client, recvItemInstanceUnidentified.ToPacket());
                }
                else if (client.character.eventSelectExecCode == 1)
                {
                    List<ItemInfoSetting> armorList = new List<ItemInfoSetting>();
                    foreach (ItemInfoSetting armor in server.settingRepository.itemInfo.Values)
                        if ((armor.id > 16100101) & (armor.id < 30499901))
                            armorList.Add(armor);

                    int baseId = armorList[Util.GetRandomNumber(0, armorList.Count)].id;
                    itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, baseId, spawmParam);

                    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                    router.Send(client, recvItemInstanceUnidentified.ToPacket());
                }
                else if (client.character.eventSelectExecCode == 2)
                {
                    //50401040,Moist Cudgel
                    int baseId = 50401040; //This can select from a small array of items, and a small array of custom names
                    spawmParam.itemStatuses = ItemStatuses.Identified;
                    itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, baseId, spawmParam);

                    RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                    router.Send(client, recvItemInstance.ToPacket());
                }

                if (itemInstance == null)
                {
                    res = BufferProvider.Provide();
                    res.WriteCString("Better Luck Next Time.  I ran out of items!"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.
                    res = BufferProvider.Provide();
                    router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
                    RecvEventEnd(client); //End The Event
                    return;
                }

                res = BufferProvider.Provide();
                res.WriteCString($"Enjoy your new Super {itemInstance.unidentifiedName}"); // Length 0xC01
                router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.
            }

            res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
            RecvEventEnd(client); //End The Event
        }

        private void ModelLibraryWarp(NecClient client, GGateSpawn ggateSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            switch (client.character.eventSelectExecCode)
            {
                case 0:
                    res = BufferProvider.Provide();
                    res.WriteCString("Seriously?! Walk across the bridge. Why so Lazy?"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.

                    break;
                case 1:
                    res = BufferProvider.Provide();
                    RecvEventScriptPlay recvEventScriptPlay = new RecvEventScriptPlay("etc/warp_samemap", client.character.instanceId);
                    router.Send(recvEventScriptPlay, client);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.character.instanceId);
                            res.WriteFloat(1574);
                            res.WriteFloat(26452);
                            res.WriteFloat(-1145);
                            res.WriteByte(client.character.heading);
                            res.WriteByte(client.character.movementAnim);
                            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 2:
                    res = BufferProvider.Provide();
                    RecvEventScriptPlay recvEventScriptPlay2 = new RecvEventScriptPlay("etc/warp_samemap", client.character.instanceId);
                    router.Send(recvEventScriptPlay2, client);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.character.instanceId);
                            res.WriteFloat(21);
                            res.WriteFloat(39157);
                            res.WriteFloat(-1838);
                            res.WriteByte(client.character.heading);
                            res.WriteByte(client.character.movementAnim);
                            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 3:
                    res = BufferProvider.Provide();
                    RecvEventScriptPlay recvEventScriptPlay3 = new RecvEventScriptPlay("etc/warp_samemap", client.character.instanceId);
                    router.Send(recvEventScriptPlay3, client);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.character.instanceId);
                            res.WriteFloat(0);
                            res.WriteFloat(47829);
                            res.WriteFloat(-2538);
                            res.WriteByte(client.character.heading);
                            res.WriteByte(client.character.movementAnim);
                            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 4:
                    res = BufferProvider.Provide();
                    res.WriteCString("Turn around genius"); // Length 0xC01
                    router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.
                    break;
            }

            RecvEventEnd(client); //End The Event
        }
    }
}
