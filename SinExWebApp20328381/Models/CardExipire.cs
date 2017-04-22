using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace SinExWebApp20328381.Models
{
    public class CardExipire:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var shippingAccount = (ShippingAccount)validationContext.ObjectInstance;
                int y = int.Parse(shippingAccount.CreditCardExpiryYear);
                int m = shippingAccount.CreditCardExpiryMonth;
                if (y < DateTime.Now.Year)
                    return new ValidationResult("The card is expired.");
                if (y == DateTime.Now.Year && m < DateTime.Now.Month)
                    return new ValidationResult("The card is expired.");
            }
            return ValidationResult.Success;
            
        }
    }
}