using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    [DebuggerDisplay("{identity,nq}")]
    public class NecClient
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecClient));

        public NecClient()
        {
            creation = DateTime.Now;
            identity = "";
            soul = new Soul();
            character = new Character();
            bodyCollection = new Dictionary<uint,NecClient>();
        }

        public DateTime creation { get; }
        public string identity { get; private set; }
        public Account account { get; set; }
        public Soul soul { get; set; }
        public Character character { get; set; }
        public Channel channel { get; set; }
        public Map map { get; set; }
        public Union.Union union { get; set; }
        public NecConnection authConnection { get; set; }
        public NecConnection msgConnection { get; set; }
        public NecConnection areaConnection { get; set; }
        public Dictionary<uint, NecClient> bodyCollection { get; set; }

        public void Send(NecPacket packet)
        {
            switch (packet.serverType)
            {
                case ServerType.Area:
                    areaConnection.Send(packet);
                    break;
                case ServerType.Msg:
                    msgConnection.Send(packet);
                    break;
                case ServerType.Auth:
                    authConnection.Send(packet);
                    break;
                default:
                    _Logger.Error(this, "Invalid ServerType");
                    break;
            }
        }

        public void UpdateIdentity()
        {
            identity = "";

            if (character != null)
            {
                identity += $"[Char:{character.id}:{character.name}]";
                return;
            }

            if (account != null)
            {
                identity += $"[Acc:{account.id}:{account.name}]";
                return;
            }

            if (authConnection != null)
            {
                identity += $"[Con:{authConnection.identity}]";
            }
        }

        public void Close()
        {
            authConnection?.socket.Close();
            msgConnection?.socket.Close();
            areaConnection?.socket.Close();
        }
    }
}
