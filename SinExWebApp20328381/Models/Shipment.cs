using SinExWebApp20328381.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.Models
{
    [Table("Shipment")]
    public class Shipment
    {
        [Key]
        //[RegularExpression(@"^\d{16}$", ErrorMessage ="Please enter a 16-digit waybill number!")]
        public virtual long WaybillId { get; set; }
        public virtual string ReferenceNumber { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string PickupType { get; set; }
        public virtual DateTime ShippedDate { get; set; }// the same as pickup date
        public virtual DateTime DeliveredDate { get; set; }
        public virtual string DeliveredPlace { get; set; }
        public virtual string DeliveredPerson { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual int NumberOfPackages { get; set; }
        public ICollection<Package> Packages { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual string Status { get; set; } // 5 status: Saved, Confirmed, PickedUp, Delivered, Cancelled
        public virtual long? ShippingAccountId { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual string TaxCurreny { get; set; }
        public virtual decimal Duty { get; set; }
        public virtual string DutyCurrency { get; set; }
        public virtual string TaxAndDutyShippingAccountId { get; set; }
        public virtual string ShipmentShippingAccountId { get; set; }
        public virtual bool EmailWhenPickup { get; set; }
        public virtual bool EmailWhenDeliver { get; set; }
        public virtual string RecipientPhoneNumber { get; set; }
        public virtual string RecipientCompanyName { get; set; }
        public virtual string RecipientDepartmentName { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid Email address.")]
        public virtual string RecipientEmailAddress { get; set; }
        public virtual string RecipientBuildingAddress { get; set; }
        public virtual string RecipientStreetAddress { get; set; }
        public virtual string RecipientCityAddress { get; set; }
        public virtual string RecipientPostalCode { get; set; }
        public virtual string PickupAddress { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Invalid Code.")]
        public virtual string TaxAuthorizationCode { get; set; } // payment information
        public virtual string ShipmentAuthorizationCode { get; set; } // payment information
        public virtual ICollection<ShipmentStatusHistory> ShipmentStatusHistory { get; set; }
        public virtual ShippingAccount ShippingAccount { get; set; }
        public virtual Invoice invoice { get; set; }

        public Shipment()
        {
            ShippedDate = new DateTime(1900, 1, 1, 0, 0, 0);
            DeliveredDate = new DateTime(1900, 1, 1, 0, 0, 0);
            ReferenceNumber = "";
            Tax = -1;
            Duty = -1;
        }
    }
}