using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNoactDead : PacketResponse
    {
        private readonly uint _instancedId;
        private readonly int _deadType;
        private readonly int _lossRegion;
        private readonly int _deathType;


        public RecvBattleReportNoactDead(uint instancedId, int deadType)
            : base((ushort) AreaPacketId.recv_battle_report_noact_notify_dead, ServerType.Area)
        {
            _instancedId = instancedId;
            _deadType = Util.GetRandomNumber(1,3);
            _lossRegion = Util.GetRandomNumber(1,6); 
            _deathType = deadType;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instancedId);
            res.WriteInt32(_deadType); //Death Message Popup :  1 = "You Died", 2 = "You Died", 3 = fainted, beyond that = nothing  . Changes based on DeathType.  
            res.WriteInt32(_lossRegion); //Loss_region :  Death animation to play for other players.   :  0 nothing, 1 head explode, 2 chest explode, 3 chest explode, 4 cut in half, 5 expload, 6 fall over, 
            res.WriteInt32(_deathType); //Death message notify in chat. Changes based on death message popup.  "DeadType"
            return res;
        }
        /*
         * SELF_DEAD	1	%s died! 	 SELF_DEAD
            ENEMY_DEAD	1	%s died! 	 ENEMY_DEAD
            PARTY_DEAD	1	%s died! 	 PARTY_DEAD
            SELF_DEAD	2	%s fainted! 	 SELF_DEAD
            ENEMY_DEAD	2	%s fainted! 	 ENEMY_DEAD
            PARTY_DEAD	2	%s fainted! 	 PARTY_DEAD
            SELF_DEAD	11	%s died instantly! 	 SELF_DEAD
            ENEMY_DEAD	11	%s died instantly! 	 ENEMY_DEAD
            PARTY_DEAD	11	%s died instantly! 	 PARTY_DEAD
            SELF_DEAD	21	%s died and turned into ashes! 	 SELF_DEAD
            ENEMY_DEAD	21	%s died and turned into ashes! 	 ENEMY_DEAD
            PARTY_DEAD	21	%s died and turned into ashes! 	 PARTY_DEAD
            ENEMY_DEAD	101	%s has been destroyed! 	 ENEMY_DEAD
            ENEMY_DEAD	111	%s has been destroyed! 	 ENEMY_DEAD
            ENEMY_DEAD	121	%s has been destroyed! 	 ENEMY_DEAD
            ENEMY_DEAD	201	%s has disappeared! 	 ENEMY_DEAD
            ENEMY_DEAD	 211%s has disappeared! 	 ENEMY_DEAD	
            ENEMY_DEAD	221	%s has disappeared! 	 ENEMY_DEAD
            */
    }
}
