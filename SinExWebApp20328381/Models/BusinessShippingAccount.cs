using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    public class BusinessShippingAccount:ShippingAccount
    {
        [Required]
        [StringLength(70)]
        [RegularExpression("^[A-Za-z]+$",ErrorMessage ="Please enter a valid person name")]
        public virtual string ContactPersonName { get; set; }
        [Required]
        [StringLength(40)]
        public virtual string CompanyName { get; set; }
        [StringLength(30)]
        public virtual string DepartmentName { get; set; }
        /*public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //string date =  CreditCardExpiryMonth.ToString() + "/15/"+ CreditCardExpiryYear;
            DateTime dt = new DateTime(Int32.Parse(CreditCardExpiryYear), CreditCardExpiryMonth, 15);
            var pDate = new[] { "CreditCardExpiryYear" };
            if (CreditCardExpiryYear == "2017")
            {

                yield return new ValidationResult("The card is expired.", pDate);
            }
        }*/
    }
}