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
                var emailContent = new EmailContent(email_Acs.Subject)
                {
                    PlainText = email_Acs.PlainContent,
                    Html = $"<html><body><h1>{email_Acs.HtmlContent}.</h1><p>This mail was sent using .NET SDK!!</p></body></html>"
                };

                // Create the To list
                var toRecipients = new List<EmailAddress>
                {                   
                  //new EmailAddress("<emailalias1@emaildomain.com>"),
                  //new EmailAddress("<emailalias2@emaildomain.com>"),
                };
                foreach (var newEmail in email_Acs.MultiRecipient)
                {
                    toRecipients = new List<EmailAddress>
                    {
                        new EmailAddress(newEmail)
                    };

                }

                // Create the CC list
                var ccRecipients = new List<EmailAddress>
                {
                  new EmailAddress("<ccemailalias@emaildomain.com>"),
                };

                foreach (var newEmail in email_Acs.CcRecipient)
                {
                    ccRecipients = new List<EmailAddress>
                    {
                        new EmailAddress(newEmail)
                    };
                }

                // Create the BCC list
                var bccRecipients = new List<EmailAddress>
                {
                  new EmailAddress("<bccemailalias@emaildomain.com>"),
                };

                foreach (var newEmail in email_Acs.BccRecipient)
                {
                    bccRecipients = new List<EmailAddress>
                    {
                        new EmailAddress(newEmail)
                    };
                }
                EmailRecipients emailRecipients = new EmailRecipients(toRecipients, ccRecipients, bccRecipients);

                // Create the EmailMessage
                var emailMessage = new EmailMessage(
                    senderAddress: "donotreply@xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.azurecomm.net" ,// The email address of the domain registered with the Communication Services resource

                    emailRecipients,
                    emailContent);

                // Add optional ReplyTo address which is where any replies to the email will go to.
                emailMessage.ReplyTo.Add(new EmailAddress("<replytoemailalias@emaildomain.com>"));

                try
                {
                    EmailSendOperation emailSendOperation = auth.Send(WaitUntil.Completed, emailMessage);
                    Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                    /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                    string operationId = emailSendOperation.Id;
                    Console.WriteLine($"Email operation id = {operationId}");
                }
                catch (RequestFailedException ex)
                {
                    /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                    Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                }
            }
            catch(Exception ex)
            {

            }
            return response;
        }
    }
}
