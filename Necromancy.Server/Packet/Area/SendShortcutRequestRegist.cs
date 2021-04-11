using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_regist : ClientHandler
    {
        public send_shortcut_request_regist(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_regist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            byte shortcutBarIdx = packet.Data.ReadByte(),
                 slot = packet.Data.ReadByte();
            int actionType = packet.Data.ReadInt32();
            long skillID = packet.Data.ReadInt64();

            ShortcutItem shortcutItem = new ShortcutItem(skillID, (ShortcutItem.ShortcutType)actionType);
            Database.InsertOrReplaceShortcutItem(client.Character, shortcutBarIdx, slot, shortcutItem);

            res.WriteByte(shortcutBarIdx);
            res.WriteByte(slot);
            res.WriteInt32(actionType);
            res.WriteInt64(skillID);
            res.WriteFixedString("SkillName", 16);//size is 0x10

            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res, ServerType.Area);
        }
    }
}
