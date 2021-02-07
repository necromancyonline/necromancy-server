using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Database;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Systems.Item;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class Character : IInstance
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Character));

        public uint InstanceId { get; set; }

        //core attributes
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }
        public byte Slot { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }

        public CharacterState State { get; set; }

        // TODO sort this messy class out...
        
        //Basic traits
        public uint RaceId { get; set; }
        public uint SexId { get; set; }
        public byte HairId { get; set; }
        public byte HairColorId { get; set; }
        public byte FaceId { get; set; }
        public uint ClassId { get; set; }


        //Stats
        public uint AlignmentId { get; set; }
        public ushort Strength { get; set; }
        public ushort Vitality { get; set; }
        public ushort Dexterity { get; set; }
        public ushort Agility { get; set; }
        public ushort Intelligence { get; set; }
        public ushort Piety { get; set; }
        public ushort Luck { get; set; }
        public BaseStat Hp;
        public BaseStat Mp;
        public BaseStat Od;
        public BaseStat Gp;

        //Model
        public int activeModel;
        public short modelScale;
        public short deadType;


        //Movement Related
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public byte battlePose { get; set; }
        public byte battleAnim { get; set; }
        public byte battleNext { get; set; }
        public int charaPose { get; set; }
        public byte movementPose { get; set; }
        public byte movementAnim { get; set; }
        public bool weaponEquipped { get; set; }
        public uint movementId { get; set; }
        public bool mapChange { get; set; }

        //Normal Attack
        public int[] AttackIds { get; set; }

        //Map Related
        public int MapId { get; set; }

        //Temporary Value Holders
        public int StepCount { get; set; }
        public long AdventureBagGold { get; set; }
        public byte soulFormState { get; set; }
        public int[] EquipId { get; set; }
        public uint activeSkillInstance { get; set; }
        public bool castingSkill { get; set; }
        public uint eventSelectReadyCode { get; set; }
        public int eventSelectExecCode { get; set; }
        public int eventSelectExtraSelectionCode { get; set; }
        public bool takeover { get; set; }
        public int skillStartCast { get; set; }
        public bool helperText { get; set; }
        public bool helperTextBlacksmith { get; set; }
        public bool helperTextDonkey { get; set; }
        public bool helperTextCloakRoom { get; set; }
        public bool helperTextAbdul { get; set; }
        public Event currentEvent { get; set; }
        public bool secondInnAccess { get; set; }

        public uint killerInstanceId { get; private set; }
        public bool HasDied { get; set; }
        public uint DeadBodyInstanceId { get; set; }
        public int Channel { get; set; }
        public int beginnerProtection { get; set; }


        //public bool playerDead { get; set; }
        public uint partyId { get; set; }
        public int unionId { get; set; }
        public byte criminalState { get; set; }

        //Msg Value Holders
        public uint friendRequest { get; set; }
        public uint partyRequest { get; set; }

        //Task
        public CharacterTask characterTask;
        public bool _characterActive { get; private set; }

        //Inventory
        public ItemManager ItemManager { get; } = new ItemManager(); //TODO make item service
        public Dictionary<ItemEquipSlots, ItemInstance> EquippedItems {get;  } = new Dictionary<ItemEquipSlots, ItemInstance>(); //TODO temp crap this is not the equipment system.

    public Character()
        {
            InstanceId = InstanceGenerator.InvalidInstanceId;
            Id = IDatabase.InvalidDatabaseId;
            AccountId = IDatabase.InvalidDatabaseId;
            SoulId = IDatabase.InvalidDatabaseId;
            Created = DateTime.Now;
            MapId = IDatabase.InvalidDatabaseId;
            X = 0;
            Y = 0;
            Z = 0;
            Slot = 0;
            Name = null;
            Level = 0;
            activeModel = 0;
            deadType = 0;
            modelScale = 100;
            AdventureBagGold = 80706050;
            eventSelectExecCode = -1;
            Hp = new BaseStat(1000, 1000);
            Mp = new BaseStat(450, 500);
            Od = new BaseStat(150, 200);
            Gp = new BaseStat(50, 50);
            takeover = false;
            skillStartCast = 0;
            battleAnim = 0;
            HasDied = false;
            State = CharacterState.NormalForm;
            helperText = true;
            helperTextBlacksmith = true;
            helperTextDonkey = true;
            helperTextCloakRoom = true;
            beginnerProtection = 1;
            currentEvent = null;
            secondInnAccess = false;
            _characterActive = true;
            killerInstanceId = 0;
            secondInnAccess = false;
            partyId = 0;
            InstanceId = 0;
            Name = "";
            ClassId = 0;
            unionId = 0;
            criminalState = 0;
            helperTextAbdul = true;
            mapChange = false;
            StepCount = 0;
        }

        public bool characterActive
        {
            get => _characterActive;
            set { _characterActive = value; }
        }

        public void CreateTask(NecServer server, NecClient client)
        {
            characterTask = new CharacterTask(server, client);
            characterTask.Start();
        }

        public void AddStateBit(CharacterState characterState)
        {
            State |= characterState;
        }

        public void ClearStateBit(CharacterState characterState)
        {
            State &= ~characterState;
        }

        public bool IsStealthed()
        {
            return State.HasFlag(CharacterState.StealthForm);
        }
    }
}
