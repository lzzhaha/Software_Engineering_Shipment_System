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
        [RegularExpression(@"^\d{1,18}(\.\d{1})?$", ErrorMessage = "Please enter a valid decimal.")]
        public virtual decimal? Weight { get; set; }
        public virtual string Size { get; set; }
        [RegularExpression(@"^\d{1,18}(\.\d{1})?$", ErrorMessage = "Please enter a valid decimal.")]
        public virtual decimal? Value { get; set; }
        public virtual string ValueCurrency { get; set; }
        public virtual string Description { get; set; }
        public virtual int? PackageId { get; set; }
    }
}