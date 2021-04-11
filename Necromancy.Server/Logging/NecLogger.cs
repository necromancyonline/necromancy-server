using System;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Logging
{
    public class NecLogger : Logger
    {
        private NecSetting _setting;

        public override void Initialize(string identity, string name, Action<Log> write, object configuration)
        {
            base.Initialize(identity, name, write, configuration);
            _setting = configuration as NecSetting;
            if (_setting == null)
            {
                Error("Couldn't apply NecLogger configuration");
            }
        }

        public void Info(NecClient client, string message)
        {
            Info($"{client.identity} {message}");
        }

        public void Info(NecConnection connection, string message)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                Info(client, message);
                return;
            }

            Info($"{connection.identity} {message}");
        }

        public void Debug(NecClient client, string message)
        {
            Debug($"{client.identity} {message}");
        }

        public void Error(NecClient client, string message)
        {
            Error($"{client.identity} {message}");
        }

        public void Error(NecConnection connection, string message)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                Error(client, message);
                return;
            }

            Error($"{connection.identity} {message}");
        }

        public void Exception(NecClient client, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{client.identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{client.identity} {exception}", exception);
            }
        }

        public void Exception(NecConnection connection, Exception exception)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                Exception(client, exception);
                return;
            }

            if (exception == null)
            {
                Write(LogLevel.Error, $"{connection.identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{connection.identity} {exception}", exception);
            }
        }

        public void Info(ITcpSocket socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(ITcpSocket socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(ITcpSocket socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{socket.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{socket.Identity} {exception}", exception);
            }
        }

        public void LogIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.logIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.identity, packet, NecLogType.PacketIn, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                LogIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.logIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.identity, packet, NecLogType.PacketIn, serverType);
            WritePacket(logPacket);
        }

        public void LogUnknownIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.logUnknownIncomingPackets)
            {
                NecLogPacket logPacket =
                    new NecLogPacket(client.identity, packet, NecLogType.PacketUnhandled, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                LogUnknownIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.logIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket =
                new NecLogPacket(connection.identity, packet, NecLogType.PacketUnhandled, serverType);
            WritePacket(logPacket);
        }

        public void LogOutgoingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.logOutgoingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.identity, packet, NecLogType.PacketOut, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogOutgoingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.client;
            if (client != null)
            {
                LogOutgoingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.logIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.identity, packet, NecLogType.PacketOut, serverType);
            WritePacket(logPacket);
        }

        private void WritePacket(NecLogPacket packet)
        {
            Write(LogLevel.Info, packet.ToLogText(), packet);
        }
    }
}
