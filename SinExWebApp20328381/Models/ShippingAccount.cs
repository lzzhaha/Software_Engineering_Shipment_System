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
        public virtual string MailingAddressBuilding { get; set; }
        [Required]
        [StringLength(35)]
        public virtual string MailingAddressStreet { get; set; }
        [Required]
        [StringLength(25)]
        public virtual string MailingAddressCity { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter valid province code!")]
        public virtual string MailingAddressProvinceCode { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid postal code!")]
        public virtual string MailingAddressPostalCode { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]
        public virtual string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^.*[@].*[.].*$", ErrorMessage = "Please enter a valid Email address.")]
        public virtual string EmailAddress { get; set; }
        [Required]
        [RegularExpression(@"^American Express$|^Diners Club$|^Discover$|^MasterCard$|^UnionPay$|^Visa$|", ErrorMessage = "Please enter valid card type.")]
        public virtual string CreditCardType { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 14)]
        public virtual string CreditCardNumber { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field Security Number must be a number.")]
        public virtual string CreditCardSecurityNumber { get; set; }
        [Required]
        [StringLength(70)]
        public virtual string CreditCardHolderName { get; set; }
        [Required]
        [Range(1, 12)]
        public virtual int CreditCardExpiryMonth { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "The field Expiry Year must be a number.")]
        public virtual string CreditCardExpiryYear { get; set; }
    }
    /*
    public class MailingAddress
    {
        [StringLength(50)]
        public virtual string Building { get; set; }
        [Required]
        [StringLength(35)]
        public virtual string Street { get; set; }
        [Required]
        [StringLength(25)]
        public virtual string City { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter valid province code!")]
        public virtual string ProvinceCode { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid postal code!")]
        public virtual string PostalCode { get; set; }
    }

    public class CreditCard
    {
        [Required]
        [RegularExpression(@"^American Express$|^Diners Club$|^Discover$|^MasterCard$|^UnionPay$|^Visa$|", ErrorMessage = "Please enter valid card type.")]
        public virtual string CardType { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 14)]
        public virtual string CardNumber { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public virtual string SecurityNumber { get; set; }
        [Required]
        [StringLength(70)]
        public virtual string CardHolderName { get; set; }
        [Required]
        [Range(1,12)]
        public virtual int ExpiryMonth { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Please enter valid year.")]
        public virtual string ExpiryYear { get; set; }
    }
    */
}