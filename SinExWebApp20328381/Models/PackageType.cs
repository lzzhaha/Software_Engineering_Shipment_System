using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    [Table("PackageType")]
    public class PackageType
    {
        public virtual int PackageTypeID { get; set; }
        public virtual string Type { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<ServicePackageFee> ServicePackageFees { get; set; }
        public virtual ICollection<PackageTypeSize>  PackageTypeSizes { get; set; }
        public virtual ICollection<Package> Packages { get; set; }

        public static explicit operator PackageType(string v)
        {
            throw new NotImplementedException();
        }
    }
}