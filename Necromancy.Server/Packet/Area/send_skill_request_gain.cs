using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_gain : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_skill_request_gain));

        public send_skill_request_gain(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_gain;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillID = packet.Data.ReadInt32(),
                skillLevel = packet.Data.ReadInt32();
            //ToDo Add prerequisite checking for new skills
            //ToDo Add passive class specialty skills
            SkillTreeItem skillTreeItem = null;
            if (skillLevel > 1)
            {
                // Should already an entry for this skill
                skillTreeItem = Database.SelectSkillTreeItemByCharSkillId(client.Character.Id, skillID);
                skillTreeItem.Level = skillLevel;
                if (Database.UpdateSkillTreeItem(skillTreeItem) == false)
                {
                    Logger.Error($"Updating SkillTreeItem for Character ID [{client.Character.Id}]");
                }
            }
            else
            {
                skillTreeItem = new SkillTreeItem();
                skillTreeItem.SkillId = skillID;
                skillTreeItem.Level = skillLevel;
                skillTreeItem.CharId = client.Character.Id;
                if (Database.InsertSkillTreeItem(skillTreeItem) == false)
                {
                    Logger.Error($"Adding SkillTreeItem for Character ID [{client.Character.Id}]");
                }
            }

            SendSkillTreeGain(client, skillID, skillLevel);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //1 = failed to aquire skill, 0 = success? but no skill aquired 
            Router.Send(client, (ushort) AreaPacketId.recv_skill_request_gain_r, res, ServerType.Area);
        }

        private void SendSkillTreeGain(NecClient client, int skillID, int skillLevel)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(skillID);
            res.WriteInt32(skillLevel); //Level of skill (1-7)
            res.WriteByte(1); //Bool
            res.WriteByte(1); //Bool

            Router.Send(client, (ushort)AreaPacketId.recv_skill_tree_gain, res, ServerType.Area);
        }
    }
}
