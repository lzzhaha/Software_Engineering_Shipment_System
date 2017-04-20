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
        public virtual string NickName { get; set; }
        [Key]
        public virtual int AddressId { get; set; }
        
        public virtual int ShippingAccountId { get; set; }
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
        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter valid province code!")]
        [Display(Name = "Province", Order = 8)]
        public virtual string ProvinceCode { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid postal code!")]
        [Display(Name = "Postal Code", Order = 9)]
        public virtual string PostalCode { get; set; }
        public virtual string AddressType { get; set; }
    }
}