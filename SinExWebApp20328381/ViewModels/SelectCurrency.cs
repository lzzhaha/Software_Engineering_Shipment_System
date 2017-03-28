using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class SelectCurrency
    {
        public virtual List<SelectListItem> Currencies { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual IEnumerable<ServicePackageFee> ServicePackageFees { get; set; }
        public virtual ServicePackageFee ServicePackageFee { get; set; }
    }
}