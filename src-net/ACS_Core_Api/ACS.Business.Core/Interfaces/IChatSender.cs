using Azure.Communication.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Business.Core.Interfaces
{
    public interface IChatSender
    {
        Task<ChatThreadClient> AppChatStart(string name);
        Task AddUsersToChat(ChatThreadClient id, string userName);
        Task<SendChatMessageResult> SendChat(string content, string threadId);
    }
}
