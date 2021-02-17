using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_access_start : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_charabody_access_start));
        public send_charabody_access_start(NecServer server) : base(server)
        {
            //ToDo :   If TargetId = Self.InstanceID,  warp to res statue.   if TargetId = Party member, Collect body.   else,  Loot
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_access_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint bodyId = packet.Data.ReadUInt32();
            DeadBody deadBody = Server.Instances.GetInstance(bodyId) as DeadBody; //add case logic to detect different instance types.  monster, deadbody, other
            Logger.Debug($"Accessing Body ID {bodyId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_access_start_r, res, ServerType.Area);

            foreach (ItemInstance itemInstance in deadBody.ItemManager.GetLootableItems())
            {
                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance, (byte)ItemZoneType.BodyCollection);
                Router.Send(client, recvItemInstanceUnidentified.ToPacket());
            }

        }
    }
}
