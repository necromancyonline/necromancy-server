using System;
using Arrowgene.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageDeserializer : IPacketDeserializer<ChatMessage>
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendChatPostMessageDeserializer));

        public SendChatPostMessageDeserializer()
        {
        }

        public ChatMessage Deserialize(NecPacket packet)
        {
            int messageTypeValue = packet.data.ReadInt32();
            if (!Enum.IsDefined(typeof(ChatMessageType), messageTypeValue))
            {
                _Logger.Error($"ChatMessageType: {messageTypeValue} not defined");
                return null;
            }

            ChatMessageType messageType = (ChatMessageType) messageTypeValue;
            string recipient = packet.data.ReadCString();
            int unknown = packet.data.ReadInt32(); //Not sure what this is, it is new from JP client. Might be talk ring related.
            string message = packet.data.ReadCString();

            return new ChatMessage(messageType, recipient, message);
        }
    }
}
