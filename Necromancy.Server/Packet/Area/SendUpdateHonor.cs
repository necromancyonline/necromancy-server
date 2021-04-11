using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_update_honor : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_update_honor));
        public send_update_honor(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_update_honor;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int honorTitleID = packet.Data.ReadInt32();
            Server.SettingRepository.Honor.TryGetValue(honorTitleID, out HonorSetting honorSetting);
            IBuffer res = BufferProvider.Provide();

            Logger.Debug($"You hovered over a new title. {honorSetting.Name}  Great job!");
            
            //Update the database entry for Honor ID (readInt32)  from new 0 to .. not-new  1

        }

    }
}
