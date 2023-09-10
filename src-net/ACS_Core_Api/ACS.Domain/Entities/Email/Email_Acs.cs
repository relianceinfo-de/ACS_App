using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Communication.Email;


namespace ACS.Domain.Entities.Email
{
    public class Email_Acs
    {
        public EmailContent Subject { get; set; }
        public EmailContent HtmlContent { get; set; }
        public string Sender { get; set; }
        public IEnumerable<EmailAddress> MultiRecipient { get; set; }
        public EmailAddress Recipient { get; set; }
    }
}
