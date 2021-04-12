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
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(Character));

        //Task
        public CharacterTask characterTask;
        public BaseStat condition;
        public BaseStat gp;
        public BaseStat hp;
        public BaseStat mp;
        public BaseStat od;
        public BaseStat weight;

        public Character()
        {
            instanceId = InstanceGenerator.InvalidInstanceId;
            id = IDatabase.InvalidDatabaseId;
            accountId = IDatabase.InvalidDatabaseId;
            soulId = IDatabase.InvalidDatabaseId;
            created = DateTime.Now;
            mapId = IDatabase.InvalidDatabaseId;
            x = 0;
            y = 0;
            z = 0;
            slot = 0;
            name = null;
            level = 0;
            activeModel = 0;
            deadType = 0;
            modelScale = 100;
            adventureBagGold = 0;
            experienceCurrent = 0;
            skillPoints = 0;
            eventSelectExecCode = -1;
            hp = new BaseStat(10, 10);
            mp = new BaseStat(450, 500);
            od = new BaseStat(150, 200);
            gp = new BaseStat(0, 0);
            weight = new BaseStat(456, 1234);
            condition = new BaseStat(140, 200);
            takeover = false;
            skillStartCast = 0;
            battleAnim = 0;
            hasDied = false;
            state = CharacterState.NormalForm;
            helperText = true;
            helperTextBlacksmith = true;
            helperTextDonkey = true;
            helperTextCloakRoom = true;
            beginnerProtection = 1;
            currentEvent = null;
            secondInnAccess = false;
            isCharacterActive = true;
            secondInnAccess = false;
            partyId = 0;
            instanceId = 0;
            name = "";
            classId = 0;
            unionId = 0;
            faceArrangeId = 0;
            voiceId = 0;
            criminalState = 0;
            helperTextAbdul = true;
            mapChange = false;
            stepCount = 0;
            lootNotify = new ItemLocation(0, 0, 0);
            odRecoveryRate = 0;
            statusEffects = new uint[4]
            {
                (uint)Statuses.AttackAura405,
                (uint)Statuses.MosquitoBuzz200,
                (uint)Statuses.PorkulCakeWhole,
                (uint)Statuses.ChimeraKillerHotMode
            };
            tradeWindowSlot = new ulong[20];
        }

        //core attributes
        public int id { get; set; } //TODO at some point make a uint
        public int accountId { get; set; }
        public int soulId { get; set; }
        public DateTime created { get; set; }
        public byte slot { get; set; }
        public string name { get; set; }
        public byte level { get; set; }


        //Basic traits
        public uint raceId { get; set; }
        public uint sexId { get; set; }
        public byte hairId { get; set; }
        public byte hairColorId { get; set; }
        public byte faceId { get; set; }
        public uint classId { get; set; }
        public byte faceArrangeId { get; set; }
        public byte voiceId { get; set; }


        //Stats
        public ushort strength { get; set; }
        public ushort vitality { get; set; }
        public ushort dexterity { get; set; }
        public ushort agility { get; set; }
        public ushort intelligence { get; set; }
        public ushort piety { get; set; }
        public ushort luck { get; set; }
        public short odRecoveryRate { get; set; }
        public short hpRecoveryRate { get; set; }
        public short mpRecoveryRate { get; set; }
        public BattleParam battleParam { get; set; }

        //Progression
        public ulong experienceCurrent { get; set; }
        public uint skillPoints { get; set; }


        //Model
        public int activeModel { get; set; }
        public short modelScale { get; set; }
        public bool hasDied { get; set; }
        public short deadType { get; set; }
        public uint deadBodyInstanceId { get; set; }
        public CharacterState state { get; set; }
        public byte soulFormState { get; set; }
        public byte criminalState { get; set; }
        public int beginnerProtection { get; set; }
        public uint activeSkillInstance { get; set; }
        public bool castingSkill { get; set; }
        public int skillStartCast { get; set; }
        public uint partyId { get; set; }
        public int unionId { get; set; }

        //Movement Related
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public byte heading { get; set; }
        public byte battleAnim { get; set; }
        public byte battleNext { get; set; }
        public int charaPose { get; set; }
        public byte movementPose { get; set; }
        public byte movementAnim { get; set; }
        public int stepCount { get; set; }
        public bool takeover { get; set; }


        //Map Related
        public int mapId { get; set; }
        public bool mapChange { get; set; }
        public int channel { get; set; }


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
        public bool isCharacterActive { get; private set; }

        //Inventory
        public ItemLocationVerifier itemLocationVerifier { get; } = new ItemLocationVerifier(); //TODO make item service
        public Dictionary<ItemEquipSlots, ItemInstance> equippedItems { get; } = new Dictionary<ItemEquipSlots, ItemInstance>(); //TODO temp crap this is not the equipment system.

        /// <summary>
        ///     Used to hold the ids of the items in the auction search window temporarily.
        /// </summary>
        public ulong[] auctionSearchIds { get; set; } = new ulong[0];

        public ItemLocation lootNotify { get; set; }
        public ulong adventureBagGold { get; set; }
        public ulong[] tradeWindowSlot { get; set; }

        //Statues
        public uint[] statusEffects { get; set; }

        public bool characterActive
        {
            get => isCharacterActive;
            set => isCharacterActive = value;
        }

        public uint instanceId { get; set; }

        public void CreateTask(NecServer server, NecClient client)
        {
            characterTask = new CharacterTask(server, client);
            characterTask.Start();
        }

        public void AddStateBit(CharacterState characterState)
        {
            state |= characterState;
        }

        public void ClearStateBit(CharacterState characterState)
        {
            state &= ~characterState;
        }

        public bool IsStealthed()
        {
            return state.HasFlag(CharacterState.StealthForm);
        }

        public void ConditionBonus()
        {
            if (condition.current > 180) odRecoveryRate = 16; //+8 to all stats
            else if (condition.current > 140) odRecoveryRate = 8; //+4 to all stats
            else if (condition.current > 40) odRecoveryRate = 4; //+0
            else if (condition.current > 20) odRecoveryRate = 2; //-2 to all stats
            else odRecoveryRate = 2; // -4 to all stats //should be 1 recovery rate, but our 500ms tick reduces to 0
        }

        public void LoginCheckDead() //todo,  further analysis on character states and poses. eliminate this HP based overide
        {
            if (hp.current <= 0)
            {
                hasDied = true;
                state = CharacterState.SoulForm;
                deadType = 1;
            }

            if (hp.current == -1)
                deadType = 4;
            else if (hp.current < -1) deadType = 5;
        }
    }
}
