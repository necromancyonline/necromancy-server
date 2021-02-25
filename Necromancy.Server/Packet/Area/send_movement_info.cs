using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using System;
using System.Numerics;

namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_movement_info));

        public send_movement_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_movement_info;


        public override void Handle(NecClient client, NecPacket packet)
        {
            // If changing maps don't update position
            if (client.Character.mapChange)
            {
                return;
            }

            client.Character.X = packet.Data.ReadFloat();
            client.Character.Y = packet.Data.ReadFloat();
            client.Character.Z = packet.Data.ReadFloat();

            float percentMovementIsX = packet.Data.ReadFloat();
            float percentMovementIsY = packet.Data.ReadFloat();
            float verticalMovementSpeedMultiplier =
                packet.Data.ReadFloat(); //  Confirm by climbing ladder at 1 up or -1 down. or Jumping

            float movementSpeed = packet.Data.ReadFloat();

            float horizontalMovementSpeedMultiplier =
                packet.Data
                    .ReadFloat(); //always 1 when moving.  Confirm by Coliding with an  object and watching it Dip.

            client.Character.movementPose =
                packet.Data.ReadByte(); //Character Movement Type: Type 8 Falling / Jumping. Type 3 normal:  Type 9  climbing

            client.Character.movementAnim = packet.Data.ReadByte(); //Action Modifier Byte
            //146 :ladder left Foot Up.      //147 Ladder right Foot Up. 
            //151 Left Foot Down,            //150 Right Root Down .. //155 falling off ladder
            //81  jumping up,                //84  jumping down       //85 landing


            //Battle Logic until we find out how to write battle byte requrements in 'send_data_get_Self_chara_data_request' so the client can send the right info
            if (client.Character.battleAnim != 0)
            {
                client.Character.movementPose = 8 /*client.Character.battlePose*/
                    ; //Setting the pose byte to the 2nd and 3rd digits of our equipped weapon ID. For battle!!
                client.Character.movementAnim =
                    client.Character
                        .battleAnim; //Setting the animation byte to an animation from C:\WO\Chara\chara\00\041\anim. 231, 232, 233, and 244 are attack animations
            }


            IBuffer res2 = BufferProvider.Provide();

            res2.WriteUInt32(client.Character.InstanceId); //Character ID
            res2.WriteFloat(client.Character.X);
            res2.WriteFloat(client.Character.Y);
            res2.WriteFloat(client.Character.Z);

            res2.WriteFloat(percentMovementIsX);
            res2.WriteFloat(percentMovementIsY);
            res2.WriteFloat(verticalMovementSpeedMultiplier);

            res2.WriteFloat(movementSpeed);

            res2.WriteFloat(horizontalMovementSpeedMultiplier);

            res2.WriteByte(client.Character.movementPose); 
            res2.WriteByte(client.Character.movementAnim); 

            Router.Send(client.Map, (ushort) AreaPacketId.recv_0x8D92, res2, ServerType.Area, client);

            client.Character.battleAnim =
                0; //re-setting the byte to 0 at the end of every iteration to allow for normal movements.

            if (client.Character.castingSkill)
            {
                RecvSkillCastCancel cancelCast = new RecvSkillCastCancel();
                Router.Send(client.Map, cancelCast.ToPacket());
                client.Character.activeSkillInstance = 0;
                client.Character.castingSkill = false;
            }


            //Uncomment for debugging movement. causes heavy console output. recommend commenting out "Packet" method in NecLogger.CS when debugging movement
            /*
            if (movementSpeed != 0)
            {
                Logger.Debug($"Character {client.Character.Name} is in map {client.Character.MapId} @ : X[{client.Character.X}]Y[{client.Character.Y}]Z[{client.Character.Z}]");
                Logger.Debug($"X Axis Aligned : {percentMovementIsX.ToString("P", CultureInfo.InvariantCulture)} | Y Axis Aligned  : {percentMovementIsY.ToString("P", CultureInfo.InvariantCulture)}");
                Logger.Debug($"vertical Speed multi : {verticalMovementSpeedMultiplier}| Move Speed {movementSpeed} | Horizontal Speed Multi {horizontalMovementSpeedMultiplier}");
                Logger.Debug($"Movement Type[{client.Character.movementPose}]  Type Anim [{client.Character.movementAnim}] View Offset:{client.Character.Heading}");
                Logger.Debug($"---------------------------------------------------------------");

                //Logger.Debug($"Var 1 {(byte)(percentMovementIsX*255)} |Var 2 {(byte)(percentMovementIsY*255)}  ");
                //Logger.Debug($" Y to Map Y[{d}][{e}][{e1}][{f} |  and {percentMovementIsY}");
            }
            else
            {
                Logger.Debug($"Movement Stop Reset");
                Logger.Debug($"---------------------------------------------------------------");
            }
            */


            ///////////
            /////////-----ToDO:  Find a home for the commands below this line as solutions develop.  Do not Delete!
            ///////////


            //Support for /takeover command to enable moving objects administratively
            if (client.Character.takeover == true)
            {
                Logger.Debug($"Moving object ID {client.Character.eventSelectReadyCode}.");
                IBuffer res = BufferProvider.Provide();
                IBuffer res3 = BufferProvider.Provide();


                res.WriteUInt32(client.Character.eventSelectReadyCode);
                res.WriteFloat(client.Character.X);
                res.WriteFloat(client.Character.Y);
                res.WriteFloat(client.Character.Z);
                res.WriteByte(client.Character.Heading); //Heading
                res.WriteByte(client.Character.movementAnim); //state

                Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                Router.Send(client, (ushort) AreaPacketId.recv_object_point_move_r, res3, ServerType.Area);
            }

            //Logic to see if you are in range of a map transition
            client.Character.StepCount++;
            if (client.Character.StepCount % 4 == 0)
            {
                CheckMapChange(client);
            }

        }

        private void CheckMapChange(NecClient client)
        {
            Character _character = client.Character;
            NecClient _client = client;
            Vector3 characterPos = new Vector3(_character.X, _character.Y, _character.Z);
            if (_character == null | _client == null)
            { 
                return; 
            }

            foreach (MapTransition mapTransition in _client.Map.MapTransitions.Values)
            {
                float lineProximity = pDistance(characterPos, mapTransition.LeftPos, mapTransition.RightPos);
               // Logger.Debug($"{_character.Name} checking map {mapTransition.MapId} [transition] id {mapTransition.Id} to destination {mapTransition.TransitionMapId}");
               // Logger.Debug($"Distance to transition : {lineProximity}");
                if (lineProximity < 155)
                {
                    if (!Server.Maps.TryGet(mapTransition.TransitionMapId, out Map transitionMap))
                    {
                        return;
                    }
                    transitionMap.EnterForce(_client, mapTransition.ToPos);
                }


            }

        }
        static float pDistance(Vector3 a, Vector3 b, Vector3 c)
        {

            float A = a.X - b.X;
            float B = a.Y - b.Y;
            float C = c.X - b.X;
            float D = c.Y - b.Y;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float param = -1;
            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            float xx, yy;

            if (param < 0)
            {
                xx = b.X;
                yy = b.Y;
            }
            else if (param > 1)
            {
                xx = c.X;
                yy = c.Y;
            }
            else
            {
                xx = b.X + param * C;
                yy = b.Y + param * D;
            }

            float dx = a.X - xx;
            float dy = a.Y - yy;
            return (float)Math.Abs(Math.Sqrt(dx * dx + dy * dy));
        }


    }
}
