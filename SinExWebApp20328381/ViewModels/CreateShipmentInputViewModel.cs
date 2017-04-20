using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class CreateShipmentInputViewModel
    {
        public virtual ShippingAccount CurrentShippingAccount { get; set; }
        public virtual FeeCheckGenerateViewModel SystemOutputSource { get; set; }
        [RegularExpression("^[0-9a-zA-Z]*$", ErrorMessage = "Please enter a valid number.")]
        [StringLength(10)]
        public virtual string ReferenceNumber { get; set; }
        [Required]
        public virtual string RecipientName { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8)]
        [RegularExpression(@"[+]?\d*", ErrorMessage = "Please enter a valid phone number.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number.")]
        public virtual string RecipientPhoneNumber { get; set; }
       public virtual string RecipientCompanyName { get; set; }
        public virtual string RecipientDepartmentName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address.")]
        public virtual string RecipientEmailAddress { get; set; }
        [StringLength(12, MinimumLength = 12, ErrorMessage = "The Account ID must be 12 digits.")]
        [Remote("ValidateAccountId", "Account", ErrorMessage = "The Account ID is not valid.")]
        public virtual string RecipientAccountId { get; set; }
        //Either addressid or address
        public virtual int AddressId { get; set; }
        [Required]
        public virtual string Address { get; set; }
        [Required]
        public virtual string ServiceType { get; set; }
        public virtual IList<PackageInputViewModel> Packages { get; set; }
        public virtual int NumberOfPackages { get; set; }
        public virtual string ShipmentPayer { get; set; }
        public virtual string DaTPayer { get; set; }
        public virtual string PickupEmail { get; set; }
        public virtual string DeliverEmail { get; set; }
    }
}