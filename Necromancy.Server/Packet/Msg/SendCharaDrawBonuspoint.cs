using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaDrawBonuspoint : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharaDrawBonuspoint));

        public SendCharaDrawBonuspoint(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_chara_draw_bonuspoint;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint unknown = packet.data.ReadByte();
            _Logger.Info($"Unknown: {unknown}");

            byte bonusPoints;


            int rangeDetermination = Util.GetRandomNumber(0, 10000);
            // ^^ generate a number between 0 and 10000 to determinate the bonus points range

            if (rangeDetermination >= 9980)
            {
                bonusPoints = (byte) Util.GetRandomNumber(21, 30);
            }
            else if (rangeDetermination >= 9880)
            {
                bonusPoints = (byte) Util.GetRandomNumber(16, 20);
            }
            else if (rangeDetermination >= 9680)
            {
                bonusPoints = (byte) Util.GetRandomNumber(10, 15);
            }
            else
            {
                bonusPoints = (byte) Util.GetRandomNumber(1, 10);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(bonusPoints); // Number of points

            router.Send(client, (ushort) MsgPacketId.recv_chara_draw_bonuspoint_r, res, ServerType.Msg);
        }
    }
}
