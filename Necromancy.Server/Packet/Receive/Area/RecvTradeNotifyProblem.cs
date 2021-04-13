using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyProblem : PacketResponse
    {
        private readonly uint _objectId;
        private readonly byte _systemMessage;

        public RecvTradeNotifyProblem(uint objectId, byte systemMessage)
            : base((ushort)AreaPacketId.recv_trade_notify_problem, ServerType.Area)
        {
            _objectId = objectId;
            _systemMessage = systemMessage;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_objectId); //optional instanceId for system messages
            res.WriteByte(_systemMessage); //which system message to throw
            return res;
        }
    }
}
/*
 * #trade,,,,
    TRADE_CANCEL,GENERIC,The trade failed.,SYSTEM_NOTIFY,
    TRADE_CANCEL,-34,"If you are in stealth"","" you cannot apply for a trade.",SYSTEM_NOTIFY,
    TRADE_CANCEL,-3002,You cannot trade during the event.,SYSTEM_NOTIFY,
    TRADE_CANCEL,-4000,You cannot start trading because you are too far away from your opponent.,SYSTEM_NOTIFY,
    TRADE_NOT,GENERIC,This item cannot be traded.,SYSTEM_WARNING,
    TRADE_NOT,0,I succeeded in trading.,SYSTEM_WARNING,
    TRADE_NOT,1,I have already applied for a trade.,SYSTEM_NOTIFY,
    TRADE_NOT,2,We are already applying for a trade.,SYSTEM_NOTIFY,
    TRADE_NOT,3,You have exceeded the number that can be traded at one time.,SYSTEM_WARNING,
    TRADE_NOT,4,Items that are equipped cannot be traded.,SYSTEM_WARNING,
    TRADE_NOT,5,This item has already been selected.,SYSTEM_WARNING,
    TRADE_NOT,6,There is nothing to trade.,SYSTEM_WARNING,
    TRADE_NOT,7,The amount of money you have in %s has exceeded the upper limit.,SYSTEM_WARNING,
    TRADE_NOT,8,There is not enough free inventory in %s.,SYSTEM_WARNING,
    TRADE_NOT,9,You cannot get gold that exceeds the maximum amount of money you have.,SYSTEM_WARNING,
    TRADE_NOT,10,There is not enough free inventory.,SYSTEM_WARNING,
    TRADE_NOT,11,This item is on sale at a stall.,SYSTEM_WARNING,
    TRADE_NOT,12,The amount of money you have in %s is less than the amount you are offering.,SYSTEM_WARNING,
    TRADE_NOT,13,The item slot in %s is invalid.,SYSTEM_WARNING,
    TRADE_NOT,14,Your money is less than what you are offering.,SYSTEM_WARNING,
    TRADE_NOT,15,The item slot is invalid.,SYSTEM_WARNING,
    TRADE_NOT,16,Trading has already started.,SYSTEM_WARNING,
    TRADE_NOT,17,It cannot be operated while the contents are being posted.,SYSTEM_WARNING,
    TRADE_NOT,18,You cannot trade items in different inventories.,SYSTEM_WARNING,
    TRADE_NOT,-215,It is not your property.,SYSTEM_WARNING,
    TRADE_NOT,-4001,Items in the Avatar Inventory cannot be traded.,SYSTEM_WARNING,
    TRADE_NOT,-4002,You cannot trade items in different inventories at the same time.,SYSTEM_WARNING,
*/
