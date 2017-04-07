using System;
using System.ComponentModel.DataAnnotations;

namespace SinExWebApp20328381.Models
{
    public class ShipmentStatusHistory
    {
        [Key]
        public virtual int ShipmentStatusHistoryId { get; set; }
        public virtual int WaybillId { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime DateAndTime { get; set; }
        public virtual string Description { get; set; }
        public virtual string Location { get; set; }
        public virtual string Remark { get; set; }
    }
}