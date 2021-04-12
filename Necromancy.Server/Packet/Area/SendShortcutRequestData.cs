using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendShortcutRequestData : ClientHandler
    {
        public SendShortcutRequestData(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_shortcut_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_shortcut_request_data_r, res, ServerType.Area);

            // Need to find definitions for different action types as Ids do overlap,  0 ?, 1 ?, 2 ? , 3 for skill_tree item, 4 for system , 5 for emote
            // Should we have a shortcutBarItem class?
            const int MaxShortcutBars = 5;
            for (int i = 0; i < MaxShortcutBars; i++)
            {
                ShortcutBar shortcutBar = database.GetShortcutBar(client.character, i);
                for (int j = 0; j < ShortcutBar.Count; j++)
                {
                    if (shortcutBar.item[j] is null) continue;
                    IBuffer res0 = BufferProvider.Provide();
                    res0.WriteByte((byte)i);
                    res0.WriteByte((byte)j);
                    res0.WriteInt32((int)shortcutBar.item[j].type);
                    res0.WriteInt64(shortcutBar.item[j].id); // SkillId from skill_tree.csv for class skills
                    res0.WriteFixedString("SkillName", 16); //size is 0x10
                    router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res0, ServerType.Area);
                }
            }
        }
    }
}
