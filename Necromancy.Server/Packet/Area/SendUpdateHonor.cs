using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendUpdateHonor : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendUpdateHonor));

        public SendUpdateHonor(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_update_honor;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int honorTitleId = packet.data.ReadInt32();
            server.settingRepository.honor.TryGetValue(honorTitleId, out HonorSetting honorSetting);
            IBuffer res = BufferProvider.Provide();

            _Logger.Debug($"You hovered over a new title. {honorSetting.name}  Great job!");

            //Update the database entry for Honor ID (readInt32)  from new 0 to .. not-new  1
        }
    }
}
