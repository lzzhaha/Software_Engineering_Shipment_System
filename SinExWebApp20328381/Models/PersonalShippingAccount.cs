using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20328381.Models
{
    public class PersonalShippingAccount:ShippingAccount
    {
        [Required]
        [StringLength(35)]
        [Display(Name = "First Name", Order = 1)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(35)]
        [Display(Name = "Last Name", Order = 2)]
        public virtual string LastName { get; set; }
        /*
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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