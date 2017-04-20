﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    [Table("Shipment")]
    public class Shipment
    {
        [Key]
        [Range(1000000000000000L, 9999999999999999L, ErrorMessage = "Please enter a valid Waybill number")]
        public virtual long WaybillId { get; set; }
        [StringLength(10),MinLength(10)]
        [RegularExpression("^[a - zA - Z0 - 9] *$")]
        public virtual string ReferenceNumber { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string PickupType { get; set; }
        public virtual DateTime ShippedDate { get; set; }
        public virtual DateTime DeliveredDate { get; set; }
        public virtual string DeliveredPlace { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual int NumberOfPackages { get; set; }
        
        public ICollection<Package> Packages { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual string Status { get; set; }
        public virtual int ShippingAccountId { get; set; }
        public virtual int AddressId { get; set; }
        public virtual string TaxAndDutyShippingAccountId { get; set; }
        public virtual string ShipmentShippingAccountId { get; set; }
        public virtual bool EmailWhenPickup { get; set; }
        public virtual bool EmailWhenDeliver { get; set; }
        public virtual string RecipientPhoneNumber { get; set; }
        public virtual string RecipientCompanyName { get; set; }
        public virtual string RecipientDepartmentName { get; set; }
        public virtual ICollection<ShipmentStatusHistory> ShipmentStatusHistory { get; set; }
        public virtual ShippingAccount ShippingAccount { get; set; }
    }
}