using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class SendSkillRequestInfo : ClientHandler
    {
        public SendSkillRequestInfo(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_skill_request_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            List <SkillTreeItem> skillTreeItems = database.SelectSkillTreeItemsByCharId(client.character.id);
            for (int i = 0; i < skillTreeItems.Count; i++)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(skillTreeItems[i].skillId);      // base skill id
                res.WriteInt32(skillTreeItems[i].level);          // skill level
                res.WriteByte(0); //bool    "Is_takeover"
                res.WriteByte(0); //bool    "is_Custom_Set"

                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new

                // No Response OP code
                router.Send(client, (ushort)AreaPacketId.recv_skill_tree_notify, res, ServerType.Area);
            }
        }
    }
}
