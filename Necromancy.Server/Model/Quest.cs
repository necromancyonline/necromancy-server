using System;

namespace Necromancy.Server.Model
{
    public class Quest
    {
        public int questId { get; set; }
        public Byte soulLevelMission { get; set; }
        public string questName { get; set; }
        public int questLevel { get; set; }
        public int timeLimit { get; set; }
        public string questGiverName { get; set; }
        public int rewardExp { get; set; }
        public int rewardGold { get; set; }
        public Int16 numbersOfItems { get; set; }
        public int itemsType { get; set; }
        public Quest()
        {

        }
    }

}
