using System;
using System.Collections.Generic;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model.Union
{
    public class Union : IInstance
    {
        public Union()
        {
            unionMembers = new List<NecClient>();
            id = -1;
            name = "";
            leaderId = 0;
            level = 0;
            currentExp = 0;
            nextLevelExp = 100;
            memberLimitIncrease = 0;
            capeDesignId = 0;
            created = DateTime.Now;
        }

        public int id { get; set; }
        public string name { get; set; }
        public int leaderId { get; set; }
        public int subLeader1Id { get; set; }
        public int subLeader2Id { get; set; }
        public uint level { get; set; }
        public uint currentExp { get; set; }
        public uint nextLevelExp { get; set; }
        public byte memberLimitIncrease { get; set; }
        public short capeDesignId { get; set; }
        public string unionNews { get; set; }
        public DateTime created { get; set; }

        public List<NecClient> unionMembers { get; set; }
        public List<UnionNewsEntry> unionNewsEntries { get; set; }
        public uint instanceId { get; set; }

        public void Join(NecClient client) //for establish and join
        {
            unionMembers.Add(client);
        }

        public void Leave(NecClient client) //for Kick and succeed
        {
            unionMembers.Remove(client); //to-do  try/catch
        }

        public void Leave(List<NecClient> unionMembers) //for disband
        {
            foreach (NecClient client in unionMembers)
                unionMembers.Remove(client); //to-do  try/catch
        }

        public void AddNews(UnionNewsEntry addEntry) //for adding news
        {
            unionNewsEntries.Add(addEntry);
        }

        public void RemoveNews(UnionNewsEntry removeEntry) //for adding news
        {
            unionNewsEntries.Remove(removeEntry);
        }
    }
}
