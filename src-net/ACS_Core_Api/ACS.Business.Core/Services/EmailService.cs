using ACS.Business.Core.Interfaces;
using ACS.Domain.Entities.Utilities;
using Azure.Communication.Email;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.Domain.Entities.Sms;
using Azure.Communication.Sms;
using ACS.Domain.Entities.Email;

namespace ACS.Business.Core.Services
{
    public class EmailService : IEmailSender
    {
        private Authentication authentication = null;
        private ResponseStatus response = null;

        public async Task<ResponseStatus> SendEmailAsync(Email_Acs email)
        {
          
                try
                {
                    var auth = await authentication.EmailAuthentication();
                    Console.WriteLine("Sending email...");
                    EmailSendOperation emailSendOperation = await auth.SendAsync(
                        Azure.WaitUntil.Completed,
                        email.Sender,
                        email.Recipient,
                        email.Subject,
                        email.HtmlContent);
                    EmailSendResult statusMonitor = emailSendOperation.Value;

                    Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                    /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                    string operationId = emailSendOperation.Id;
                    Console.WriteLine($"Email operation id = {operationId}");

                  response.Status = true;
                response.Message = "Sms Sent Successfully.";
                return response;
                }

                catch (RequestFailedException ex)
                {
                    /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                    Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                }
                catch (Exception ex)
                {

                }
            return response;
        }

        public async Task<ResponseStatus> SendMultipleEmailsAsync(Email_Acs email_Acs)
        {
            try
            {
                var auth = await authentication.EmailAuthentication();

                // Create the email content
                var emailContent = new EmailContent(email_Acs.Subject);
                emailContent.Html= email_Acs.HtmlContent;

                // Create the to list
                var toRecipients = email_Acs.MultiRecipient;
                var emailRecipients = new EmailRecipients(toRecipients);
                var emailMessage = new EmailMessage
                    (
                   
                        email_Acs.Sender,
                        emailContent,
                         email_Acs.MultiRecipient
                        
                       
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
