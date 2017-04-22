using SinExWebApp20328381.Models;
using SinExWebApp20328381.ViewModels;
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

        protected ShippingAccount GetCurrentShippingAccount()
        {
            string UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if (UserName == null)
            {
                throw new Exception("You're not a valid user.");
            }
            return db.ShippingAccounts.AsNoTracking().SingleOrDefault(s => s.UserName == UserName);
		}
		
        protected string joinAddress(Address address)
        {

            return address.Building + "," + address.City + "," + address.ServiceCity;
        }
        
        protected DropdownListsViewModel PopulateDrownLists(DropdownListsViewModel DropdownListsViewModel)
        {
            DropdownListsViewModel.Destinations = PopulateDestinationsDropdownList().ToList();
            DropdownListsViewModel.ServiceTypes = PopulateServiceTypesDropdownList().ToList();
            DropdownListsViewModel.PackageTypes = PopulatePackageTypesDropdownList().ToList();
            DropdownListsViewModel.Addresses = PopulateAddressDropdownList().ToList();
            DropdownListsViewModel.Exchange = db.Currencies.Select(s => s);
            return DropdownListsViewModel;
        }

        protected SelectList PopulatePackageTypesDropdownList()
        {
            var PackageTypeQuery = db.PackageTypes.Select(s => s.Type).Distinct().OrderBy(s => s);
            return new SelectList(PackageTypeQuery);
        }

        protected SelectList PopulateServiceTypesDropdownList()
        {
            var ServiceTypeQuery = db.ServiceTypes.Select(s => s.Type).Distinct().OrderBy(s => s);
            return new SelectList(ServiceTypeQuery);
        }

        protected SelectList PopulateDestinationsDropdownList()
        {
            var DestinationQuery = db.Destinations.Select(s => s.City).Distinct().OrderBy(s => s);
            return new SelectList(DestinationQuery);
        }
        protected SelectList PopulateCurrenciesDropdownList()
        {
            var CurrencyQuery = db.Currencies.Select(s => s.CurrencyCode).Distinct().OrderBy(s => s);
            return new SelectList(CurrencyQuery);
        }

        protected SelectList PopulateAddressDropdownList()
        {
            var CurrentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            var AddressQuery = db.Addresses.Where(s => s.ShippingAccountId == CurrentShippingAccountId).Select(s => s.NickName).Distinct().OrderBy(s => s);
            return new SelectList(AddressQuery);
        }
    }

}