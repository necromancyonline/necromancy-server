using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyApply : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendPartyApply));

        public SendPartyApply(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int errorCheck = packet.data.ReadInt32();
            uint targetPartyInstanceId = packet.data.ReadUInt32();
            _Logger.Debug($"{client.character.name}Applied to party {targetPartyInstanceId} with sys_msg {errorCheck}. 0 is good");

            Party myParty = server.instances.GetInstance(targetPartyInstanceId) as Party;
            NecClient myPartyLeaderClient = server.clients.GetByCharacterInstanceId(myParty.partyLeaderId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errorCheck);
            router.Send(client, (ushort)AreaPacketId.recv_party_apply_r, res, ServerType.Area);

            RecvPartyNotifyApply recvPartyNotifyApply = new RecvPartyNotifyApply(client);
            router.Send(recvPartyNotifyApply, myPartyLeaderClient); //Send the application to the party leader.
        }
    }
}
