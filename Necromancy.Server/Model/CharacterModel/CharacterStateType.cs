using System;

namespace Necromancy.Server.Model.CharacterModel
{
    [Flags]
    public enum CharacterState : uint
    {
        // CharacterState             // Binary                           Dec
        InvalidState = 0, // 0000 0000 0000 0000 0000 0000    0
        SoulForm = 1 << 0, // 0000 0000 0000 0000 0000 0001    1
        BlockPose = 1 << 1, // 0000 0000 0000 0000 0000 0010    2
        StealthForm = 1 << 2, // 0000 0000 0000 0000 0000 0100    4
        NothingForm = 1 << 3, // 0000 0000 0000 0000 0000 1000    8
        NormalForm = 1 << 4, // 0000 0000 0000 0000 0001 0000    16
        InvisibleForm = 1 << 5, // 0000 0000 0000 0000 0010 0000    32 
        InvulnerableForm = 1 << 6, // 0000 0000 0000 0000 0100 0000    64 
        GameMaster = 1 << 12, // 0000 0000 0001 0000 0000 0000    4096 
        RequestPartyJoin = 1 << 13, // 0000 0000 0010 0000 0000 0000    8192 
        RecruitPartyMember = 1 << 14, // 0000 0000 0100 0000 0000 0000    16384 
        LostState = 1 << 15, // 0000 0000 1000 0000 0000 0000    32768
        HeadState = 1 << 16, // 0000 0001 0000 0000 0000 0000    65536
        MemberBonus = 1 << 20 // 0001 0000 0000 0000 0000 0000‬    1048576
    }
}
