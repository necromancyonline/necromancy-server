using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_exec_r : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_event_select_exec_r));

        public send_event_select_exec_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_select_exec_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.eventSelectExecCode = packet.Data.ReadInt32();
            Logger.Debug($" The packet contents are :{client.Character.eventSelectExecCode}");
            if (client.Character.eventSelectExecCode == -1)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteByte(0);
                Router.Send(client, (ushort) AreaPacketId.recv_event_end, res2, ServerType.Area);
            }
            else
            {
                //logic to execute different actions based on the event that triggered this select execution.
                IInstance instance = Server.Instances.GetInstance(client.Character.eventSelectReadyCode);

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                        Logger.Debug(
                            $"instanceId : {client.Character.eventSelectReadyCode} |  npcSpawn.Id: {npcSpawn.Id}  |   npcSpawn.NpcId: {npcSpawn.NpcId}");

                        var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
                        {
                            {
                                x => x == 10000704, () => ChangeMap(client, npcSpawn.NpcId)
                            }, //set to Manaphes in slums for testing.
                            {x => x == 10000012, () => defaultEvent(client, npcSpawn.NpcId)},
                            {x => x == 10000019, () => Abdul(client, npcSpawn)},
                            {
                                x => (x == 74000022) || (x == 74000024) || (x == 74000023),
                                () => RecoverySpring(client, npcSpawn.NpcId)
                            },
                            {x => x == 74013071, () => ChangeMap(client, npcSpawn.NpcId)},
                            {x => x == 74013161, () => ChangeMap(client, npcSpawn.NpcId)},
                            {x => x == 74013271, () => ChangeMap(client, npcSpawn.NpcId)},
                            {x => x == 10000912, () => ChangeMap(client, npcSpawn.NpcId)},
                            {x => x == 10000002, () => RegularInn(client, npcSpawn.NpcId, npcSpawn)},
                            {x => x == 10000703, () => CrimInn(client, npcSpawn.NpcId, npcSpawn)},
                            { x => x == 10000004 ,  () => SoulRankNPC(client, npcSpawn.NpcId, npcSpawn)},
                            {
                                x => (x == 1900002) || (x == 1900003),
                                () => RandomItemGuy(client, npcSpawn)
                            },
                            {
                                x => x < 10,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {
                                x => x < 100,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {
                                x => x < 1000,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {
                                x => x < 10000,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {
                                x => x < 100000,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {
                                x => x < 1000000,
                                () => Logger.Debug($" Event Object switch for NPC ID {npcSpawn.NpcId} reached")
                            },
                            {x => x < 900000100, () => UpdateNPC(client, npcSpawn)}
                        };

                        eventSwitchPerObjectID.First(sw => sw.Key(npcSpawn.NpcId)).Value();


                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id}");
                        break;

                    case GGateSpawn ggateSpawn:
                        client.Map.GGateSpawns.TryGetValue(ggateSpawn.InstanceId, out ggateSpawn);
                        Logger.Debug(
                            $"instanceId : {client.Character.eventSelectReadyCode} |  ggateSpawn.Id: {ggateSpawn.Id}  |   ggateSpawn.NpcId: {ggateSpawn.SerialId}");

                        var eventSwitchPerObjectID2 = new Dictionary<Func<int, bool>, Action>
                        {
                            {x => x == 74013071, () => ChangeMap(client, ggateSpawn.SerialId)},
                            {x => x == 74013161, () => ChangeMap(client, ggateSpawn.SerialId)},
                            {x => x == 74013271, () => ChangeMap(client, ggateSpawn.SerialId)},
                            {x => x == 7500001, () => ModelLibraryWarp(client, ggateSpawn)},

                            {x => x < 900000100, () => Logger.Debug("Yea, Work in progress still.")}
                        };

                        eventSwitchPerObjectID2.First(sw => sw.Key(ggateSpawn.SerialId)).Value();


                        break;
                    default:
                        Logger.Error(
                            $"Instance with InstanceId: {client.Character.eventSelectReadyCode} does not exist");
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void RecvEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_end, res, ServerType.Area);
            Task.Delay(TimeSpan.FromMilliseconds((int) (2 * 1000))).ContinueWith
            (t1 =>
                {
                    IBuffer res = BufferProvider.Provide();
                    res.WriteByte(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
                }
            );
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        private void RecoverySpring(NecClient client, int objectID)
        {
            if (client.Character.eventSelectExecCode == 0)
            {
                if ((client.Character.Hp.current == client.Character.Hp.max) &&
                    (client.Character.Mp.current == client.Character.Mp.max))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString(
                        "You try drinking the water but it doesn't seem to have an effect."); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                }
                else
                {
                    IBuffer res22 = BufferProvider.Provide();
                    res22.WriteCString("etc/heal_fountain"); // find max size 
                    Router.Send(client, (ushort) AreaPacketId.recv_event_script_play, res22, ServerType.Area);

                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You drink The water and it replenishes you"); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.

                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32((client.Character.Hp.max)); //To-Do : Math for Max gain of 50% MaxHp
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);
                    client.Character.Hp.toMax();

                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(client.Character.Mp.max); //To-Do : Math for Max gain of 50% MaxMp
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_mp, res9, ServerType.Area);
                    client.Character.Mp.setCurrent(client.Character.Mp.max);
                }
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You Say no to random Dungeun water"); // Length 0xC01
                Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }

            IBuffer res13 = BufferProvider.Provide();
            res13.WriteCString("To raise your level, you need 1337 more exp."); // Length 0xC01
            Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res13,
                ServerType.Area); // show system message on middle of the screen.

            RecvEventEnd(client); //End The Event 
        }


        private void Abdul(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.eventSelectExecCode == 0)
            {
                if ((client.Character.Hp.current == client.Character.Hp.max) &&
                    (client.Character.Mp.current == client.Character.Mp.max))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("What do you want Adul to say?"); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                }
                else
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("You drink The water and it replenishes you"); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.

                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteInt32((client.Character.Hp.max)); //To-Do : Math for Max gain of 50% MaxHp
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);
                    client.Character.Hp.toMax();

                    IBuffer res9 = BufferProvider.Provide();
                    res9.WriteInt32(client.Character.Mp.max); //To-Do : Math for Max gain of 50% MaxMp
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_mp, res9, ServerType.Area);
                    client.Character.Mp.toMax();
                }
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You hate Abdul,  He's messed up"); // Length 0xC01
                Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }
            else if (client.Character.eventSelectExecCode == 2)
            {
                IBuffer res12 = BufferProvider.Provide();
                res12.WriteCString("You Stoll hate Abdul,  He's messed up"); // Length 0xC01
                Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                    ServerType.Area); // show system message on middle of the screen.
            }


            RecvEventEnd(client); //End The Event 
        }


        private void ChangeMap(NecClient client, int objectID)
        {
            Map map = null;
            switch (objectID)
            {
                case 74013071:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002105);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002106);
                    }

                    break;
                case 74013161:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002104);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002106);
                    }

                    break;
                case 74013271:
                    Logger.Debug($"objectId[{objectID}] selected {client.Character.eventSelectExecCode}");
                    if (client.Character.eventSelectExecCode == 0)
                    {
                        map = Server.Maps.Get(2002104);
                    }
                    else if (client.Character.eventSelectExecCode == 1)
                    {
                        map = Server.Maps.Get(2002105);
                    }

                    break;
                default:
                    return;
            }

            map.EnterForce(client);
            SendEventEnd(client);
        }

        private void defaultEvent(NecClient client, int objectID)
        {
            SendEventEnd(client);
        }

        private void UpdateNPC(NecClient client, NpcSpawn npcSpawn)
        {
            if (client.Character.eventSelectExecCode == 0)
            {
                npcSpawn.Heading = (byte) (client.Character.Heading + 90);
                npcSpawn.Heading = (byte) (npcSpawn.Heading % 180);
                if (npcSpawn.Heading < 0)
                {
                    npcSpawn.Heading += 180;
                }

                npcSpawn.Updated = DateTime.Now;


                if (!Server.Database.UpdateNpcSpawn(npcSpawn))
                {
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteCString("Could not update the database"); // Length 0xC01
                    Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res12,
                        ServerType.Area); // show system message on middle of the screen.
                    return;
                }

                IBuffer res13 = BufferProvider.Provide();
                res13.WriteCString("NPC Updated"); // Length 0xC01
                Router.Send(client, (ushort) AreaPacketId.recv_event_system_message, res13,
                    ServerType.Area); // show system message on middle of the screen.

                RecvEventEnd(client); //End The Event 
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                NpcModelUpdate npcModelUpdate = new NpcModelUpdate();
                Server.Instances.AssignInstance(npcModelUpdate);
                npcModelUpdate.npcSpawn = npcSpawn;

                client.Character.currentEvent = npcModelUpdate;

                IBuffer res14 = BufferProvider.Provide();
                RecvEventRequestInt getModelId = new RecvEventRequestInt("Select Model ID from Model_common.csv", 11000,
                    1911105, npcSpawn.ModelId);
                Router.Send(getModelId, client);
            }
        }

        private void RegularInn(NecClient client, int objectID, NpcSpawn npcSpawn)
        {
            if (client.Character.secondInnAccess == true) ResolveInn(client, npcSpawn.NpcId, npcSpawn);
            else
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res7, ServerType.Area); // 

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res8, ServerType.Area); // 

                client.Character.secondInnAccess = true;
                IBuffer res9 = BufferProvider.Provide();

                switch (client.Character.eventSelectExecCode)
                {
                    case 0:
                        if (client.Soul.Level > 3)
                        {
                            SendEventEnd(client);
                            client.Character.eventSelectExtraSelectionCode = 0;
                            client.Character.eventSelectExecCode = 0;
                            client.Character.eventSelectReadyCode = 0;
                            client.Character.secondInnAccess = false;
                            break;
                        }
                        else
                        {
                            res9.WriteCString("Effect: Recover all HP, all MP, and 150 Condition"); // 
                            res9.WriteUInt32(client.Character.InstanceId);
                            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                            client.Character.eventSelectExtraSelectionCode = 0;
                            break;
                        }
                    case 1:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 50 Condition"); //
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 1;
                        break;
                    case 2:
                        res9.WriteCString("Effect: Recover half HP, half MP, and 100 Condition"); //
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 2;
                        break;
                    case 3:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 110 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 3;
                        break;
                    case 4:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 120 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.Character.eventSelectExtraSelectionCode = 4;
                        break;
                    case 5:
                        res9.WriteCString("Effect: Recover all HP, all MP, and 160 Condition"); //
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.Character.eventSelectExtraSelectionCode = 5;
                        break;
                    case 6:
                        client.Character.secondInnAccess = false;
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void CrimInn(NecClient client, int objectID, NpcSpawn npcSpawn)
        {
            if (client.Character.secondInnAccess == true) ResolveInn(client, npcSpawn.NpcId, npcSpawn);
            else
            {
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteCString("Stay"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res7,
                    ServerType.Area); // It's the fifth choice

                IBuffer res8 = BufferProvider.Provide();
                res8.WriteCString("Back"); //Length 0x601 // name of the choice
                Router.Send(client, (ushort) AreaPacketId.recv_event_select_push, res8,
                    ServerType.Area); // It's the sixth choice

                IBuffer res9 = BufferProvider.Provide();
                client.Character.secondInnAccess = true;

                switch (client.Character.eventSelectExecCode)
                {
                    case 0:
                        if (client.Soul.Level > 3)
                        {
                            SendEventEnd(client);
                            client.Character.eventSelectExtraSelectionCode = 0;
                            client.Character.eventSelectExecCode = 0;
                            client.Character.eventSelectReadyCode = 0;
                            client.Character.secondInnAccess = false;
                            break;
                        }
                        else
                        {
                            res9.WriteCString("Effect: Recover full HP, full MP, and 150 Condition"); // 
                            res9.WriteUInt32(client.Character.InstanceId);
                            Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9,
                                ServerType.Area); // 
                            client.Character.eventSelectExtraSelectionCode = 6;
                            break;
                        }
                    case 1:
                        res9.WriteCString("Effect: Recover half HP, half MP, and 50 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); //
                        client.Character.eventSelectExtraSelectionCode = 7;
                        break;
                    case 2:
                        res9.WriteCString("Effect: Recover 80% HP, 80% MP, and 80 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 8;
                        break;
                    case 3:
                        res9.WriteCString("Effect: Recover full HP, full MP, and 100 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 9;
                        break;
                    case 4:
                        res9.WriteCString("Effect: Recover full HP, full MP, and 120 Condition"); // 
                        res9.WriteUInt32(client.Character.InstanceId);
                        Router.Send(client, (ushort) AreaPacketId.recv_event_select_exec, res9, ServerType.Area); // 
                        client.Character.eventSelectExtraSelectionCode = 10;
                        break;
                    case 5:
                        client.Character.secondInnAccess = false;
                        SendEventEnd(client);
                        break;
                }
            }
        }

        private void ResolveInn(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            if (client.Character.eventSelectExecCode == 0)
            {
                int[] HPandMPperChoice = new int[] {100, 50, 100, 100, 100, 100, 100, 50, 80, 100, 100};
                int[] ConditionPerChoice = new int[] {150, 50, 100, 110, 120, 160, 150, 50, 80, 100, 120};
                ulong[] GoldCostPerChoice = new ulong[] {0, 0, 60, 300, 1200, 3000, 100, 0, 60, 300, 10000};
                Logger.Debug($"The selection you have made is {client.Character.eventSelectExtraSelectionCode}");

                client.Character.Hp.setCurrent((sbyte) HPandMPperChoice[client.Character.eventSelectExtraSelectionCode],
                    true);
                client.Character.Mp.setCurrent((sbyte) HPandMPperChoice[client.Character.eventSelectExtraSelectionCode],
                    true);
                /*client.Character.condition*/
                client.Character.Od.toMax();
                client.Character.Gp.toMax();
                client.Character.AdventureBagGold -= GoldCostPerChoice[client.Character.eventSelectExtraSelectionCode];
                if (client.Character.Hp.current >= client.Character.Hp.max) client.Character.Hp.toMax();
                if (client.Character.Mp.current >= client.Character.Mp.current) client.Character.Mp.toMax();


                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(client.Character.Hp.current);
                Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res, ServerType.Area);
                res = BufferProvider.Provide();
                res.WriteInt32(client.Character.Mp.current);
                Router.Send(client, (ushort) AreaPacketId.recv_chara_update_mp, res, ServerType.Area);
                res = BufferProvider.Provide();
                res.WriteByte((byte) ConditionPerChoice[client.Character.eventSelectExtraSelectionCode]);
                Router.Send(client, (ushort) AreaPacketId.recv_chara_update_con, res, ServerType.Area);
                res = BufferProvider.Provide();
                res.WriteUInt64(client.Character.AdventureBagGold); // Sets your Adventure Bag Gold
                Router.Send(client, (ushort) AreaPacketId.recv_self_money_notify, res, ServerType.Area);

                IBuffer res22 = BufferProvider.Provide();
                res22.WriteCString("inn/fade_bgm"); // find max size 
                Router.Send(client, (ushort) AreaPacketId.recv_event_script_play, res22, ServerType.Area);
            }
            else
            {
                SendEventEnd(client);
            }

            client.Character.eventSelectExtraSelectionCode = 0;
            client.Character.eventSelectExecCode = 0;
            client.Character.eventSelectReadyCode = 0;
            client.Character.secondInnAccess = false;
        }

        private void SoulRankNPC(NecClient client, int objectId, NpcSpawn npcSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            switch (client.Character.eventSelectExecCode)
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
            if (client.Character.eventSelectExecCode == 0)
            {
                //List<Item> Weaponlist = new System.Collections.Generic.List<Item>();
                //foreach (Item weapon in Server.Items.Values)
                //{ 
                //    if(weapon.Id > 10100101 & weapon.Id < 15300101)
                //    {
                //        Weaponlist.Add(weapon);
                //    }
                //}

                //Item item = Weaponlist[Util.GetRandomNumber(0,Weaponlist.Count)];

                //    Character character = client.Character;

                //    // Create InventoryItem
                //    InventoryItem inventoryItem = new InventoryItem();
                //    inventoryItem.Item = item;
                //    inventoryItem.ItemId = item.Id;
                //    inventoryItem.Quantity = 1;
                //    inventoryItem.CurrentDurability = item.Durability;
                //    inventoryItem.CharacterId = character.Id;
                //    inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                //    inventoryItem.State = 0;

                //    Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

                //    client.Character.Inventory.AddInventoryItem(inventoryItem);
                //    if (!Server.Database.InsertInventoryItem(inventoryItem))
                //    {
                //        IBuffer res13 = BufferProvider.Provide();
                //        res13.WriteCString("Better Luck Next Time.  I ran out of items!"); // Length 0xC01
                //        Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13, ServerType.Area); // show system message on middle of the screen.
                //        return;
                //    }

                //    RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                //    Router.Send(recvItemInstance, client);
                //    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
                //    Router.Send(recvItemInstanceUnidentified, client);

                //    itemStats(inventoryItem, client);
                
                //    IBuffer res12 = BufferProvider.Provide();
                //    res12.WriteCString($"Enjoy your new {item.Name}"); // Length 0xC01
                //    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area); // show system message on middle of the screen.
                
            }
            else if (client.Character.eventSelectExecCode == 1)
            {
                //List<Item> ArmorList = new System.Collections.Generic.List<Item>();
                //foreach (Item armor in Server.Items.Values)
                //{
                //    if (armor.Id > 16100101 & armor.Id < 30499901)
                //    {
                //        ArmorList.Add(armor);
                //    }
                //}

                //Item item = ArmorList[Util.GetRandomNumber(0, ArmorList.Count)];

                //Character character = client.Character;

                //// Create InventoryItem
                //InventoryItem inventoryItem = new InventoryItem();
                //inventoryItem.Item = item;
                //inventoryItem.ItemId = item.Id;
                //inventoryItem.Quantity = 1;
                //inventoryItem.CurrentDurability = item.Durability;
                //inventoryItem.CharacterId = character.Id;
                //inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                //inventoryItem.State = 0;

                //Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

                //client.Character.Inventory.AddInventoryItem(inventoryItem);
                //if (!Server.Database.InsertInventoryItem(inventoryItem))
                //{
                //    IBuffer res13 = BufferProvider.Provide();
                //    res13.WriteCString("Better Luck Next Time.  I ran out of items!"); // Length 0xC01
                //    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13, ServerType.Area); // show system message on middle of the screen.
                //    return;
                //}

                //RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                //Router.Send(recvItemInstance, client);
                //RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
                //Router.Send(recvItemInstanceUnidentified, client);

                //itemStats(inventoryItem, client);

                //IBuffer res12 = BufferProvider.Provide();
                //res12.WriteCString($"Enjoy your new {item.Name}"); // Length 0xC01
                //Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area); // show system message on middle of the screen.

            }
            else if (client.Character.eventSelectExecCode == 2)
            { //50401040,Moist Cudgel
                //Item item = Server.Items[50401040]; //This can select from a small array of items, and a small array of custom names

                //Character character = client.Character;

                //// Create InventoryItem
                //InventoryItem inventoryItem = new InventoryItem();
                //inventoryItem.Item = item;
                //inventoryItem.ItemId = item.Id;
                //inventoryItem.Quantity = 1;
                //inventoryItem.CurrentDurability = item.Durability;
                //inventoryItem.CharacterId = character.Id;
                //inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
                //inventoryItem.State = 0;

                //Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.Item.Id, out ItemLibrarySetting itemLibrarySetting);

                //client.Character.Inventory.AddInventoryItem(inventoryItem);
                //if (!Server.Database.InsertInventoryItem(inventoryItem))
                //{
                //    IBuffer res13 = BufferProvider.Provide();
                //    res13.WriteCString("Better Luck Next Time.  I ran out of items!"); // Length 0xC01
                //    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res13, ServerType.Area); // show system message on middle of the screen.
                //    return;
                //}

                //RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
                //Router.Send(recvItemInstance, client);
                //RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
                //Router.Send(recvItemInstanceUnidentified, client);

                //itemStats(inventoryItem, client);

                //IBuffer res12 = BufferProvider.Provide();
                //res12.WriteCString($"Enjoy your new Super {item.Name}"); // Length 0xC01
                //Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res12, ServerType.Area); // show system message on middle of the screen.
            }


            RecvEventEnd(client); //End The Event 
        }

        private void ModelLibraryWarp(NecClient client, GGateSpawn ggateSpawn)
        {
            IBuffer res = BufferProvider.Provide();
            switch (client.Character.eventSelectExecCode)
            {
                case 0:
                    res = BufferProvider.Provide();
                    res.WriteCString($"Seriously?! Walk across the bridge. Why so Lazy?"); // Length 0xC01
                    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.

                    break;
                case 1:
                    res = BufferProvider.Provide();
                    res.WriteCString("etc/warp_samemap"); // find max size 
                    res.WriteUInt32(client.Character.InstanceId); //newJp
                    Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res, ServerType.Area);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.Character.InstanceId);
                            res.WriteFloat(1574);
                            res.WriteFloat(26452);
                            res.WriteFloat(-1145);
                            res.WriteByte(client.Character.Heading);
                            res.WriteByte(client.Character.movementAnim);
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 2:
                    res = BufferProvider.Provide();
                    res.WriteCString("etc/warp_samemap"); // find max size
                    res.WriteUInt32(client.Character.InstanceId); //newJp
                    Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res, ServerType.Area);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.Character.InstanceId);
                            res.WriteFloat(21);
                            res.WriteFloat(39157);
                            res.WriteFloat(-1838);
                            res.WriteByte(client.Character.Heading);
                            res.WriteByte(client.Character.movementAnim);
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 3:
                    res = BufferProvider.Provide();
                    res.WriteCString("etc/warp_samemap"); // find max size
                    res.WriteUInt32(client.Character.InstanceId); //newJp
                    Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res, ServerType.Area);
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).ContinueWith
                    (t1 =>
                        {
                            res = BufferProvider.Provide();
                            res.WriteUInt32(client.Character.InstanceId);
                            res.WriteFloat(0);
                            res.WriteFloat(47829);
                            res.WriteFloat(-2538);
                            res.WriteByte(client.Character.Heading);
                            res.WriteByte(client.Character.movementAnim);
                            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                        }
                    );
                    break;
                case 4:
                    res = BufferProvider.Provide();
                    res.WriteCString($"Turn around genius"); // Length 0xC01
                    Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area); // show system message on middle of the screen.
                    break;

            }
            RecvEventEnd(client); //End The Event 

        }

    }
}

