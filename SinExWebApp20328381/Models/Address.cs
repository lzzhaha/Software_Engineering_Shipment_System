using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20328381.Models
{
    public class Address
    {
        [Required]
        public virtual string NickName { get; set; }
        [Key]
        public virtual int AddressId { get; set; }
        
        public virtual long? ShippingAccountId { get; set; }
        [StringLength(50)]
        [Display(Name = "Building", Order = 5)]
        public virtual string Building { get; set; }
        [Required]
        [StringLength(35)]
        [Display(Name = "Street", Order = 6)]
        public virtual string Street { get; set; }
        [Required]
        [StringLength(25)]
        [Display(Name = "City", Order = 7)]
        public virtual string City { get; set; }

        [RegularExpression(@"^[a-zA-Z\s']*$", ErrorMessage = "Please select valid cities")]
        [Display(Name = "ServiceCity", Order = 8)]
        public virtual string ServiceCity { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid postal code!")]
        [Display(Name = "Postal Code", Order = 9)]
        public virtual string PostalCode { get; set; }
        public virtual string AddressType { get; set; }
    }
}