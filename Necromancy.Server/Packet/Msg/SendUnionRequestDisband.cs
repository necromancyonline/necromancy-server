using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_disband : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_request_disband));

        public send_union_request_disband(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_disband;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res, ServerType.Msg);
            Union myUnion = Server.Instances.GetInstance((uint) client.Character.unionId) as Union;

            if (!Server.Database.DeleteUnion(myUnion.Id))
            {
                Logger.Error($"{myUnion.Name} could not be removed from the database");
                return;
            }

            Logger.Debug(
                $"{myUnion.Name} with Id {myUnion.Id} and instanceId {myUnion.InstanceId} removed and disbanded");
            client.Union = null;
        }
    }
}
