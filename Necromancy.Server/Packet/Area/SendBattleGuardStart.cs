using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleGuardStart : ClientHandler
    {
        public enum GaurdExceptionType
        {
            Ok = 0, //Gaurd ok
            GpMissing = 1, //GP is missing, so it cannot be guarded, SYSTEM_WARNING,
            ShieldMissing = 2, //Cannot guard because it is not equipped with a shield, SYSTEM_WARNING,
            GuardUnable = 3 //currently unable to guard, SYSTEM_WARNING,
        }

        public SendBattleGuardStart(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_battle_guard_start;


        public override void Handle(NecClient client, NecPacket packet)
        {
            GaurdExceptionType exception = GaurdExceptionType.ShieldMissing;


            client.character.equippedItems.TryGetValue(ItemEquipSlots.LeftHand, out ItemInstance itemInstance);
            if (itemInstance != null)
                if ((itemInstance.type == ItemType.SHIELD_LARGE) | (itemInstance.type == ItemType.SHIELD_MEDIUM) | (itemInstance.type == ItemType.SHIELD_SMALL))
                    exception = GaurdExceptionType.Ok;


            if (client.character.Gp.current < 1) exception = GaurdExceptionType.GpMissing;


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32((int)exception); //If sending a 1, guard fails. Need to come up with logic to make it so people can't block when not using shields.
            router.Send(client, (ushort)AreaPacketId.recv_battle_guard_start_r, res, ServerType.Area);

            if (exception != GaurdExceptionType.Ok) return;

            res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_battle_guard_start_self, res, ServerType.Area);


            client.character.AddStateBit(CharacterState.BlockPose);


            res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            router.Send(client.map, (ushort)AreaPacketId.recv_dbg_battle_guard_start_notify, res, ServerType.Area);
        }
    }
}
