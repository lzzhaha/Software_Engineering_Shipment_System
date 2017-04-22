using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class RegisterCustomerViewModel
    {
        public PersonalShippingAccount PersonalInformation { get; set; }
        public BusinessShippingAccount BusinessInformation { get; set; }
        public RegisterViewModel LoginInformation { get; set; }

        public virtual List<SelectListItem> ProvinceList { get; set; }
    }
}