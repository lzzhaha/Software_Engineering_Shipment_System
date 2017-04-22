﻿using SinExWebApp20328381.Models;
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
		
        protected string joinAddress(Address address)
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
            using (var smtp = new SmtpClient())
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

            }
        }

    }

}