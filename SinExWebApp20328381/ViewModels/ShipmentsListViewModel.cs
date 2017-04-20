using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.ViewModels
{
    public class ShipmentsListViewModel
    {
        public virtual long WaybillId { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual DateTime ShippedDate { get; set; }
        public virtual DateTime DeliveredDate { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual int NumberOfPackages { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual long ShippingAccountId { get; set; }
    }
}