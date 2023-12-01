using Azure.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Domain.Entities.Chat
{
    public class ScopeResponse
    {
        public CommunicationUserIdentifier UserIdentity { get; set; }
        public string AccessToken { get; set; }
    }
}
