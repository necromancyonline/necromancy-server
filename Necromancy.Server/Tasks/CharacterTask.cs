using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks.Core;

namespace Necromancy.Server.Tasks
{
    public class CharacterTask : PeriodicTask

    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(CharacterTask));

        private readonly object LogoutLock = new object();
        private NecServer _server;
        private NecClient _client;
        private int tickTime;
        private DateTime _logoutTime;
        private byte _logoutType;
        private bool playerDied;
        private int _tickCounter;
        private List<NecClient> _clients;

        public CharacterTask(NecServer server, NecClient client)
        {
            _server = server;
            _client = client;
            tickTime = 500;
            _logoutTime = DateTime.MinValue;
            playerDied = false;
            _tickCounter = 0;
        }

        public override string TaskName => $"CharacterTask : {_client.Soul.Name}";
        public override TimeSpan TaskTimeSpan { get; }
        protected override bool TaskRunAtStart => false;

        protected override void Execute()
        {
            Thread.Sleep(5000); //fix null ref on chara select. 
            if(_client == null) this.Stop(); //crash/disconnect handling
            while (_client.Character.characterActive)
            {
                if (_logoutTime != DateTime.MinValue)
                {
                    if (DateTime.Now >= _logoutTime)
                    {
                        LogOutRequest();
                    }
                }

                if (_client.Character.Hp.depleted && !playerDied)
                    PlayerDead();
                else if (!_client.Character.Hp.depleted && playerDied)
                    playerDied = false;

                StatRegen();

                if (_tickCounter == 600)
                {
                    CriminalRepent();
                    SoulMaterialIncrease();
                    _tickCounter = 0;
                }

                _tickCounter++;
                Thread.Sleep(tickTime);
            }

            this.Stop();
        }

        private void CriminalRepent()
        {
            _client.Soul.CriminalLevel--;
            if (_client.Soul.CriminalLevel <= 0) _client.Soul.CriminalLevel = 0;

            _client.Character.criminalState = _client.Soul.CriminalLevel;

        }
        private void SoulMaterialIncrease()
        {
            _client.Soul.MaterialChaos += 10;
            _client.Soul.MaterialLawful += 10;
            _client.Soul.MaterialLife += 10;
            _client.Soul.MaterialReincarnation += 10;
            _client.Soul.PointsChaos += 10; //temporary. testing
        }
        private void StatRegen()
        {
            if (_client.Character.Gp.current < _client.Character.Gp.max) 
            { 
                _client.Character.Gp.setCurrent(_client.Character.Gp.current + 5/*_client.Character.GPRecoveryRate*/);
                RecvCharaUpdateAc recvCharaUpdateAc = new RecvCharaUpdateAc(_client.Character.Gp.current);
                _server.Router.Send(recvCharaUpdateAc, _client);
            }

            if(_client.Character.movementPose == 4/*running byte*/)
            {
                _client.Character.Od.setCurrent(_client.Character.Od.current - 5/*_client.Character.APCostDiff*/);
                RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(_client.Character.Od.current);
                _server.Router.Send(recvCharaUpdateAp, _client);
            }
            else if (_client.Character.Od.current < _client.Character.Od.max)
            {
                _client.Character.Od.setCurrent(_client.Character.Od.current + _client.Character.OdRecoveryRate/2);
                RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(_client.Character.Od.current);
                _server.Router.Send(recvCharaUpdateAp, _client);
            }
        }

        private void PlayerDead()
        {
            playerDied = true;
            _client.Character.HasDied = true; 
            _client.Character.State = CharacterState.SoulForm;
            _client.Character.deadType = (short)Util.GetRandomNumber(1, 4);
            Logger.Debug($"Death Animation Number : {_client.Character.deadType}");

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(_client.Character.InstanceId, _client.Character.deadType);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

            brList.Add(brStart);
            brList.Add(cDead1); //animate the death of your living body
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList); // send death animation to all players

            DeadBody deadBody = _server.Instances.GetInstance((uint)_client.Character.DeadBodyInstanceId) as DeadBody;
            deadBody.X = _client.Character.X;
            deadBody.Y = _client.Character.Y;
            deadBody.Z = _client.Character.Z;
            deadBody.Heading = _client.Character.Heading;
            deadBody.BeginnerProtection = (byte)_client.Character.beginnerProtection;
            deadBody.CharaName = _client.Character.Name;
            deadBody.SoulName = _client.Soul.Name;
            deadBody.EquippedItems = _client.Character.EquippedItems;
            deadBody.RaceId = _client.Character.RaceId;
            deadBody.SexId = _client.Character.SexId;
            deadBody.HairId = _client.Character.HairId;
            deadBody.HairColorId = _client.Character.HairColorId;
            deadBody.FaceId = _client.Character.FaceId;
            deadBody.FaceArrangeId = _client.Character.FaceArrangeId;
            deadBody.VoiceId = _client.Character.VoiceId;
            deadBody.Level = _client.Character.Level;
            deadBody.ClassId = _client.Character.ClassId;
            deadBody.EquippedItems = _client.Character.EquippedItems;
            deadBody.CriminalStatus = _client.Character.criminalState;
            deadBody.ConnectionState = 1;
            _clients = _client.Map.ClientLookup.GetAll();
            _client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
            List<NecClient> soulStateClients = new List<NecClient>();

            //Disappear .. all the monsters, NPCs, and characters.  welcome to death! it's lonely
            foreach (NpcSpawn npcSpawn in _client.Map.NpcSpawns.Values)
            {
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(npcSpawn.InstanceId);
                _server.Router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }
            foreach (MonsterSpawn monsterSpawn in _client.Map.MonsterSpawns.Values)
            {
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(monsterSpawn.InstanceId);
                _server.Router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }
            foreach (NecClient client in _clients)
            {
                if (client == _client) continue; //Don't dissapear yourself ! that'd be bad news bears.
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.Character.InstanceId);
                _server.Router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }

            //load your dead body on the map for looting.  disappear your character model for everyone else besides you
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
            (t1 =>
                {
                    RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                    if (_client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                    { _server.Router.Send(_client.Map, cBodyData.ToPacket(), _client); }
                    _server.Router.Send(_client, cBodyData.ToPacket());
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(_client.Character.InstanceId);
                    _server.Router.Send(_client.Map, recvObjectDisappearNotify.ToPacket(),_client);
                    //send your soul to all the other souls runnin around
                    foreach (NecClient client in _clients)
                    {
                        if (client.Character.State == CharacterState.SoulForm) { soulStateClients.Add(client); }
                    }
                    //re-render your soulstate character to your client with out gear on it, and any other soul state clients on map.
                    RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(_client.Character, _client.Soul.Name);
                    _server.Router.Send(soulStateClients, cData.ToPacket());
                }
            );
            //Re-render all the NPCs and Monsters, and character objects
            Task.Delay(TimeSpan.FromSeconds(6)).ContinueWith
            (t1 =>
                {
                    foreach (NecClient otherClient in _clients)
                    {
                        if (otherClient == _client)
                        {
                            // skip myself
                            continue;
                        }
                        //Render all the souls if you are in soul form yourself
                        if (otherClient.Character.State == CharacterState.SoulForm)
                        {
                            RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                            _server.Router.Send(otherCharacterData, _client);
                        }

                        if (otherClient.Union != null)
                        {
                            RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                            _server.Router.Send(otherUnionData, _client);
                        }
                    }
                    foreach (NpcSpawn npcSpawn in _client.Map.NpcSpawns.Values)
                    {
                        if (npcSpawn.Visibility == 2) //2 is the magic number for soul state only.  make it an Enum some day
                        {
                            RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                            _server.Router.Send(npcData, _client);
                        }
                    }
                }
            );

        }

        public void Logout(DateTime logoutTime, byte logoutType)
        {
            lock (LogoutLock)
            {
                _logoutTime = logoutTime;
                _logoutType = logoutType;
            }

            Logger.Debug($"logoutTime [{logoutTime}] _logoutType [{_logoutType}]");
        }

        private void LogOutRequest()
        {
            _logoutTime = DateTime.MinValue;
            IBuffer res = BufferProvider.Provide();
            Logger.Debug($"_logoutType [{_logoutType}]");
            if (_logoutType == 0x00) // Return to Title   also   Exit Game
            {
                res = null;
                res = BufferProvider.Provide();
                //res.WriteInt64(1);
                //res.WriteInt16(1);
                _server.Router.Send(_client, (ushort) AreaPacketId.recv_escape_start, res, ServerType.Area);


                //IBuffer buffer = BufferProvider.Provide();
                //buffer.WriteInt32(0);
                //NecPacket response = new NecPacket((ushort)CustomPacketId.RecvDisconnect,buffer,ServerType.Msg,PacketType.Disconnect);

                //_server.Router.Send(_client, response);
            }

            if (_logoutType == 0x01) // Return to Character Select
            {
                res.WriteInt32(0);
                _server.Router.Send(_client, (ushort) MsgPacketId.recv_chara_select_back_soul_select_r, res,
                    ServerType.Msg);

                Thread.Sleep(4100);

                res = null;
                res = BufferProvider.Provide();
                res.WriteInt32(0);
                res.WriteByte(0);
                _server.Router.Send(_client, (ushort) MsgPacketId.recv_soul_authenticate_passwd_r, res, ServerType.Msg);
            }

            if (_logoutType == 0x02)
            {
                res.WriteInt32(0);
                _server.Router.Send(_client, (ushort) MsgPacketId.recv_chara_select_back_r, res, ServerType.Msg);
            }
        }
    }
}
