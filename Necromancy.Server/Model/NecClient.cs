using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    [DebuggerDisplay("{Identity,nq}")]
    public class NecClient
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(NecClient));

        public NecClient()
        {
            Creation = DateTime.Now;
            Identity = "";
            Soul = new Soul();
            Character = new Character();
            BodyCollection = new Dictionary<uint,NecClient>();
        }

        public DateTime Creation { get; }
        public string Identity { get; private set; }
        public Account Account { get; set; }
        public Soul Soul { get; set; }
        public Character Character { get; set; }
        public Channel Channel { get; set; }
        public Map Map { get; set; }
        public Union.Union Union { get; set; }
        public NecConnection AuthConnection { get; set; }
        public NecConnection MsgConnection { get; set; }
        public NecConnection AreaConnection { get; set; }
        public Dictionary<uint, NecClient> BodyCollection { get; set; }

        public void Send(NecPacket packet)
        {
            switch (packet.ServerType)
            {
                case ServerType.Area:
                    AreaConnection.Send(packet);
                    break;
                case ServerType.Msg:
                    MsgConnection.Send(packet);
                    break;
                case ServerType.Auth:
                    AuthConnection.Send(packet);
                    break;
                default:
                    Logger.Error(this, "Invalid ServerType");
                    break;
            }
        }

        public void UpdateIdentity()
        {
            Identity = "";

            if (Character != null)
            {
                Identity += $"[Char:{Character.Id}:{Character.Name}]";
                return;
            }

            if (Account != null)
            {
                Identity += $"[Acc:{Account.Id}:{Account.Name}]";
                return;
            }

            if (AuthConnection != null)
            {
                Identity += $"[Con:{AuthConnection.Identity}]";
            }
        }

        public void Close()
        {
            AuthConnection?.Socket.Close();
            MsgConnection?.Socket.Close();
            AreaConnection?.Socket.Close();
        }
    }
}
