using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_data : ClientHandler
    {
        public send_shortcut_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_request_data_r, res, ServerType.Area);

            // Need to find definitions for different action types as Ids do overlap,  0 ?, 1 ?, 2 ? , 3 for skill_tree item, 4 for system , 5 for emote
            // Should we have a shortcutBarItem class?
            const int MAX_SHORTCUT_BARS = 5;
            for (int i = 0; i < MAX_SHORTCUT_BARS; i++)
            {
                ShortcutBar shortcutBar = Database.GetShortcutBar(client.Character, i);
                for (int j = 0; j < ShortcutBar.COUNT; j++)
                {
                    if (shortcutBar.Item[j] is null) continue;
                    IBuffer res0 = BufferProvider.Provide();
                    res0.WriteByte((byte) i);
                    res0.WriteByte((byte) j);
                    res0.WriteInt32((int) shortcutBar.Item[j].Type);
                    res0.WriteInt64(shortcutBar.Item[j].Id);       // SkillId from skill_tree.csv for class skills
                    res0.WriteFixedString("SkillName", 16); //size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res0, ServerType.Area);
                }
            }           
        }
    }
}
