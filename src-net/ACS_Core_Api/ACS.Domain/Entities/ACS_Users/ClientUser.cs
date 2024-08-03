using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Domain.Entities.ACS_Users
{
    public class ClientUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Acs_Id { get; set; }
        public string Acs_Token { get;}
    }
}
