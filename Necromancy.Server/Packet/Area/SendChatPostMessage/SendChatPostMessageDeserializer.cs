using System;
using Arrowgene.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageDeserializer : IPacketDeserializer<ChatMessage>
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendChatPostMessageDeserializer));

        public SendChatPostMessageDeserializer()
        {
        }

        public ChatMessage Deserialize(NecPacket packet)
        {
            int messageTypeValue = packet.Data.ReadInt32();
            if (!Enum.IsDefined(typeof(ChatMessageType), messageTypeValue))
            {
                Logger.Error($"ChatMessageType: {messageTypeValue} not defined");
                return null;
            }

            ChatMessageType messageType = (ChatMessageType) messageTypeValue;
            string recipient = packet.Data.ReadCString();
            int unknown = packet.Data.ReadInt32(); //Not sure what this is, it is new from JP client. Might be talk ring related.
            string message = packet.Data.ReadCString();

            return new ChatMessage(messageType, recipient, message);
        }
    }
}
