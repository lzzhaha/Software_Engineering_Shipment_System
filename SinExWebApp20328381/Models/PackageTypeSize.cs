using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    [Table("PackageTypeSize")]
    public class PackageTypeSize
    {
        public virtual int PackageTypeSizeID { get; set; }
        public virtual string size { get; set; }
        public virtual string limit { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual int PackageTypeID { get; set; }
    }
}