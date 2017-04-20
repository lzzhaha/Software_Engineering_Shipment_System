using SinExWebApp20328381.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SinExWebApp20328381.Controllers
{
    public class BaseController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();
        protected decimal ConvertCurrency(string ToCurrency, decimal OriginValue)
        {
            if (ToCurrency == "")
            {
                ToCurrency = "CNY";
            }
            if (Session[ToCurrency] == null)
            {
                Session[ToCurrency] = db.Currencies.SingleOrDefault(s => s.CurrencyCode == ToCurrency).ExchangeRate;
            }
            return OriginValue * decimal.Parse(Session[ToCurrency].ToString());
        }

        public ShippingAccount GetCurrentShippingAccount()
        {
            string UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if (UserName == null)
            {
                throw new Exception("You're not a valid user.");
            }
            return db.ShippingAccounts.AsNoTracking().SingleOrDefault(s => s.UserName == UserName);
        }
    }
}