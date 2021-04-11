using System;

namespace Necromancy.Server.Model
{
    public class Account
    {
        public int id { get; set; }
        public string name { get; set; }
        public string normalName { get; set; }
        public string hash { get; set; }
        public string mail { get; set; }
        public string mailToken { get; set; }
        public string passwordToken { get; set; }
        public bool mailVerified { get; set; }
        public DateTime? mailVerifiedAt { get; set; }
        public AccountStateType state { get; set; }
        public DateTime? lastLogin { get; set; }
        public DateTime created { get; set; }

        public Account()
        {
            id = -1;
        }
    }
}
