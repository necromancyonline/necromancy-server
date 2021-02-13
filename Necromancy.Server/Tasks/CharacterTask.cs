using System;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
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
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.Character.killerInstanceId);
            RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(_client.Character.InstanceId, 1);
            RecvBattleReportNoactDead cDead2 = new RecvBattleReportNoactDead(_client.Character.InstanceId, 2);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

            brList.Add(brStart);
            brList.Add(cDead1); //animate the death of your living body
            brList.Add(brEnd);
            _server.Router.Send(_client.Map, brList, _client); // send death animation to other players


            brList[1] = cDead2;
            _server.Router.Send(_client, brList); // send death animaton to player 1

            DeadBody deadBody = _server.Instances.GetInstance((uint) _client.Character.DeadBodyInstanceId) as DeadBody;

            deadBody.X = _client.Character.X;
            deadBody.Y = _client.Character.Y;
            deadBody.Z = _client.Character.Z;
            deadBody.Heading = _client.Character.Heading;
            _client.Character.movementId = _client.Character.DeadBodyInstanceId;
            _client.Character.deadType = 1;

            Thread.Sleep(5000);
            //_client.Character.HasDied = false; // quick switch to living state so your dead body loads with your gear
            //load your dead body on to the map for you to see in soul form. 
            //_client.Character.State = Model.CharacterModel.CharacterState.SoulForm;  //Mess with this after you finish inventory
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody, _client.Character, _client);
            _server.Router.Send(_client, cBodyData.ToPacket());

            _client.Character.HasDied = true; // back to dead so your soul appears with-out gear.

            Thread.Sleep(100);

            //reload your living body with no gear
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(_client.Character, _client.Soul.Name);
            _server.Router.Send(_client.Map, cData.ToPacket());
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
