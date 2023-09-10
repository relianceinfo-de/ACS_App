using ACS.Domain.Entities.Email;
using ACS.Domain.Entities.Sms;
using ACS.Domain.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Business.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<ResponseStatus> SendEmailAsync(Email_Acs email_Acs);
        Task<ResponseStatus> SendMultipleEmailsAsync(Email_Acs email_Acs);
    }
}
