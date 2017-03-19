using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class FeeCheckInputViewModel
    {
        public virtual List<SelectListItem> PackageTypes { get; set; }
        public virtual string PackageType { get; set; }
        //public virtual string CurrencyCode { get; set; }
        public virtual Decimal Weight { get; set; }
        public virtual string Size { get; set; }
    }
}