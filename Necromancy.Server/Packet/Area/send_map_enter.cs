using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_map_enter));

        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check. must be 0
            res.WriteByte(0); //Bool - play cutscene. 1 yes, 0 no?  //to-do,  play a cutscene on first time map entry 
            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res, ServerType.Area);

            if (client.Map.DeadBodies.ContainsKey(client.Character.DeadBodyInstanceId))
            {
                Logger.Debug($"found dead body of {client.Character.Name} already on map.  Hope you didn't lose your loot!");
                RecvCharaBodyNotifySpirit recvCharaBodyNotifySpirit = new RecvCharaBodyNotifySpirit(client.Character.DeadBodyInstanceId, (byte)RecvCharaBodyNotifySpirit.ValidSpirit.ConnectedClient);
                Router.Send(client.Map, recvCharaBodyNotifySpirit.ToPacket());
            }
            else if (client.Character.State.HasFlag(CharacterState.SoulForm))
            {
                DeadBody deadBody = Server.Instances.GetInstance((uint)client.Character.DeadBodyInstanceId) as DeadBody;
                deadBody.X = client.Character.X;
                deadBody.Y = client.Character.Y;
                deadBody.Z = client.Character.Z;
                deadBody.Heading = client.Character.Heading;
                deadBody.BeginnerProtection = (byte)client.Character.beginnerProtection;
                deadBody.CharaName = client.Character.Name;
                deadBody.SoulName = client.Soul.Name;
                deadBody.EquippedItems = client.Character.EquippedItems;
                deadBody.RaceId = client.Character.RaceId;
                deadBody.SexId = client.Character.SexId;
                deadBody.HairId = client.Character.HairId;
                deadBody.HairColorId = client.Character.HairColorId;
                deadBody.FaceId = client.Character.FaceId;
                deadBody.FaceArrangeId = client.Character.FaceArrangeId;
                deadBody.VoiceId = client.Character.VoiceId;
                deadBody.Level = client.Character.Level;
                deadBody.ClassId = client.Character.ClassId;
                deadBody.EquippedItems = client.Character.EquippedItems;
                deadBody.ItemManager = client.Character.ItemManager;
                deadBody.ConnectionState = 1;
                client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
                RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                if (client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                { 
                    Router.Send(client.Map, cBodyData.ToPacket(), client); 
                }
                Router.Send(client, cBodyData.ToPacket());
            }

            //Re-do all your stats
            ItemService itemService = new ItemService(client.Character);
            Router.Send(client, itemService.CalculateBattleStats(client));


            //added delay to prevent crash on map entry.
            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
            (t1 =>
            {

                client.Character.ClearStateBit(CharacterState.InvulnerableForm);

                RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(client.Character.InstanceId, (ulong)client.Character.State);
                Router.Send(client.Map, recvCharaNotifyStateflag.ToPacket());
            }
            );

        }

    }
}
