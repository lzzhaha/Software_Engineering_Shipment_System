using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    [Table("ShippingAccount")]
    public abstract class ShippingAccount
    {
        public virtual int ShippingAccountId { get; set; }
        [StringLength(50)]
        [Display(Name = "Building", Order = 5)]
        public virtual string MailingAddressBuilding { get; set; }
        [Required]
        [StringLength(35)]
        [Display(Name = "Street", Order = 6)]
        public virtual string MailingAddressStreet { get; set; }
        [Required]
        [StringLength(25)]
        [Display(Name = "City", Order = 7)]
        public virtual string MailingAddressCity { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter valid province code!")]
        [Display(Name = "Province", Order = 8)]
        public virtual string MailingAddressProvinceCode { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid postal code!")]
        [Display(Name = "Postal Code", Order = 9)]
        public virtual string MailingAddressPostalCode { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]
        [Display(Name = "Phonenumber", Order = 3)]
        public virtual string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^.*[@].*[.].*$", ErrorMessage = "Please enter a valid Email address.")]
        [Display(Name = "Email", Order = 4)]
        public virtual string EmailAddress { get; set; }
        [Required]
        [RegularExpression(@"^American Express$|^Diners Club$|^Discover$|^MasterCard$|^UnionPay$|^Visa$", ErrorMessage = "Please enter valid card type.")]
        [Display(Name = "Type", Order = 10)]
        public virtual string CreditCardType { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 14)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field Number must be numeric.")]
        [Display(Name = "Number", Order = 11)]
        public virtual string CreditCardNumber { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field Security Number must be a number.")]
        [Display(Name = "Security Number", Order = 12)]
        public virtual string CreditCardSecurityNumber { get; set; }
        [Required]
        [StringLength(70)]
        [Display(Name = "Cardholder Name", Order = 13)]
        public virtual string CreditCardHolderName { get; set; }
        [Required]
        [Range(1, 12)]
        [Display(Name = "Expiry Month", Order = 14)]
        public virtual int CreditCardExpiryMonth { get; set; }
        [Required]
        [Range(1000,3000)]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "The field Expiry Year must be a number.")]
        [Display(Name = "Expiry Year", Order = 15)]
        public virtual string CreditCardExpiryYear { get; set; }
        [StringLength(10, MinimumLength = 6)]
        public virtual string UserName { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }

    }
}