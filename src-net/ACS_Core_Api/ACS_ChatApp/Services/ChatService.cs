using Azure.Communication;
using Azure.Communication.Chat;
using Azure.Communication.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS_ChatApp.Services
{
    public class ChatService
    {
        private readonly ChatClient _chatClient;
        private ChatThreadClient _chatThreadClient;


        public event EventHandler<string> MessageReceived;
        public ChatService(string connectionString)
        {
            var communicationIdentityClient = new CommunicationIdentityClient(connectionString);
            var tokenResponse = communicationIdentityClient.CreateUserAndToken(scopes: new[] { CommunicationTokenScope.Chat });
            var userToken = tokenResponse.Value.AccessToken.Token;
            var communicationTokenCredential = new CommunicationTokenCredential(userToken);
            _chatClient = new ChatClient(new Uri("https://<YOUR_RESOURCE_NAME>.communication.azure.com"), communicationTokenCredential);
        }

        public async Task CreateChatThreadAsync(string topic)
        {
            try
            {
                var createChatThreadResult = await _chatClient.CreateChatThreadAsync(topic: topic, participants: new List<ChatParticipant>());
                _chatThreadClient = _chatClient.GetChatThreadClient(createChatThreadResult.Value.ChatThread.Id);

                // Subscribe to incoming messages
                StartPollingMessages();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (_chatThreadClient == null)
            {
                throw new InvalidOperationException("Chat thread has not been created.");
            }

            await _chatThreadClient.SendMessageAsync(new SendChatMessageOptions { Content = message });
        }

        private async void StartPollingMessages()
        {
            try
            {
                while (true)
                {

                    await foreach (var chatMessage in _chatThreadClient.GetMessagesAsync())
                    {
                        OnMessageReceived(chatMessage.Content.Message);
                    }

                    await Task.Delay(5000); 
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }
    }
}
