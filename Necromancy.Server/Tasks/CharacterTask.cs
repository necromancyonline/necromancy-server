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
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(CharacterTask));

        private readonly object _logoutLock = new object();
        private NecServer _server;
        private NecClient _client;
        private int _tickTime;
        private DateTime _logoutTime;
        private byte _logoutType;
        private bool _playerDied;
        private int _tickCounter;
        private List<NecClient> _clients;

        public CharacterTask(NecServer server, NecClient client)
        {
            _server = server;
            _client = client;
            _tickTime = 500;
            _logoutTime = DateTime.MinValue;
            _playerDied = false;
            _tickCounter = 0;
        }

        public override string taskName => $"CharacterTask : {_client.soul.name}";
        public override TimeSpan taskTimeSpan { get; }
        protected override bool taskRunAtStart => false;

        protected override void Execute()
        {
            Thread.Sleep(5000); //fix null ref on chara select.
            if(_client == null) this.Stop(); //crash/disconnect handling
            while (_client.character.characterActive)
            {
                if (_logoutTime != DateTime.MinValue)
                {
                    if (DateTime.Now >= _logoutTime)
                    {
                        LogOutRequest();
                    }
                }

                if (_client.character.hp.depleted && !_playerDied)
                    PlayerDead();
                else if (!_client.character.hp.depleted && _playerDied)
                    _playerDied = false;

                StatRegen();

                if (_tickCounter == 600)
                {
                    CriminalRepent();
                    SoulMaterialIncrease();
                    _tickCounter = 0;
                }

                _tickCounter++;
                Thread.Sleep(_tickTime);
            }

            this.Stop();
        }

        private void CriminalRepent()
        {
            _client.soul.criminalLevel--;
            if (_client.soul.criminalLevel <= 0) _client.soul.criminalLevel = 0;

            _client.character.criminalState = _client.soul.criminalLevel;

        }
        private void SoulMaterialIncrease()
        {
            _client.soul.materialChaos += 10;
            _client.soul.materialLawful += 10;
            _client.soul.materialLife += 10;
            _client.soul.materialReincarnation += 10;
            _client.soul.pointsChaos += 10; //temporary. testing
        }
        private void StatRegen()
        {
            if (_client.character.gp.current < _client.character.gp.max)
            {
                _client.character.gp.SetCurrent(_client.character.gp.current + 5/*_client.Character.GPRecoveryRate*/);
                RecvCharaUpdateAc recvCharaUpdateAc = new RecvCharaUpdateAc(_client.character.gp.current);
                _server.router.Send(recvCharaUpdateAc, _client);
            }

            if(_client.character.movementPose == 4/*running byte*/)
            {
                _client.character.od.SetCurrent(_client.character.od.current - 5/*_client.Character.APCostDiff*/);
                RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(_client.character.od.current);
                _server.router.Send(recvCharaUpdateAp, _client);
            }
            else if (_client.character.od.current < _client.character.od.max)
            {
                _client.character.od.SetCurrent(_client.character.od.current + _client.character.odRecoveryRate/2);
                RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(_client.character.od.current);
                _server.router.Send(recvCharaUpdateAp, _client);
            }
        }

        private void PlayerDead()
        {
            _playerDied = true;
            _client.character.hasDied = true;
            _client.character.state = CharacterState.SoulForm;
            _client.character.deadType = (short)Util.GetRandomNumber(1, 4);
            _Logger.Debug($"Death Animation Number : {_client.character.deadType}");

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(_client.character.instanceId, _client.character.deadType);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

            brList.Add(brStart);
            brList.Add(cDead1); //animate the death of your living body
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList); // send death animation to all players

            DeadBody deadBody = _server.instances.GetInstance((uint)_client.character.deadBodyInstanceId) as DeadBody;
            deadBody.x = _client.character.x;
            deadBody.y = _client.character.y;
            deadBody.z = _client.character.z;
            deadBody.heading = _client.character.heading;
            deadBody.beginnerProtection = (byte)_client.character.beginnerProtection;
            deadBody.charaName = _client.character.name;
            deadBody.soulName = _client.soul.name;
            deadBody.equippedItems = _client.character.equippedItems;
            deadBody.raceId = _client.character.raceId;
            deadBody.sexId = _client.character.sexId;
            deadBody.hairId = _client.character.hairId;
            deadBody.hairColorId = _client.character.hairColorId;
            deadBody.faceId = _client.character.faceId;
            deadBody.faceArrangeId = _client.character.faceArrangeId;
            deadBody.voiceId = _client.character.voiceId;
            deadBody.level = _client.character.level;
            deadBody.classId = _client.character.classId;
            deadBody.equippedItems = _client.character.equippedItems;
            deadBody.criminalStatus = _client.character.criminalState;
            deadBody.connectionState = 1;
            _clients = _client.map.clientLookup.GetAll();
            _client.map.deadBodies.Add(deadBody.instanceId, deadBody);
            List<NecClient> soulStateClients = new List<NecClient>();

            //Disappear .. all the monsters, NPCs, and characters.  welcome to death! it's lonely
            foreach (NpcSpawn npcSpawn in _client.map.npcSpawns.Values)
            {
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(npcSpawn.instanceId);
                _server.router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }
            foreach (MonsterSpawn monsterSpawn in _client.map.monsterSpawns.Values)
            {
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(monsterSpawn.instanceId);
                _server.router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }
            foreach (NecClient client in _clients)
            {
                if (client == _client) continue; //Don't dissapear yourself ! that'd be bad news bears.
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.character.instanceId);
                _server.router.Send(_client, recvObjectDisappearNotify.ToPacket());
            }

            //load your dead body on the map for looting.  disappear your character model for everyone else besides you
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
            (t1 =>
                {
                    RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                    if (_client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                    { _server.router.Send(_client.map, cBodyData.ToPacket(), _client); }
                    _server.router.Send(_client, cBodyData.ToPacket());
                    RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(_client.character.instanceId);
                    _server.router.Send(_client.map, recvObjectDisappearNotify.ToPacket(),_client);
                    //send your soul to all the other souls runnin around
                    foreach (NecClient client in _clients)
                    {
                        if (client.character.state == CharacterState.SoulForm) { soulStateClients.Add(client); }
                    }
                    //re-render your soulstate character to your client with out gear on it, and any other soul state clients on map.
                    RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(_client.character, _client.soul.name);
                    _server.router.Send(soulStateClients, cData.ToPacket());
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
                        if (otherClient.character.state == CharacterState.SoulForm)
                        {
                            RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.character, otherClient.soul.name);
                            _server.router.Send(otherCharacterData, _client);
                        }

                        if (otherClient.union != null)
                        {
                            RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.character, otherClient.union.name);
                            _server.router.Send(otherUnionData, _client);
                        }
                    }
                    foreach (NpcSpawn npcSpawn in _client.map.npcSpawns.Values)
                    {
                        if (npcSpawn.visibility == 2) //2 is the magic number for soul state only.  make it an Enum some day
                        {
                            RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                            _server.router.Send(npcData, _client);
                        }
                    }
                }
            );

        }

        public void Logout(DateTime logoutTime, byte logoutType)
        {
            lock (_logoutLock)
            {
                _logoutTime = logoutTime;
                _logoutType = logoutType;
            }

            _Logger.Debug($"logoutTime [{logoutTime}] _logoutType [{_logoutType}]");
        }

        private void LogOutRequest()
        {
            _logoutTime = DateTime.MinValue;
            IBuffer res = BufferProvider.Provide();
            _Logger.Debug($"_logoutType [{_logoutType}]");
            if (_logoutType == 0x00) // Return to Title   also   Exit Game
            {
                res = null;
                res = BufferProvider.Provide();
                //res.WriteInt64(1);
                //res.WriteInt16(1);
                _server.router.Send(_client, (ushort) AreaPacketId.recv_escape_start, res, ServerType.Area);


                //IBuffer buffer = BufferProvider.Provide();
                //buffer.WriteInt32(0);
                //NecPacket response = new NecPacket((ushort)CustomPacketId.RecvDisconnect,buffer,ServerType.Msg,PacketType.Disconnect);

                //_server.Router.Send(_client, response);
            }

            if (_logoutType == 0x01) // Return to Character Select
            {
                res.WriteInt32(0);
                _server.router.Send(_client, (ushort) MsgPacketId.recv_chara_select_back_soul_select_r, res,
                    ServerType.Msg);

                Thread.Sleep(4100);

                res = null;
                res = BufferProvider.Provide();
                res.WriteInt32(0);
                res.WriteByte(0);
                _server.router.Send(_client, (ushort) MsgPacketId.recv_soul_authenticate_passwd_r, res, ServerType.Msg);
            }

            if (_logoutType == 0x02)
            {
                res.WriteInt32(0);
                _server.router.Send(_client, (ushort) MsgPacketId.recv_chara_select_back_r, res, ServerType.Msg);
            }
        }
    }
}
