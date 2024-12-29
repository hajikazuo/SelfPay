using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Models
{
    public class ClientInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Plate { get; set; }
        public string Email { get; set; }   
        public string TelePhone { get; set; }   
    }
}
