using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20328381.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        [Key,ForeignKey("shipment")]
        public virtual long InvoiceId { get; set; }
        public virtual long WaybillId { get; set; }
        public virtual string TotalCostCurrency { get; set; }
        public virtual decimal TotalCost { get; set; }
        public virtual long? ShippingAccountId { get; set; }
        public virtual Shipment shipment { get; set;}
    }
}