using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet
{
    public class PacketRouter
    {
        private readonly object AreaLock = new object();
        private readonly object ClientLock = new object();

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, NecPacket packet)
        {
            client.Send(packet);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, NecPacket packet)
        {
            connection.Send(packet);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, ushort id, IBuffer data, ServerType serverType)
        {
            NecPacket packet = new NecPacket(id, data, serverType);
            Send(client, packet);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, ushort id, IBuffer data)
        {
            NecPacket packet = new NecPacket(id, data, connection.ServerType);
            Send(connection, packet);
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, NecPacket packet, params NecClient[] excepts)
        {
            clients = GetClients(clients, excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet);
            }
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, ushort id, IBuffer data, ServerType serverType,
            params NecClient[] excepts)
        {
            Send(clients, new NecPacket(id, data, serverType), excepts);
        }


        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, ushort id, IBuffer data, ServerType serverType, params NecClient[] excepts)
        {
            Send(map, new NecPacket(id, data, serverType), excepts);
        }

        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, NecPacket packet, params NecClient[] excepts)
        {
            if (map == null) return;
            List<NecClient> clients = GetClients(map.ClientLookup.GetAll(), excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet);
            }
        }

        /// <summary>
        /// Send a specific packet response.
        /// </summary>
        public void Send(PacketResponse response)
        {
            foreach (NecClient client in response.Clients)
            {
                Send(client, response.ToPacket());
            }
        }

        /// <summary>
        /// Send a List of packets to everyone in the map.
        /// Will block until complete
        /// </summary>
        public void Send(Map map, List<PacketResponse> buffers, params NecClient[] excepts)
        {
            lock (AreaLock)
            {
                foreach (PacketResponse data in buffers)
                {
                    Send(map, data, excepts);
                }
            }
        }

        /// <summary>
        /// Send a List of packets to everyone in the map.
        /// Will block until complete
        /// </summary>
        public void Send(NecClient client, List<PacketResponse> buffers)
        {
            lock (ClientLock)
            {
                foreach (PacketResponse response in buffers)
                {
                    response.Clients.Add(client);
                    Send(response);
                }
            }
        }

        /// <summary>
        /// Send a specific packet response to a map.
        /// </summary>
        public void Send(Map map, PacketResponse response, params NecClient[] excepts)
        {
            List<NecClient> mapClients = map.ClientLookup.GetAll();
            mapClients.AddRange(response.Clients);
            response.Clients.Clear();
            List<NecClient> clients = GetClients(mapClients, excepts);
            response.Clients.AddRange(clients);
            Send(response);
        }

        /// <summary>
        /// Send a specific packet response.
        /// </summary>
        public void Send(PacketResponse response, List<NecClient> clientList)
        {
            foreach (NecClient client in clientList)
            {
                response.Clients.Add(client);              
            }
            Send(response);
        }

        /// <summary>
        /// Send a specific packet response.
        /// </summary>
        public void Send(PacketResponse response, params NecClient[] clients)
        {
            response.Clients.AddRange(clients);
            Send(response);
        }

        /// <summary>
        /// Send a chat message
        /// </summary>
        public void Send(ChatResponse response)
        {
            RecvChatNotifyMessage notifyMessage = new RecvChatNotifyMessage(response);
            notifyMessage.Clients.AddRange(response.Recipients);
            Send(notifyMessage);
        }

        private List<NecClient> GetClients(List<NecClient> clients, params NecClient[] excepts)
        {
            if (excepts.Length == 0)
            {
                return clients;
            }

            foreach (NecClient except in excepts)
            {
                clients.Remove(except);
            }

            return clients;
        }
    }
}
