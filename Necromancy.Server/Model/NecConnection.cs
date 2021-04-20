using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    public class NecConnection
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecConnection));

        public NecConnection(ITcpSocket clientSocket, PacketFactory packetFactory, ServerType serverType)
        {
            socket = clientSocket;
            this.packetFactory = packetFactory;
            this.serverType = serverType;
            client = null;
        }

        public string identity => socket.Identity;
        public ServerType serverType { get; }
        public ITcpSocket socket { get; }
        public PacketFactory packetFactory { get; }
        public NecClient client { get; set; }

        public List<NecPacket> Receive(byte[] data)
        {
            List<NecPacket> packets;
            try
            {
                packets = packetFactory.Read(data, serverType);
            }
            catch (Exception ex)
            {
                _Logger.Exception(this, ex);
                packets = new List<NecPacket>();
            }

            return packets;
        }

        public void Send(NecPacket packet)
        {
            byte[] data;
            try
            {
                data = packetFactory.Write(packet);
            }
            catch (Exception ex)
            {
                _Logger.Exception(this, ex);
                return;
            }

            _Logger.LogOutgoingPacket(this, packet, serverType);
            socket.Send(data);
        }
    }
}
