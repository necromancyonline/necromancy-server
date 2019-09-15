using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    [DebuggerDisplay("{Identity,nq}")]
    public class NecClient
    {
        private readonly NecLogger _logger;

        public NecClient(ITcpSocket clientSocket, PacketFactory packetFactory)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            PacketFactory = packetFactory;
            Socket = clientSocket;
            UpdateIdentity();
        }

        #region Session

        public Session Session { get; set; }

        public Account Account
        {
            get => Session.Account;
            set => Session.Account = value;
        }

        public Soul Soul
        {
            get => Session.Soul;
            set => Session.Soul = value;
        }

        public Character Character
        {
            get => Session.Character;
            set => Session.Character = value;
        }

        public Channel Channel
        {
            get => Session.Channel;
            set => Session.Channel = value;
        }

        public Map Map
        {
            get => Session.Map;
            set => Session.Map = value;
        }

        #endregion

        public string Identity { get; private set; }
        public ITcpSocket Socket { get; }
        public PacketFactory PacketFactory { get; }

        public List<NecPacket> Receive(byte[] data)
        {
            List<NecPacket> packets;
            try
            {
                packets = PacketFactory.Read(data, this);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                packets = new List<NecPacket>();
            }

            return packets;
        }

        public void Send(NecPacket packet)
        {
            byte[] data;
            try
            {
                data = PacketFactory.Write(packet, this);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                return;
            }

            _logger.LogOutgoingPacket(this, packet);
            Socket.Send(data);
        }

        public void UpdateIdentity()
        {
            Identity = $"[{Socket.Identity}]";
        }
    }
}