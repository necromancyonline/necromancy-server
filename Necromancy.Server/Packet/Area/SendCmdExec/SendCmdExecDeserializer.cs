namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecDeserializer : IPacketDeserializer<SendCmdExecRequest>
    {
        public SendCmdExecDeserializer()
        {
        }

        public SendCmdExecRequest Deserialize(NecPacket packet)
        {
            string command = packet.data.ReadCString();

            SendCmdExecRequest sendCmdExecRequest = new SendCmdExecRequest(command);

            int startPosition = 49;
            int blockSize = 769;
            while (startPosition + blockSize < packet.data.Size)
            {
                packet.data.Position = startPosition;
                string parameter = packet.data.ReadCString();
                sendCmdExecRequest.parameter.Add(parameter);
                packet.data.Position = startPosition;
                startPosition += blockSize;
                // Skip 769 Unknown bytes
            }

            packet.data.Position = 7739;
            int u = packet.data.ReadInt32();
            int u1 = packet.data.ReadInt32();

            return sendCmdExecRequest;
        }
    }
}
