using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Domain.Entities.User
{
    public class ChatUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string ChatId { get; set; }
    }
}
