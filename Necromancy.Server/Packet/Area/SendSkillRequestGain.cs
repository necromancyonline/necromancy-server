using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendSkillRequestGain : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSkillRequestGain));

        public SendSkillRequestGain(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_skill_request_gain;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillId = packet.data.ReadInt32(),
                skillLevel = packet.data.ReadInt32();
            //ToDo Add prerequisite checking for new skills
            //ToDo Add passive class specialty skills
            SkillTreeItem skillTreeItem = null;
            if (skillLevel > 1)
            {
                // Should already an entry for this skill
                skillTreeItem = database.SelectSkillTreeItemByCharSkillId(client.character.id, skillId);
                skillTreeItem.level = skillLevel;
                if (database.UpdateSkillTreeItem(skillTreeItem) == false) _Logger.Error($"Updating SkillTreeItem for Character ID [{client.character.id}]");
            }
            else
            {
                skillTreeItem = new SkillTreeItem();
                skillTreeItem.skillId = skillId;
                skillTreeItem.level = skillLevel;
                skillTreeItem.charId = client.character.id;
                if (database.InsertSkillTreeItem(skillTreeItem) == false) _Logger.Error($"Adding SkillTreeItem for Character ID [{client.character.id}]");
            }

            SendSkillTreeGain(client, skillId, skillLevel);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //1 = failed to aquire skill, 0 = success? but no skill aquired
            router.Send(client, (ushort)AreaPacketId.recv_skill_request_gain_r, res, ServerType.Area);
        }

        private void SendSkillTreeGain(NecClient client, int skillId, int skillLevel)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(skillId);
            res.WriteInt32(skillLevel); //Level of skill (1-7)
            res.WriteByte(1); //Bool
            res.WriteByte(1); //Bool

            router.Send(client, (ushort)AreaPacketId.recv_skill_tree_gain, res, ServerType.Area);
        }
    }
}
