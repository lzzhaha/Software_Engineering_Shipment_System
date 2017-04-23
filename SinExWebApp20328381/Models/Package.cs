using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace SinExWebApp20328381.Models
{
    [Table("Package")]
    public class Package
    {
        [Key]
        public virtual int PackageId { get; set; }
        public virtual long WaybillId { get; set; }
        public virtual decimal Weight { get; set; } // to 0.1 decimal place
        public virtual decimal ActualWeight { get; set; }
        public virtual int PackageTypeID { get; set; }
        public virtual int PackageTypeSizeID { get; set; }
        public virtual decimal Value { get; set; }
        public virtual string ValueCurrency { get; set; }
        public virtual string Description { get; set; }

        public PackageType PackageType { get; set; }
        public Package()
        {
            ActualWeight = -1;
        }
    }
}