using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace SinExWebApp20328381.ViewModels
{
    public class InvoiceReportViewModel
    {
        public virtual InvoiceSearchViewModel Invoice { get; set; }
        public virtual IPagedList<InvoiceListViewModel> Invoices { get; set; }
    }

    public class InvoiceListViewModel
    {
        public virtual long WaybillId { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual DateTime ShippedDate { get; set; }
        public virtual DateTime DeliveredDate { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual decimal TotalInvoiceAmount { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual long ShippingAccountId { get; set; }
    }

    public class InvoiceSearchViewModel
    {
        public virtual int ShippingAccountId { get; set; }
        public virtual DateTime ShippedStartDate { get; set; }
        public virtual DateTime ShippedEndDate { get; set; }
        public virtual DateTime DeliveredStartDate { get; set; }
        public virtual DateTime DeliveredEndDate { get; set; }
        public virtual List<SelectListItem> ShippingAccounts { get; set; }
    }
}