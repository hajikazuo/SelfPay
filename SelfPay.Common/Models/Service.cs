using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Models
{
    public class Service
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Preço 15")]
        public Decimal Price15 { get; set; }

        [Display(Name = "Preço 20")]
        public Decimal Price20 { get; set; }

        [Display(Name = "Preço 30")]
        public Decimal Price30 { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }    
    }
}
