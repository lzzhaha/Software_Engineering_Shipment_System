using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    public class BusinessShippingAccount : ShippingAccount
    {
        [Required]
        [StringLength(70)]
        public virtual string ContactPersonName { get; set; }
        [Required]
        [StringLength(40)]
        public virtual string CompanyName { get; set; }
        [StringLength(30)]
        public virtual string DepartmentName { get; set; }
    }
}