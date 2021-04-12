using System.Collections.Generic;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Party : IInstance
    {
        public Party()
        {
            partyMembers = new List<NecClient>();
            partyLeaderId = 0;
            normalItemDist = 1;
            rareItemDist = 1;
            targetClientId = 0;
        }

        public int partyType { get; set; }
        public int normalItemDist { get; set; }
        public int rareItemDist { get; set; }
        public uint targetClientId { get; set; }
        public uint partyLeaderId { get; set; }
        public List<NecClient> partyMembers { get; set; }
        public uint instanceId { get; set; }

        public void Join(NecClient client)
        {
            partyMembers.Add(client);
        }

        public void Leave(NecClient client)
        {
            partyMembers.Remove(client); //to-do  try/catch
        }

        public void Leave(List<NecClient> partyMembers)
        {
            foreach (NecClient client in partyMembers)
                partyMembers.Remove(client); //to-do  try/catch
        }
    }
}
