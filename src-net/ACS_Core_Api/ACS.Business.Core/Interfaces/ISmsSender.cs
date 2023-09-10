using ACS.Domain.Entities.Sms;
using ACS.Domain.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Business.Core.Interfaces
{
    public interface ISmsSender
    {
       Task<ResponseStatus> SendMessageAsync(Sms_Acs sms_Acs);
        Task<ResponseStatus> SendMultipleMessagesAsync(Sms_Acs sms_Acs);
    }
}
