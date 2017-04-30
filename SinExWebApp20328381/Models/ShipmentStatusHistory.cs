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
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/mm/yy}",ApplyFormatInEditMode =true)]
        public virtual DateTime Date { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString ="{0:hh:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime? Time { get ; set; }

        [Required]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Enter the time in HH:mm 24-hour format! ")]
        public virtual string TimeValue {
            get {
                 return Time.HasValue ? Time.Value.ToString("HH:mm") : string.Empty;
            }

            set
            {
                Time = DateTime.Parse(value);
            }
        }

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