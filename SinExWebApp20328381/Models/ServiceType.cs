using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SinExWebApp20328381.Models
{
    [Table("ServiceType")]
    public class ServiceType
    {
        public virtual int ServiceTypeID { get; set; }
        public virtual string Type { get; set; }
        public virtual string CutoffTime { get; set; }
        public virtual string DeliveryTime { get; set; }
        public virtual ICollection<ServicePackageFee> ServicePackageFees { get; set; }
    }
}