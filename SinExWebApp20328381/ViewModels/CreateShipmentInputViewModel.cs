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
        public virtual FeeCheckGenerateViewModel ShipmentInformation { get; set; }
        public virtual string ReferenceNumber { get; set; }
        public virtual string RecipientName { get; set; }
        [StringLength(14, MinimumLength = 8)]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid phone number.")]
        [DataType("PhoneNumber", ErrorMessage = "Please enter a valid phone number.")]
        public virtual string RecipientPhoneNumber { get; set; }
       public virtual string RecipientCompanyName { get; set; }
        public virtual string RecipientDepartmentName { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid Email address.")]
        public virtual string RecipientEmailAddress { get; set; }
        [StringLength(12, MinimumLength = 12, ErrorMessage = "The Account ID must be 12 digits.")]
        public virtual string RecipientAccountId { get; set; }
        //Either addressid or address
        public virtual int AddressId { get; set; }
        public virtual string Address { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual IList<PackageInputViewModel> Packages { get; set; }
    }
}