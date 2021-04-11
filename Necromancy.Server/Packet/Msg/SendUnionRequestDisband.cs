using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestDisband : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendUnionRequestDisband));

        public SendUnionRequestDisband(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_disband;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res, ServerType.Msg);
            Union myUnion = server.instances.GetInstance((uint) client.character.unionId) as Union;

            if (!server.database.DeleteUnion(myUnion.id))
            {
                _Logger.Error($"{myUnion.name} could not be removed from the database");
                return;
            }

            _Logger.Debug(
                $"{myUnion.name} with Id {myUnion.id} and instanceId {myUnion.instanceId} removed and disbanded");
            client.union = null;
        }
    }
}
