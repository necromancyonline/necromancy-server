using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_info : ClientHandler
    {
        public send_skill_request_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            List <SkillTreeItem> skillTreeItems = Database.SelectSkillTreeItemsByCharId(client.Character.Id);
            for (int i = 0; i < skillTreeItems.Count; i++)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(skillTreeItems[i].SkillId);      // base skill id
                res.WriteInt32(skillTreeItems[i].Level);          // skill level
                res.WriteByte(0); //bool    "Is_takeover"
                res.WriteByte(0); //bool    "is_Custom_Set"

                res.WriteInt32(0);//new
                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new

                // No Response OP code
                Router.Send(client, (ushort)AreaPacketId.recv_skill_tree_notify, res, ServerType.Area);
            }
        }
    }
}
