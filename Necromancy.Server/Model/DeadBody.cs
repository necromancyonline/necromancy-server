using System.Collections.Generic;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Model
{
    public class DeadBody : IInstance
    {
        //Inventory
        public Dictionary<ItemEquipSlots, ItemInstance> EquippedItems;

        public DeadBody()
        {
            level = 0;
            connectionState = 1; //0 if disconnected, 1 if dead.
            modelType = 1; //4 if they are an ash pile
            criminalStatus = 0; //We need a criminal status value from original character
            beginnerProtection = 1; // We need a beginner protection value from original character
            deathPose = 1; // We need to send whatever value our character dies with here, 1 = head popped off, 4 = chopped in half (this should come from recv_battle_report_noact_notify_dead)*/
        }

        public uint characterInstanceId { get; set; }
        public int id { get; set; }
        public string charaName { get; set; }
        public string soulName { get; set; }
        public string title { get; set; }
        public int mapId { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public byte heading { get; set; }

        //Basic traits
        public uint raceId { get; set; }
        public uint sexId { get; set; }
        public byte hairId { get; set; }
        public byte hairColorId { get; set; }
        public byte faceId { get; set; }
        public byte faceArrangeId { get; set; }
        public byte voiceId { get; set; }
        public uint classId { get; set; }

        public int connectionState { get; set; }
        public int modelType { get; set; }
        public byte criminalStatus { get; set; }
        public byte beginnerProtection { get; set; }
        public int deathPose { get; set; }
        public int level { get; set; }
        public uint salvagerId { get; set; }
        public uint instanceId { get; set; }
    }
}
