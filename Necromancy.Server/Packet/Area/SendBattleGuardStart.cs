using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_start : ClientHandler
    {
        public enum GaurdExceptionType
        {
            OK              = 0, //Gaurd ok
            GPMissing       = 1, //GP is missing, so it cannot be guarded, SYSTEM_WARNING,
            ShieldMissing   = 2, //Cannot guard because it is not equipped with a shield, SYSTEM_WARNING,
            GuardUnable     = 3, //currently unable to guard, SYSTEM_WARNING,
        }
        public send_battle_guard_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_guard_start;


        public override void Handle(NecClient client, NecPacket packet)
        {

            GaurdExceptionType Exception = GaurdExceptionType.ShieldMissing;


            client.Character.EquippedItems.TryGetValue(ItemEquipSlots.LeftHand, out  ItemInstance itemInstance);
            if (itemInstance != null)
            {
                if (itemInstance.Type == ItemType.SHIELD_LARGE | itemInstance.Type == ItemType.SHIELD_MEDIUM | itemInstance.Type == ItemType.SHIELD_SMALL)
                {
                    Exception = GaurdExceptionType.OK;
                }
            }



            if (client.Character.Gp.current < 1)
            { Exception = GaurdExceptionType.GPMissing; }


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32((int)Exception); //If sending a 1, guard fails. Need to come up with logic to make it so people can't block when not using shields.
            Router.Send(client, (ushort)AreaPacketId.recv_battle_guard_start_r, res, ServerType.Area);

            if (Exception != GaurdExceptionType.OK)
            { return; }

            res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_battle_guard_start_self, res, ServerType.Area);

           
            client.Character.AddStateBit(CharacterState.BlockPose);


            res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_battle_guard_start_notify, res, ServerType.Area);
            

        }
    }
}
