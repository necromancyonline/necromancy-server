using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_apply : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_apply));

        public send_party_apply(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int errorCheck = packet.Data.ReadInt32();
            uint targetPartyInstanceId = packet.Data.ReadUInt32();
            Logger.Debug($"{client.Character.Name}Applied to party {targetPartyInstanceId} with sys_msg {errorCheck}. 0 is good");

            Party myParty = Server.Instances.GetInstance(targetPartyInstanceId) as Party;
            NecClient myPartyLeaderClient = Server.Clients.GetByCharacterInstanceId(myParty.PartyLeaderId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errorCheck);
            Router.Send(client, (ushort) AreaPacketId.recv_party_apply_r, res, ServerType.Area);

            RecvPartyNotifyApply recvPartyNotifyApply = new RecvPartyNotifyApply(client);
            Router.Send(recvPartyNotifyApply, myPartyLeaderClient); //Send the application to the party leader.
        }
    }
}
