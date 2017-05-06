using SinExWebApp20328381.Models;
using SinExWebApp20328381.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Net;
using System.Net.Mail;

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
		
        public string joinAddress(Address address)
        {

            return address.Building + "," + address.City + "," + address.ServiceCity;
        }
        
        protected DropdownListsViewModel PopulateDrownLists(DropdownListsViewModel DropdownListsViewModel)
        {
            DropdownListsViewModel.Destinations = PopulateDestinationsDropdownList().ToList();
            DropdownListsViewModel.ServiceTypes = PopulateServiceTypesDropdownList().ToList();
            DropdownListsViewModel.PackageTypes = PopulatePackageTypesDropdownList().ToList();
            DropdownListsViewModel.RecipientAddresses = PopulateRecipientAddressDropdownList().ToList();
            DropdownListsViewModel.PickupLocations = PopulatePickupLocationDropdownList().ToList();
            DropdownListsViewModel.Exchange = db.Currencies.Select(s => s);
            return DropdownListsViewModel;
        }

        protected DropdownListsViewModel PopulateDrownLists4Feecheck(DropdownListsViewModel DropdownListsViewModel)
        {
            DropdownListsViewModel.Destinations = PopulateDestinationsDropdownList().ToList();
            DropdownListsViewModel.ServiceTypes = PopulateServiceTypesDropdownList().ToList();
            DropdownListsViewModel.PackageTypes = PopulatePackageTypesDropdownList().ToList();
            //DropdownListsViewModel.RecipientAddresses = PopulateRecipientAddressDropdownList().ToList();
            //DropdownListsViewModel.PickupLocations = PopulatePickupLocationDropdownList().ToList();
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

        protected SelectList PopulateRecipientAddressDropdownList()
        {
            var CurrentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            var AddressQuery = db.Addresses.Where(s => (s.ShippingAccountId == CurrentShippingAccountId && s.AddressType == "Recipient Address")).Select(s => s.NickName).Distinct().OrderBy(s => s);
            return new SelectList(AddressQuery);
        }

        protected SelectList PopulatePickupLocationDropdownList()
        {
            var CurrentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            var AddressQuery = db.Addresses.Where(s => (s.ShippingAccountId == CurrentShippingAccountId && s.AddressType == "Pickup Location")).Select(s => s.NickName).Distinct().OrderBy(s => s);
            return new SelectList(AddressQuery);
        }
        protected  bool sendEmail(MailMessage message)
        {
            /*using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "comp3111_team105@cse.ust.hk",  // replace with valid value
                    Password = "team105#"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.cse.ust.hk";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                return true;

            }*/
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            emailServer.Send(message);
            return true;
        }
        protected PackageInputViewModel PackageToPackageViewModel(Package input)
        {
            PackageInputViewModel PackageInputViewModel = new PackageInputViewModel();
            PackageInputViewModel.Weight = decimal.Round(input.Weight, 1);
            PackageInputViewModel.Value = decimal.Round(input.Value, 2);
            PackageInputViewModel.ValueCurrency = input.ValueCurrency;
            PackageInputViewModel.Description = input.Description;
            PackageType PackageTypeRes = db.PackageTypes.SingleOrDefault(s => s.PackageTypeID == input.PackageTypeID);
            PackageInputViewModel.PackageType = PackageTypeRes.Type;
            if (PackageTypeRes.PackageTypeSizes.Count != 0)
            {
                PackageInputViewModel.Size = PackageTypeRes.PackageTypeSizes.SingleOrDefault(s => s.PackageTypeSizeID == input.PackageTypeSizeID).size;
            }
            else
            {
                PackageInputViewModel.Size = null;
            }
            PackageInputViewModel.PackageId = input.PackageId;
            PackageInputViewModel.ActualWeight = decimal.Round(input.ActualWeight, 1);
            return PackageInputViewModel;
        }

        // Mimicing Credit card authorization system
        protected Tuple<bool,int> creditCard_request(string CardNum, string SecurityNum, decimal ChargeAmount) {
            int min = 1000;
            int max = 9999;
            Random rdm = new Random();
            int authCode = rdm.Next(min,max);
            return Tuple.Create(true,authCode);
        }
        public void AddressObjectMap(ref Address output, Address input)
        {
            output.NickName = input.NickName;
            output.PostalCode = input.PostalCode;
            output.ServiceCity = input.ServiceCity;
            output.Street = input.Street;
            output.City = input.City;
            output.Building = input.Building;
            output.AddressType = input.AddressType;
        }
    }

}