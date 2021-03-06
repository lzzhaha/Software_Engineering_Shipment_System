﻿using SinExWebApp20328381.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class FeeCheckGenerateViewModel : DropdownListsViewModel
    {
        public virtual int PackageNumber { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string PackageType { get; set; }
        public virtual string Size { get; set; }
        public virtual List<Decimal> Fees { get; set; }
    }
}