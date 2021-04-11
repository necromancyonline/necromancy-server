using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendEquipHonor : ClientHandler
    {
        public SendEquipHonor(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_equip_honor;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int honorTitleId = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            //todo   if(client.Character.State == characterState.Lost) {ErrMsg = 2);

            res.WriteInt32(0);
            /*
            HONOR,  0,  "Equipped the title ""%s""",SYSTEM_NOTIFY,
            HONOR,      GENERIC,Unable to equip title,SYSTEM_WARNING,
            HONOR,  2,  You may not change your equipped title while your character is lost,SYSTEM_WARNING,
             */

            router.Send(client, (ushort) AreaPacketId.recv_equip_honor_r, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            res.WriteInt32(honorTitleId);

            router.Send(client.map, (ushort)AreaPacketId.recv_chara_update_notify_honor, res, ServerType.Area);
        }
    }
}
