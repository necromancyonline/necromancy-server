namespace Necromancy.Server.Data.Setting
{
    public class CharacterAttackSetting : ISettingRepositoryItem
    {
        public int motionId { get; set; }
        public string weapon { get; set; }
        public bool firstShot { get; set; }
        public int nextAttackId { get; set; }
        public int channel { get; set; }
        public int moveStart { get; set; }
        public int moveEnd { get; set; }
        public int moveAmount { get; set; }
        public int swordShadowStart { get; set; }
        public int swordShadowEnd { get; set; }
        public int socket1Type { get; set; }
        public int fx1Id { get; set; }
        public int socket2Type { get; set; }
        public int fx2Id { get; set; }
        public int interruptStart { get; set; }
        public int interruptEnd { get; set; }
        public int rigidTime { get; set; }
        public int inputReception { get; set; }
        public int hit { get; set; }
        public int guardCanel { get; set; }
        public int attackAtariStart { get; set; }
        public int attackAtariEnd { get; set; }
        public float consecutiveAttackStart { get; set; }
        public float continuousAttackEnd { get; set; }
        public float delay { get; set; }
        public float rigidity { get; set; }
        public bool reuse { get; set; }
        public int id { get; set; }
    }
}
