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
        public string Subject { get; set; }
        public string PlainContent { get; set; }
        public string HtmlContent { get; set; }
        public string Sender { get; set; }
        public IEnumerable<string> MultiRecipient { get; set; }
        public IEnumerable<string> CcRecipient { get; set; }
        public IEnumerable<string> BccRecipient { get; set; }
        public string Recipient { get; set; }
    }
}
