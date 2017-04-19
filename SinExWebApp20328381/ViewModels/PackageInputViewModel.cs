using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20328381.ViewModels
{
    public class PackageInputViewModel
    {
        public virtual string PackageType { get; set; }
        public virtual Decimal? Weight { get; set; }
        public virtual string Size { get; set; }
        public virtual decimal? Value { get; set; }
        public virtual string Description { get; set; }
    }
}