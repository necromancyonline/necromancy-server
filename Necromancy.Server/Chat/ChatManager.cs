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
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ChatManager));
        
        private readonly List<IChatHandler> _handler;
        private readonly NecServer _server;

        public ChatManager(NecServer server)
        {
            _server = server;
            _handler = new List<IChatHandler>();
            CommandHandler = new ChatCommandHandler(server);
            AddHandler(CommandHandler);
        }

        public ChatCommandHandler CommandHandler { get; }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
        }

        public void Handle(NecClient client, ChatMessage message)
        {
            if (client == null)
            {
                Logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                Logger.Debug(client, "Chat Message is Null");
                return;
            }

            ChatResponse response =
                new ChatResponse(client, message.Message, message.MessageType, message.RecipientSoulName);
            List<ChatResponse> responses = new List<ChatResponse>();
            foreach (IChatHandler handler in _handler)
            {
                handler.Handle(client, message, response, responses);
            }

            if (!response.Deliver)
            {
                RespondPostMessage(client, ChatErrorType.GenericUnknownStatement);
                return;
            }

            Deliver(client, response);
            RespondPostMessage(client, ChatErrorType.Success);


            foreach (ChatResponse chatResponse in responses)
            {
                Deliver(client, chatResponse);
            }
        }

        private void Deliver(NecClient sender, ChatResponse chatResponse)
        {
            switch (chatResponse.MessageType)
            {
                case ChatMessageType.ChatCommand:
                    chatResponse.Recipients.Add(sender);
                    break;
                case ChatMessageType.All:
                    chatResponse.Recipients.AddRange(sender.Map.ClientLookup.GetAll());
                    break;
                case ChatMessageType.Area:
                    chatResponse.Recipients.AddRange(sender.Map.ClientLookup.GetAll());
                    break;
                case ChatMessageType.Shout:
                    chatResponse.Recipients.AddRange(sender.Map.ClientLookup.GetAll());
                    break;
                case ChatMessageType.Whisper:
                    NecClient recipient = _server.Clients.GetBySoulName(chatResponse.RecipientSoulName);
                    if (recipient == null)
                    {
                        Logger.Error($"SoulName: {chatResponse.RecipientSoulName} not found");
                        return;
                    }

                    chatResponse.Recipients.Add(sender);
                    chatResponse.Recipients.Add(recipient);
                    break;
                default:
                    chatResponse.Recipients.Add(sender);
                    break;
            }

            _server.Router.Send(chatResponse);
        }

        private void RespondPostMessage(NecClient client, ChatErrorType chatErrorType)
        {
            RecvChatPostMessageR postMessageResponse = new RecvChatPostMessageR(chatErrorType);
            postMessageResponse.Clients.Add(client);
            _server.Router.Send(postMessageResponse);
        }
    }
}
