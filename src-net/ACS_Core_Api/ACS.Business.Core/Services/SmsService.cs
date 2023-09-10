using ACS.Business.Core.Interfaces;
using ACS.Domain.Entities.Sms;
using ACS.Domain.Entities.Utilities;
using Azure;
using Azure.Communication.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace ACS.Business.Core.Services
{
    public class SmsService : ISmsSender
    {
        private Authentication authentication = null;
        private ResponseStatus response = null;
       
        public async Task<ResponseStatus> SendMessageAsync(Sms_Acs sms_Acs)
        {
            try
            {
                var auth = await authentication.SmsAuthentication();
                SmsSendResult sendResult = auth.Send
                    (
                        from: sms_Acs.From,
                        to: sms_Acs.To,
                        message: sms_Acs.Message
                    );
                response.Status = true;
                response.Message = "Sms Sent Successfully.";
                return response;
            }
            catch(Exception ex)
            {

            }
           return response;
        }

        public async Task<ResponseStatus> SendMultipleMessagesAsync(Sms_Acs sms_Acs)
        {

            try
            {
                var auth = await authentication.SmsAuthentication();
                Response<IReadOnlyList<SmsSendResult>> sendResult = auth.Send
                    (
                        from: sms_Acs.From,
                        to:  sms_Acs.ToMulti ,
                        message: sms_Acs.Message
                    );

                
                response.Status = true;
                response.Message = "Sms Sent Successfully.";
                return response;
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
