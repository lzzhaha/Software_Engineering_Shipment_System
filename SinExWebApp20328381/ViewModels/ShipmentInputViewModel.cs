﻿using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class ShipmentInputViewModel
    {
        public virtual ShippingAccount CurrentShippingAccount { get; set; }
        public virtual FeeCheckGenerateViewModel SystemOutputSource { get; set; }
        [RegularExpression("^[0-9a-zA-Z]*$", ErrorMessage = "Please enter alphanumeric string.(10 characters at most)")]
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
        public virtual string RecipientBuildingAddress { get; set; }
        [Required]
        public virtual string RecipientStreetAddress { get; set; }
        [Required]
        public virtual string RecipientCityAddress { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^\d{5,6}$", ErrorMessage = "Please enter a valid postal code.")]
        public virtual string RecipientPostalCode { get; set; }
        public virtual string PickupAddress { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address.")]
        public virtual string RecipientEmailAddress { get; set; }
        [StringLength(12, MinimumLength = 12, ErrorMessage = "The Account ID must be 12 digits.")]
        [Remote("ValidateAccountId", "Account", ErrorMessage = "The Account ID is not valid.")]
        public virtual string RecipientAccountId { get; set; }
        [Required]
        public virtual string ServiceType { get; set; }
        public virtual IList<PackageInputViewModel> Packages { get; set; }
        public virtual int NumberOfPackages { get; set; }
        public virtual string ShipmentPayer { get; set; }
        public virtual string DaTPayer { get; set; }
        public virtual string PickupEmail { get; set; }
        public virtual string DeliverEmail { get; set; }
        [Required]
        public virtual string Origin { get; set; }
        [Required]
        public virtual string Destination { get; set; }
    }
}