using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace ACS.Domain.Entities.Sms
{
    public class Sms_Acs
    {
        public string From { get; set; }
        public string To { get; set; }
        public string[] ToMulti { get; set; }
        public string Message { get; set; }
    }
}
