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
    public class SendMovementInfo : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendMovementInfo));

        public SendMovementInfo(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_movement_info;


        public override void Handle(NecClient client, NecPacket packet)
        {
            // If changing maps don't update position
            if (client.character.mapChange)
            {
                return;
            }

            client.character.x = packet.data.ReadFloat();
            client.character.y = packet.data.ReadFloat();
            client.character.z = packet.data.ReadFloat();

            float percentMovementIsX = packet.data.ReadFloat();
            float percentMovementIsY = packet.data.ReadFloat();
            float verticalMovementSpeedMultiplier =
                packet.data.ReadFloat(); //  Confirm by climbing ladder at 1 up or -1 down. or Jumping

            float movementSpeed = packet.data.ReadFloat();

            float horizontalMovementSpeedMultiplier =
                packet.data
                    .ReadFloat(); //always 1 when moving.  Confirm by Coliding with an  object and watching it Dip.

            client.character.movementPose =
                packet.data.ReadByte(); //Character Movement Type: Type 8 Falling / Jumping. Type 3 normal:  Type 9  climbing

            client.character.movementAnim = packet.data.ReadByte(); //Action Modifier Byte
            //146 :ladder left Foot Up.      //147 Ladder right Foot Up.
            //151 Left Foot Down,            //150 Right Root Down .. //155 falling off ladder
            //81  jumping up,                //84  jumping down       //85 landing


            //Battle Logic until we find out how to write battle byte requrements in 'send_data_get_Self_chara_data_request' so the client can send the right info
            if (client.character.battleAnim != 0)
            {
                client.character.movementPose = 8 /*client.Character.battlePose*/
                    ; //Setting the pose byte to the 2nd and 3rd digits of our equipped weapon ID. For battle!!
                client.character.movementAnim =
                    client.character
                        .battleAnim; //Setting the animation byte to an animation from C:\WO\Chara\chara\00\041\anim. 231, 232, 233, and 244 are attack animations
            }


            IBuffer res2 = BufferProvider.Provide();

            res2.WriteUInt32(client.character.instanceId); //Character ID
            res2.WriteFloat(client.character.x);
            res2.WriteFloat(client.character.y);
            res2.WriteFloat(client.character.z);

            res2.WriteFloat(percentMovementIsX);
            res2.WriteFloat(percentMovementIsY);
            res2.WriteFloat(verticalMovementSpeedMultiplier);

            res2.WriteFloat(movementSpeed);

            res2.WriteFloat(horizontalMovementSpeedMultiplier);

            res2.WriteByte(client.character.movementPose);
            res2.WriteByte(client.character.movementAnim);

            router.Send(client.map, (ushort) AreaPacketId.recv_0x8D92, res2, ServerType.Area, client);

            client.character.battleAnim = 0; //re-setting the byte to 0 at the end of every iteration to allow for normal movements.

            //Cancel skill execution on movement start
            if (client.character.castingSkill)
            {
                RecvSkillCastCancel cancelCast = new RecvSkillCastCancel();
                router.Send(client.map, cancelCast.ToPacket());
                client.character.activeSkillInstance = 0;
                client.character.castingSkill = false;
            }

            //Tell any charaBodies who are along for the ride that movement is happening.
            foreach (NecClient necClient in client.bodyCollection.Values)
            {
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteUInt32(necClient.character.instanceId);
                res5.WriteFloat(client.character.x);
                res5.WriteFloat(client.character.y);
                res5.WriteFloat(client.character.z);
                res5.WriteByte(client.character.heading); //Heading
                res5.WriteByte(client.character.movementAnim); //state
                router.Send(necClient, (ushort)AreaPacketId.recv_object_point_move_notify, res5, ServerType.Area);
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
            if (client.character.takeover == true)
            {
                _Logger.Debug($"Moving object ID {client.character.eventSelectReadyCode}.");
                IBuffer res = BufferProvider.Provide();
                res.WriteUInt32(client.character.eventSelectReadyCode);
                res.WriteFloat(client.character.x);
                res.WriteFloat(client.character.y);
                res.WriteFloat(client.character.z);
                res.WriteByte(client.character.heading); //Heading
                res.WriteByte(client.character.movementAnim); //state
                router.Send(client.map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
            }

            //Logic to see if you are in range of a map transition
            client.character.stepCount++;
            if (client.character.stepCount % 4 == 0)
            {
                CheckMapChange(client);
            }

        }

        private void CheckMapChange(NecClient client)
        {
            Vector3 characterPos = new Vector3(client.character.x, client.character.y, client.character.z);
            if (client.character == null)
            {
                return;
            }

            foreach (MapTransition mapTransition in client.map.mapTransitions.Values)
            {
                float lineProximity = PDistance(characterPos, mapTransition.leftPos, mapTransition.rightPos);
               // Logger.Debug($"{_character.Name} checking map {mapTransition.MapId} [transition] id {mapTransition.Id} to destination {mapTransition.TransitionMapId}");
               // Logger.Debug($"Distance to transition : {lineProximity}");
                if (lineProximity < 155)
                {
                    if (!server.maps.TryGet(mapTransition.transitionMapId, out Map transitionMap))
                    {
                        return;
                    }
                    transitionMap.EnterForce(client, mapTransition.toPos);
                }


            }

        }
        static float PDistance(Vector3 vectorA, Vector3 vectorB, Vector3 vectorC)
        {

            float distanceA = vectorA.X - vectorB.X;
            float distanceB = vectorA.Y - vectorB.Y;
            float distanceC = vectorC.X - vectorB.X;
            float distanceD = vectorC.Y - vectorB.Y;

            float dot = distanceA * distanceC + distanceB * distanceD;
            float lenSq = distanceC * distanceC + distanceD * distanceD;
            float param = -1;
            if (lenSq != 0) //in case of 0 length line
                param = dot / lenSq;

            float xx, yy;

            if (param < 0)
            {
                xx = vectorB.X;
                yy = vectorB.Y;
            }
            else if (param > 1)
            {
                xx = vectorC.X;
                yy = vectorC.Y;
            }
            else
            {
                xx = vectorB.X + param * distanceC;
                yy = vectorB.Y + param * distanceD;
            }

            float dx = vectorA.X - xx;
            float dy = vectorA.Y - yy;
            return (float)Math.Abs(Math.Sqrt(dx * dx + dy * dy));
        }


    }
}
