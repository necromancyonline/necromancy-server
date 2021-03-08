using System;
using System.Collections.Generic;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Model
{
    public class DeadBody : IInstance
    {
        public uint InstanceId { get; set; }
        public uint CharacterInstanceId { get; set; }
        public int Id { get; set; }
        public string CharaName { get; set; }
        public string SoulName { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        //Basic traits
        public uint RaceId { get; set; }
        public uint SexId { get; set; }
        public byte HairId { get; set; }
        public byte HairColorId { get; set; }
        public byte FaceId { get; set; }
        public byte FaceArrangeId { get; set; }
        public byte VoiceId { get; set; }
        public uint ClassId { get; set; }

        public int ConnectionState { get; set; }
        public int ModelType { get; set; }
        public byte CriminalStatus { get; set; }
        public byte BeginnerProtection { get; set; }
        public int deathPose { get; set; }
        public int Level { get; set; }
        public uint SalvagerId { get; set; }

        //Inventory
        public ItemManager ItemManager { get; set; } = new ItemManager(); //TODO make item service
        public Dictionary<ItemEquipSlots, ItemInstance> EquippedItems;

        public DeadBody()
        {
            Level = 0;
            ConnectionState = 1;//0 if disconnected, 1 if dead.
            ModelType = 1; //4 if they are an ash pile
            CriminalStatus = 0; //We need a criminal status value from original character
            BeginnerProtection = 1; // We need a beginner protection value from original character
            deathPose = 1; // We need to send whatever value our character dies with here, 1 = head popped off, 4 = chopped in half (this should come from recv_battle_report_noact_notify_dead)*/
        }
    }
}
