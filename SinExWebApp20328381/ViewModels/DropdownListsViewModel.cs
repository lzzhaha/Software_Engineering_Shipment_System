using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class DropdownListsViewModel
    {
        public virtual List<SelectListItem> Destinations { get; set; }
        public virtual List<SelectListItem> ServiceTypes { get; set; }
        public virtual List<SelectListItem> PackageTypes { get; set; }
        public virtual List<SelectListItem> RecipientAddresses { get; set; }
        public virtual List<SelectListItem> PickupLocations { get; set; }
        public virtual IEnumerable<Currency> Exchange { get; set; }
    }
}