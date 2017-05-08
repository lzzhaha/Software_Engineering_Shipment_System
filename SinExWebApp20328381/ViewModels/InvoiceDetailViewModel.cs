using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.ViewModels
{
    public class InvoiceDetailViewModel
    {
        public Shipment shipment { get; set; }
        public Invoice invoice { get; set; }
        public ShippingAccount payer { get; set; }
        public ShippingAccount sender { get; set; }
        public string senderName { get; set; }
        public virtual IList<PackageInputViewModel> Packages { get; set; }
        public virtual int NumberOfPackages { get; set; }
        public virtual FeeCheckGenerateViewModel SystemOutputSource { get; set; }
        public string mailingAddress { get; set; }
        public string deliveryAddress { get; set; }
        public string creditCardNumber { get; set; }
        
    }
}