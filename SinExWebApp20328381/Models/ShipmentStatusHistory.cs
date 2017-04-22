using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SinExWebApp20328381.Models
{
    [Table("ShipmentStatusHistories")]
    public class ShipmentStatusHistory
    {
        [Key]
        public virtual int ShipmentStatusHistoryId { get; set; }
        public virtual long WaybillId { get; set; }
        public virtual DateTime DateAndTime { get; set; }
        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual string Location { get; set; }
        public virtual string Remarks { get; set; }
        [StringLength(70)]
        [RegularExpression(@"^[A-Z]{1}[a-zA-Z]+$",ErrorMessage ="Please enter a valid name of a person")]
        public virtual string DeliveredPerson  { get; set; }
        [Required]
        public virtual string Status  { get; set; }
        public virtual string DeliveredPlace { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}