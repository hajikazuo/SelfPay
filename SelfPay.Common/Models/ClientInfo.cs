using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Models
{
    public class ClientInfo
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [MaxLength(10)]
        [Display(Name = "Placa")]
        public string Plate { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }  
        
        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string? Telephone { get; set; }   
    }
}
