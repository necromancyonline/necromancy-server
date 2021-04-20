using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecQueueConsumer : ThreadedBlockingQueueConsumer
    {
        public const int NO_EXPECTED_SIZE = -1;

        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecQueueConsumer));

        private readonly Dictionary<int, IClientHandler> _clientHandlers;
        private readonly Dictionary<int, IConnectionHandler> _connectionHandlers;
        private readonly Dictionary<ITcpSocket, NecConnection> _connections;
        private readonly object _lock;

        private readonly ServerType _serverType;
        private readonly NecSetting _setting;
        public Action<NecConnection> clientConnected;

        public Action<NecConnection> clientDisconnected;

        public NecQueueConsumer(ServerType serverType, NecSetting setting, AsyncEventSettings socketSetting) : base(
            socketSetting, serverType.ToString())
        {
            _serverType = serverType;
            _setting = setting;
            _lock = new object();
            _clientHandlers = new Dictionary<int, IClientHandler>();
            _connectionHandlers = new Dictionary<int, IConnectionHandler>();
            _connections = new Dictionary<ITcpSocket, NecConnection>();
        }

        public void Clear()
        {
            _clientHandlers.Clear();
            _connectionHandlers.Clear();
        }

        public void AddHandler(IClientHandler clientHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_clientHandlers.ContainsKey(clientHandler.id))
                    _clientHandlers[clientHandler.id] = clientHandler;
                else
                    _clientHandlers.Add(clientHandler.id, clientHandler);

                return;
            }

            if (_clientHandlers.ContainsKey(clientHandler.id))
                _Logger.Error($"[{_serverType}] ClientHandlerId: {clientHandler.id} already exists");
            else
                _clientHandlers.Add(clientHandler.id, clientHandler);
        }

        public void AddHandler(IConnectionHandler connectionHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_connectionHandlers.ContainsKey(connectionHandler.id))
                    _connectionHandlers[connectionHandler.id] = connectionHandler;
                else
                    _connectionHandlers.Add(connectionHandler.id, connectionHandler);

                return;
            }

            if (_connectionHandlers.ContainsKey(connectionHandler.id))
                _Logger.Error($"[{_serverType}] ConnectionHandlerId: {connectionHandler.id} already exists");
            else
                _connectionHandlers.Add(connectionHandler.id, connectionHandler);
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive) return;

            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _Logger.Error(socket, $"[{_serverType}] Client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
            }

            List<NecPacket> packets = connection.Receive(data);
            foreach (NecPacket packet in packets)
            {
                NecClient client = connection.client;
                if (client != null)
                    HandleReceived_Client(client, packet);
                else
                    HandleReceived_Connection(connection, packet);
            }
        }

        private void HandleReceived_Connection(NecConnection connection, NecPacket packet)
        {
            if (!_connectionHandlers.ContainsKey(packet.id))
            {
                _Logger.LogUnknownIncomingPacket(connection, packet, _serverType);
                return;
            }

            IConnectionHandler connectionHandler = _connectionHandlers[packet.id];
            if (connectionHandler.expectedSize != NO_EXPECTED_SIZE && packet.data.Size < connectionHandler.expectedSize)
            {
                _Logger.Error(connection,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.id}) is smaller ({packet.data.Size}) than expected ({connectionHandler.expectedSize})");
                return;
            }

            _Logger.LogIncomingPacket(connection, packet, _serverType);
            packet.data.SetPositionStart();
            try
            {
                connectionHandler.Handle(connection, packet);
            }
            catch (Exception ex)
            {
                _Logger.Exception(connection, ex);
            }
        }

        private void HandleReceived_Client(NecClient client, NecPacket packet)
        {
            if (!_clientHandlers.ContainsKey(packet.id))
            {
                _Logger.LogUnknownIncomingPacket(client, packet, _serverType);
                return;
            }

            IClientHandler clientHandler = _clientHandlers[packet.id];
            if (clientHandler.expectedSize != NO_EXPECTED_SIZE && packet.data.Size < clientHandler.expectedSize)
            {
                _Logger.Error(client,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.id}) is smaller ({packet.data.Size}) than expected ({clientHandler.expectedSize})");
                return;
            }

            _Logger.LogIncomingPacket(client, packet, _serverType);
            packet.data.SetPositionStart();
            try
            {
                clientHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                _Logger.Exception(client, ex);
            }
        }

        protected override void HandleDisconnected(ITcpSocket socket)
        {
            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _Logger.Error(socket, $"[{_serverType}] Disconnected client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
                _connections.Remove(socket);
                _Logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientDisconnected = clientDisconnected;
            if (onClientDisconnected != null)
                try
                {
                    onClientDisconnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _Logger.Exception(connection, ex);
                }

            _Logger.Info(connection, $"[{_serverType}] Client disconnected");
        }

        protected override void HandleConnected(ITcpSocket socket)
        {
            NecConnection connection = new NecConnection(socket, new PacketFactory(_setting), _serverType);
            lock (_lock)
            {
                _connections.Add(socket, connection);
                _Logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientConnected = clientConnected;
            if (onClientConnected != null)
                try
                {
                    onClientConnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _Logger.Exception(connection, ex);
                }

            _Logger.Info(connection, $"[{_serverType}] Client connected");
        }
    }
}
