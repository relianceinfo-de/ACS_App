using ACS.Domain.Entities.Chat;
using Azure;
using Azure.Communication;
using Azure.Communication.Chat;
using Azure.Communication.Email;
using Azure.Communication.Identity;
using Azure.Communication.Sms;
using Azure.Core;
using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACS.Business.Core.Services
{
    public class Authentication
    {
        public async Task<SmsClient> SmsAuthentication()
        {
            SmsClient result = null;
            try
            {
                string resourceEndpoint = "https://commazeezdemo.africa.communication.azure.com";
                //"https://noccommservice.unitedstates.communication.azure.com";

                SmsClient smsClient = new SmsClient(new Uri(resourceEndpoint), new InteractiveBrowserCredential());

                result = smsClient;
                return result;
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        public async Task<EmailClient> EmailAuthentication()
        {
            EmailClient result = null;
            try
            {
                string resourceEndpoint = "https://commazeezdemo.africa.communication.azure.com";
                //"https://noccommservice.unitedstates.communication.azure.com";

                EmailClient emailClient = new EmailClient(new Uri(resourceEndpoint), new InteractiveBrowserCredential());
                result = emailClient;
                return result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        //public async Task ListChatsThreads(ChatThreadClient)
        //{
        //    try
        //    {
        //        var chatClient = ChatAuthenticate();
        //        AsyncPageable<ChatThreadItem> chatThreadItems = chatClient.Result.GetChatThreadsAsync();
        //        await foreach (ChatThreadItem chatThreadItem in chatThreadItems)
        //        {
        //            Console.WriteLine($"{chatThreadItem.Id}");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}




    }
}
