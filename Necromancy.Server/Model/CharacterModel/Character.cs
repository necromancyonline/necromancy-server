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
        public int Id { get; set; } //TODO at some point make a uint
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }
        public byte Slot { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }


        //Basic traits
        public uint RaceId { get; set; }
        public uint SexId { get; set; }
        public byte HairId { get; set; }
        public byte HairColorId { get; set; }
        public byte FaceId { get; set; }
        public uint ClassId { get; set; }
        public byte FaceArrangeId { get; set; }
        public byte VoiceId { get; set; }


        //Stats
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
        public BaseStat Weight;
        public BaseStat Condition;
        public short OdRecoveryRate { get; set; }
        public short HpRecoveryRate { get; set; }
        public short MpRecoveryRate { get; set; }
        public BattleParam battleParam { get; set; }

        //Progression
        public ulong ExperienceCurrent { get; set; }
        public uint SkillPoints { get; set; }


        //Model
        public int activeModel { get; set; }
        public short modelScale { get; set; }
        public bool HasDied { get; set; }
        public short deadType { get; set; }
        public uint DeadBodyInstanceId { get; set; }
        public CharacterState State { get; set; }
        public byte soulFormState { get; set; }
        public byte criminalState { get; set; }
        public int beginnerProtection { get; set; }
        public uint activeSkillInstance { get; set; }
        public bool castingSkill { get; set; }
        public int skillStartCast { get; set; }
        public uint partyId { get; set; }
        public int unionId { get; set; }

        //Movement Related
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public byte battleAnim { get; set; }
        public byte battleNext { get; set; }
        public int charaPose { get; set; }
        public byte movementPose { get; set; }
        public byte movementAnim { get; set; }
        public int StepCount { get; set; }
        public bool takeover { get; set; }


        //Map Related
        public int MapId { get; set; }
        public bool mapChange { get; set; }
        public int Channel { get; set; }


        //Event helpers
        public uint eventSelectReadyCode { get; set; }
        public int eventSelectExecCode { get; set; }
        public int eventSelectExtraSelectionCode { get; set; }
        public int[] shopItemIndex { get; set; }
        public bool helperText { get; set; }
        public bool helperTextBlacksmith { get; set; }
        public bool helperTextDonkey { get; set; }
        public bool helperTextCloakRoom { get; set; }
        public bool helperTextAbdul { get; set; }
        public Event currentEvent { get; set; }
        public bool secondInnAccess { get; set; }


        //Msg Value Holders
        public uint friendRequest { get; set; }
        public uint partyRequest { get; set; }

        //Task
        public CharacterTask characterTask;
        public bool _characterActive { get; private set; }

        //Inventory
        public ItemLocationVerifier ItemLocationVerifier { get; } = new ItemLocationVerifier(); //TODO make item service
        public Dictionary<ItemEquipSlots, ItemInstance> EquippedItems { get; } = new Dictionary<ItemEquipSlots, ItemInstance>(); //TODO temp crap this is not the equipment system.

        /// <summary>
        /// Used to hold the ids of the items in the auction search window temporarily.
        /// </summary>
        public ulong[] AuctionSearchIds { get; set; } = new ulong[0];
        public ItemLocation lootNotify { get; set; }
        public ulong AdventureBagGold { get; set; }

        //Statues
        public uint[] StatusEffects { get; set; }

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
            AdventureBagGold = 0;
            ExperienceCurrent = 0;
            SkillPoints = 0;
            eventSelectExecCode = -1;
            Hp = new BaseStat(10, 10);
            Mp = new BaseStat(450, 500);
            Od = new BaseStat(150, 200);
            Gp = new BaseStat(0, 0);
            Weight = new BaseStat(456, 1234);
            Condition = new BaseStat(140, 200);
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
            secondInnAccess = false;
            partyId = 0;
            InstanceId = 0;
            Name = "";
            ClassId = 0;
            unionId = 0;
            FaceArrangeId = 0;
            VoiceId = 0;
            criminalState = 0;
            helperTextAbdul = true;
            mapChange = false;
            StepCount = 0;
            lootNotify = new ItemLocation((ItemZoneType)0, 0, 0);
            OdRecoveryRate = 0;
            StatusEffects = new uint[4]
            {
                (uint)Statuses.Attack_Aura405,
                (uint)Statuses.Mosquito_Buzz200,
                (uint)Statuses.Porkul_Cake_Whole,
                (uint)Statuses._Chimera_Killer_Hot_Mode
            };
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

        public void ConditionBonus()
        {
            if (this.Condition.current > 180) this.OdRecoveryRate = 16; //+8 to all stats
            else if (this.Condition.current > 140) this.OdRecoveryRate = 8; //+4 to all stats
            else if (this.Condition.current > 40) this.OdRecoveryRate = 4; //+0 
            else if (this.Condition.current > 20) this.OdRecoveryRate = 2; //-2 to all stats
            else this.OdRecoveryRate = 2; // -4 to all stats //should be 1 recovery rate, but our 500ms tick reduces to 0
        }
        public void LoginCheckDead() //todo,  further analysis on character states and poses. eliminate this HP based overide
        {
            if (this.Hp.current <= 0)
            {
                this.HasDied = true;
                this.State = CharacterState.SoulForm;
                this.deadType = 1;
            }
            if (this.Hp.current == -1)
            {
                this.deadType = 4;
            }
            else if (this.Hp.current < -1)
            {
                this.deadType = 5;
            }
        }
    }
}
