using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    public class PersonalShippingAccount : ShippingAccount
    {
        [Required]
        [StringLength(35)]

        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(35)]
        public virtual string LastName { get; set; }

    }
}