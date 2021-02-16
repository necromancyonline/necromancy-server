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

        public CharacterTask(NecServer server, NecClient client)
        {
            _server = server;
            _client = client;
            tickTime = 500;
            _logoutTime = DateTime.MinValue;
            playerDied = false;
        }

        public override string TaskName => "CharacterTask";
        public override TimeSpan TaskTimeSpan { get; }
        protected override bool TaskRunAtStart => false;

        protected override void Execute()
        {
            Thread.Sleep(5000); //fix null ref on chara select. 
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

                Thread.Sleep(tickTime);
            }

            this.Stop();
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
                _client.Character.Od.setCurrent(_client.Character.Od.current + 5/*_client.Character.GPRecoveryRate*/);
                RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(_client.Character.Od.current);
                _server.Router.Send(recvCharaUpdateAp, _client);
            }
        }

        private void PlayerDead()
        {
            playerDied = true;
            _client.Character.HasDied = true; // back to dead so your soul appears with-out gear.
            _client.Character.State = CharacterState.SoulForm;
            _client.Character.State = CharacterState.SoulForm;
            _client.Character.deadType = 2;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.InstanceId);
            RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(_client.Character.InstanceId, 1);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

            brList.Add(brStart);
            brList.Add(cDead1); //animate the death of your living body
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList, _client); // send death animation to other players

            RecvBattleReportNoactDead cDead2 = new RecvBattleReportNoactDead(_client.Character.InstanceId, 2);
            brList[1] = cDead2;
            _server.Router.Send(_client, brList); // send death animaton to player 1

            DeadBody deadBody = _server.Instances.GetInstance((uint)_client.Character.DeadBodyInstanceId) as DeadBody;

            deadBody.X = _client.Character.X;
            deadBody.Y = _client.Character.Y;
            deadBody.Z = _client.Character.Z;
            deadBody.Heading = _client.Character.Heading;
            _client.Character.movementId = _client.Character.DeadBodyInstanceId;
            deadBody.BeginnerProtection = (byte)_client.Character.beginnerProtection;
            deadBody.CharaName = _client.Character.Name;
            deadBody.SoulName = _client.Soul.Name;
            deadBody.EquippedItems = _client.Character.EquippedItems;
            deadBody.RaceId = _client.Character.RaceId;
            deadBody.SexId = _client.Character.SexId;
            deadBody.HairId = _client.Character.HairId;
            deadBody.HairColorId = _client.Character.HairColorId;
            deadBody.FaceId = _client.Character.FaceId;


            _client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
            _client.Character.deadType = 1;

            //load your soul so you can run around and do soul stuff.  should also send to other soul state players.
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
            (t1 =>
                {
                    RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                    _server.Router.Send(_client, cBodyData.ToPacket());
                }
            );
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
            (t1 =>
                {
                    RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(_client.Character, _client.Soul.Name);
                    _server.Router.Send(_client, cData.ToPacket());
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
