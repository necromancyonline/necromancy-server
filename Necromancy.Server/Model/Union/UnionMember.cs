using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model.Union
{
    public class UnionMember : IInstance
    {
        public UnionMember()
        {
            instanceId = 0;
            id = -1;
            unionId = 0;
            characterDatabaseId = 1;
            //CharacterInstanceId = 0;
            memberPriviledgeBitMask = 0b01100111;
            rank = 3;
            joined = DateTime.Now;
        }

        public int id { get; set; }
        public int unionId { get; set; }

        public int characterDatabaseId { get; set; }

        // public uint CharacterInstanceId { get; set; }
        public uint memberPriviledgeBitMask { get; set; }
        public uint rank { get; set; }

        public DateTime joined { get; set; }
        public uint instanceId { get; set; }
    }
}
