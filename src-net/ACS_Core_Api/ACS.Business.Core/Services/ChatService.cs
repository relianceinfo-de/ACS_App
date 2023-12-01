using ACS.Business.Core.Interfaces;
using Azure.Communication.Chat;
using Azure.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.Domain.Entities.Chat;
using Azure.Communication.Identity;
using Azure;

namespace ACS.Business.Core.Services
{
    public class ChatService : IChatSender
    {
        private async Task<ChatClient> ChatAuthenticate(string token)
        {
            try
            {
                string Access_Token = token;

                // Your unique Azure Communication service endpoint

                string uri = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_ENDPOINT");
                Uri endpoint = new Uri(uri);

                CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(Access_Token);
                ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);

                return chatClient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ChatThreadClient> AppChatStart()
        {
            try
            {
                var identity = CreateIdentity();
                var chatClient = ChatAuthenticate(identity.Result.AccessToken);

                var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: identity.Result.UserIdentity.Id))
                {
                    DisplayName = "Admin User"
                };
                CreateChatThreadResult createChatThreadResult = await chatClient.Result.CreateChatThreadAsync(topic: "ACS Chat Group!", participants: new[] { chatParticipant });
                ChatThreadClient chatThreadClient = chatClient.Result.GetChatThreadClient(threadId: createChatThreadResult.ChatThread.Id);
                string threadId = chatThreadClient.Id;
                Console.WriteLine($"Thread Topic: {createChatThreadResult.ChatThread.Topic}");

                Console.WriteLine($"User Name: {chatParticipant.DisplayName}");
                return chatThreadClient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatThreadClient> GetChatThread(string id, string token)
        {
            try
            {
                var chatClient = ChatAuthenticate(token);
                string threadId = id;
                ChatThreadClient chatThreadClient = chatClient.Result.GetChatThreadClient(threadId: threadId);

                return chatThreadClient;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<SendChatMessageResult> SendChat(string id, string content, ChatThreadClient client)
        {
            try
            {
                var chatClient = client;
                SendChatMessageOptions sendChatMessageOptions = new SendChatMessageOptions()
                {
                    Content = content,
                    MessageType = ChatMessageType.Text
                };
                sendChatMessageOptions.Metadata["hasAttachment"] = "true";
                sendChatMessageOptions.Metadata["attachmentUrl"] = "https://contoso.com/files/attachment.docx";

                SendChatMessageResult sendChatMessageResult = chatClient.SendMessage(sendChatMessageOptions);

                string messageId = sendChatMessageResult.Id;

                return sendChatMessageResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task ReceiveChat(ChatThreadClient id)
        {
            try
            {
                var chatClient = id;
                Pageable<ChatMessage> allMessages = chatClient.GetMessages(null);
                foreach (ChatMessage message in allMessages)
                {
                    Console.WriteLine($"{message.Id}:{message.Content.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task ReceiveChats(ChatThreadClient id)
        {
            try
            {
                var chatClient = id;
                AsyncPageable<ChatMessage> allMessages = chatClient.GetMessagesAsync(DateTimeOffset.Now);
                await foreach (ChatMessage message in allMessages)
                {
                    Console.WriteLine($"{message.Id}:{message.Content.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddUsersToChat(ChatThreadClient id, string userName)
        {
            try
            {
                var chatClient = id;
                var identity = await CreateIdentity();

                var uName = new CommunicationUserIdentifier(id: identity.UserIdentity.Id);

                var participants = new[]
                {
                    new ChatParticipant(uName) { DisplayName = userName },

                };

                var result = chatClient.AddParticipants(participants: participants);
                Console.WriteLine(result.Value);

                Pageable<ChatParticipant> allParticipants = chatClient.GetParticipants();
                foreach (ChatParticipant participant in allParticipants)
                {
                    Console.WriteLine($"{((CommunicationUserIdentifier)participant.User).Id}:{participant.DisplayName}:{participant.ShareHistoryTime}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add users to chat");
            }
        }

        public async Task<CommunicationIdentityClient> CreateIdentityClient()
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
                var client = new CommunicationIdentityClient(connectionString);


                string endpoint = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_ENDPOINT");

                string accessKey = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_ACCESSKEY");

                var clientKey = new CommunicationIdentityClient(new Uri(endpoint), new AzureKeyCredential(accessKey));



                return client;

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<ScopeResponse> CreateIdentity()
        {
            try
            {
                var client = await CreateIdentityClient();
                // Create an identity
                var identityResponse = await client.CreateUserAsync();
                var identity = identityResponse.Value;
                Console.WriteLine($"\nCreated an identity with ID: {identity.Id}");
                var token = AccessToken(identifier: identity, client);
                var scopeId = new ScopeResponse()
                {
                    UserIdentity = identity,
                    AccessToken = token.Result
                };
                return scopeId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> AccessToken(CommunicationUserIdentifier identifier, CommunicationIdentityClient identityClient)
        {
            try
            {

                var client = identityClient;
                var identity = identifier;
                // Issue an access token with a validity of 24 hours and the "chat" scope for an identity

                var tokenResponse = await client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.Chat });

                // Get the token from the response
                var token = tokenResponse.Value.Token;
                var expiresOn = tokenResponse.Value.ExpiresOn;
                Console.WriteLine($"\nIssued an access token with 'chat' scope that expires at {expiresOn}:");
                Console.WriteLine(token);

                return token;




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RefreshAccessToken(string id)
        {
            try
            {
                var client = await CreateIdentityClient();
                //var identity = await CreateIdentity();

                // Refresh an access token
                var identityToRefresh = new CommunicationUserIdentifier(id);
                var tokenRefreshResponse = await client.GetTokenAsync(identityToRefresh, scopes: new[] { CommunicationTokenScope.Chat });

                var result = tokenRefreshResponse.Value.Token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
