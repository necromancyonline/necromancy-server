using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Model.Skills
{
    public class Trap : IInstance
    {
        public Trap(int skillBase, SkillBaseSetting skillBaseSetting, EoBaseSetting eoBaseSetting, EoBaseSetting eoBaseSettingTriggered)
        {
            skillBaseId = skillBase;
            skillId = skillBaseSetting.id;
            name = skillBaseSetting.name;
            triggerRadius = eoBaseSetting.effectRadius;
            effectRadius = eoBaseSettingTriggered.effectRadius;
            itemType = skillBaseSetting.item1Id;
            itemCount = skillBaseSetting.item1Count;
            castTime = skillBaseSetting.castingTime;
            trapTime = skillBaseSetting.effectTime;
            skillEffectId = eoBaseSetting.id;
            triggerEffectId = eoBaseSettingTriggered.id;
            coolTime = skillBaseSetting.castingCooldown;
        }

        public string name { get; set; }
        public int skillId { get; set; }
        public int skillBaseId { get; set; }
        public int triggerRadius { get; set; }
        public int effectRadius { get; set; }
        public int itemType { get; set; }
        public int itemCount { get; set; }
        public float castTime { get; set; }
        public float trapTime { get; set; }
        public int skillEffectId { get; set; }
        public int triggerEffectId { get; set; }
        public float coolTime { get; set; }
        public uint instanceId { get; set; }
    }
}
