using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Chat.Command;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat
{
    public class ChatManager
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ChatManager));

        private readonly List<IChatHandler> _handler;
        private readonly NecServer _server;

        public ChatManager(NecServer server)
        {
            _server = server;
            _handler = new List<IChatHandler>();
            commandHandler = new ChatCommandHandler(server);
            AddHandler(commandHandler);
        }

        public ChatCommandHandler commandHandler { get; }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
        }

        public void Handle(NecClient client, ChatMessage message)
        {
            if (client == null)
            {
                _Logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                _Logger.Debug(client, "Chat Message is Null");
                return;
            }

            ChatResponse response =
                new ChatResponse(client, message.message, message.messageType, message.recipientSoulName);
            List<ChatResponse> responses = new List<ChatResponse>();
            foreach (IChatHandler handler in _handler) handler.Handle(client, message, response, responses);

            if (!response.deliver)
            {
                RespondPostMessage(client, ChatErrorType.GenericUnknownStatement);
                return;
            }

            Deliver(client, response);
            RespondPostMessage(client, ChatErrorType.Success);


            foreach (ChatResponse chatResponse in responses) Deliver(client, chatResponse);
        }

        private void Deliver(NecClient sender, ChatResponse chatResponse)
        {
            switch (chatResponse.messageType)
            {
                case ChatMessageType.ChatCommand:
                    chatResponse.recipients.Add(sender);
                    break;
                case ChatMessageType.All:
                    chatResponse.recipients.AddRange(sender.map.clientLookup.GetAll());
                    break;
                case ChatMessageType.Area:
                    chatResponse.recipients.AddRange(sender.map.clientLookup.GetAll());
                    break;
                case ChatMessageType.Shout:
                    chatResponse.recipients.AddRange(sender.map.clientLookup.GetAll());
                    break;
                case ChatMessageType.Whisper:
                    NecClient recipient = _server.clients.GetBySoulName(chatResponse.recipientSoulName);
                    if (recipient == null)
                    {
                        _Logger.Error($"SoulName: {chatResponse.recipientSoulName} not found");
                        return;
                    }

                    chatResponse.recipients.Add(sender);
                    chatResponse.recipients.Add(recipient);
                    break;
                default:
                    chatResponse.recipients.Add(sender);
                    break;
            }

            _server.router.Send(chatResponse);
        }

        private void RespondPostMessage(NecClient client, ChatErrorType chatErrorType)
        {
            RecvChatPostMessageR postMessageResponse = new RecvChatPostMessageR(chatErrorType);
            postMessageResponse.clients.Add(client);
            _server.router.Send(postMessageResponse);
        }
    }
}
