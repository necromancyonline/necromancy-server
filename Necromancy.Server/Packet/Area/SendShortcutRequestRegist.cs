using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendShortcutRequestRegist : ClientHandler
    {
        public SendShortcutRequestRegist(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_shortcut_request_regist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            byte shortcutBarIdx = packet.data.ReadByte(),
                slot = packet.data.ReadByte();
            int actionType = packet.data.ReadInt32();
            long skillId = packet.data.ReadInt64();

            ShortcutItem shortcutItem = new ShortcutItem(skillId, (ShortcutItem.ShortcutType)actionType);
            database.InsertOrReplaceShortcutItem(client.character, shortcutBarIdx, slot, shortcutItem);

            res.WriteByte(shortcutBarIdx);
            res.WriteByte(slot);
            res.WriteInt32(actionType);
            res.WriteInt64(skillId);
            res.WriteFixedString("SkillName", 16); //size is 0x10

            router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res, ServerType.Area);
        }
    }
}
