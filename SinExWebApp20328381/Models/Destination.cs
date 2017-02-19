using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    [Table("Destination")]
    public class Destination
    {
        public string City { get; set; }
        public string ProvinceCode { get; set; }
        [Key]
        public virtual int DestinationKey { get; set; }
        public virtual string CurrencyCode { get; set; }
        //navigation
        public virtual Currency Currency { get; set; }
    }
}