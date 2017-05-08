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
        public PackageInputViewModel PackageToPackageViewModel(Package input)
        {
            PackageInputViewModel PackageInputViewModel = new PackageInputViewModel();
            PackageToPackageViewModelWithoutDB(ref input, ref PackageInputViewModel);
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
            return PackageInputViewModel;
        }

        public void PackageToPackageViewModelWithoutDB (ref Package input, ref PackageInputViewModel PackageInputViewModel)
        {
            PackageInputViewModel.Weight = decimal.Round(input.Weight, 1);
            PackageInputViewModel.Value = decimal.Round(input.Value, 2);
            PackageInputViewModel.ValueCurrency = input.ValueCurrency;
            PackageInputViewModel.Description = input.Description;
            PackageInputViewModel.PackageId = input.PackageId;
            PackageInputViewModel.ActualWeight = decimal.Round(input.ActualWeight, 1);
        }
        public Package PackageViewModelToPackage(PackageInputViewModel input)
        {
            Package Package = new Package();
            PackageViewModelToPackageWithoutDB(ref input, ref Package);
            Package.PackageTypeID = db.PackageTypes.SingleOrDefault(s => s.Type == input.PackageType).PackageTypeID;
            if (input.Size != null)
            {
                Package.PackageTypeSizeID = db.PackageTypeSizes.SingleOrDefault(s => (s.size == input.Size && s.PackageTypeID == Package.PackageTypeID)).PackageTypeSizeID;
            }
            else
            {
                Package.PackageTypeSizeID = 0;
            }
            
            return Package;
        }

        public void PackageViewModelToPackageWithoutDB(ref PackageInputViewModel input, ref Package Package)
        {
            Package.Weight = decimal.Round((decimal)input.Weight, 1);
            Package.Value = decimal.Round((decimal)input.Value, 2);
            Package.ValueCurrency = input.ValueCurrency;
            Package.Description = input.Description;
            if (input.PackageId != null)
                Package.PackageId = (int)input.PackageId;
            if (input.ActualWeight != null)
                Package.ActualWeight = decimal.Round((decimal)input.ActualWeight, 1);
            else
                Package.ActualWeight = -1;
        }
        public ShipmentInputViewModel ShipmentToShipmentViewModel(Shipment input)
        {
            var Shipment = new ShipmentInputViewModel();
            ShipmentToShipmentViewModelWithoutDB(ref input, ref Shipment);
            if (long.Parse(input.ShipmentShippingAccountId) == GetCurrentShippingAccount().ShippingAccountId)
            {
                Shipment.ShipmentPayer = "sender";
            }
            else
            {
                Shipment.ShipmentPayer = "recipient";
                Shipment.RecipientAccountId = input.ShipmentShippingAccountId;
            }
            if (long.Parse(input.TaxAndDutyShippingAccountId) == GetCurrentShippingAccount().ShippingAccountId)
            {
                Shipment.DaTPayer = "sender";
            }
            else
            {
                Shipment.DaTPayer = "recipient";
                Shipment.RecipientAccountId = input.TaxAndDutyShippingAccountId;
            }
            Shipment.Packages = new List<PackageInputViewModel>();
            foreach (var Package in input.Packages)
            {
                Shipment.Packages.Add(PackageToPackageViewModel(Package));
            }
            for (int i = Shipment.Packages.Count; i < 10; i++)
            {
                Shipment.Packages.Add(new PackageInputViewModel());
            }
            return Shipment;
        }

        public void ShipmentToShipmentViewModelWithoutDB(ref Shipment input, ref ShipmentInputViewModel Shipment)
        {
            Shipment.ReferenceNumber = input.ReferenceNumber;
            Shipment.RecipientName = input.RecipientName;
            Shipment.RecipientCompanyName = input.RecipientCompanyName;
            Shipment.RecipientDepartmentName = input.RecipientDepartmentName;
            Shipment.RecipientPhoneNumber = input.RecipientPhoneNumber;
            Shipment.RecipientEmailAddress = input.RecipientEmailAddress;
            Shipment.ServiceType = input.ServiceType;
            Shipment.Origin = input.Origin;
            Shipment.Destination = input.Destination;
            Shipment.RecipientBuildingAddress = input.RecipientBuildingAddress;
            Shipment.RecipientCityAddress = input.RecipientCityAddress;
            Shipment.RecipientStreetAddress = input.RecipientStreetAddress;
            Shipment.RecipientPostalCode = input.RecipientPostalCode;
            Shipment.WaybillId = input.WaybillId;
            Shipment.Tax = input.Tax;
            Shipment.Duty = input.Duty;
            Shipment.TaxAuthorizationCode = input.TaxAuthorizationCode;
            Shipment.ShipmentAuthorizationCode = input.ShipmentAuthorizationCode;
            Shipment.TaxCurrency = input.TaxCurrency;
            Shipment.DutyCurrency = input.DutyCurrency;
            Shipment.PickupType = input.PickupType;
            Shipment.PickupAddress = input.PickupAddress;
            Shipment.ShippedDate = input.ShippedDate;
            Shipment.DeliveredDate = input.DeliveredDate;
            Shipment.DeliverEmail = input.EmailWhenDeliver == false ? "0" : "1";
            Shipment.PickupEmail = input.EmailWhenPickup == false ? "0" : "1";
            Shipment.NumberOfPackages = input.NumberOfPackages;
        }

        // usage: when saving or editing shipment
        public Shipment ShipmentViewModelToShipment(ShipmentInputViewModel input)
        {
            var Shipment = new Shipment();
            ShipmentViewModelToShipmentWithoutDB(ref input, ref Shipment);
            Shipment.ShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            if (input.ShipmentPayer == "sender")
            {
                Shipment.ShipmentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId.ToString().PadLeft(12, '0');
            }
            else
            {
                Shipment.ShipmentShippingAccountId = input.RecipientAccountId;
            }
            if (input.DaTPayer == "sender")
            {
                Shipment.TaxAndDutyShippingAccountId = GetCurrentShippingAccount().ShippingAccountId.ToString().PadLeft(12, '0');
            }
            else
            {
                Shipment.TaxAndDutyShippingAccountId = input.RecipientAccountId;
            }
            Shipment.Packages = new List<Package>();
            for (int i = 0; i < input.NumberOfPackages; i++)
            {
                Shipment.Packages.Add(PackageViewModelToPackage(input.Packages[i]));
            }
            //undefined: PickupType, ShippedDate, DeliveredDate
            return Shipment;
        }

        public void ShipmentViewModelToShipmentWithoutDB(ref ShipmentInputViewModel input, ref Shipment Shipment)
        {
            Shipment.ReferenceNumber = input.ReferenceNumber;
            Shipment.RecipientName = input.RecipientName;
            Shipment.RecipientCompanyName = input.RecipientCompanyName;
            Shipment.RecipientDepartmentName = input.RecipientDepartmentName;
            Shipment.RecipientPhoneNumber = input.RecipientPhoneNumber;
            Shipment.RecipientEmailAddress = input.RecipientEmailAddress;
            Shipment.EmailWhenDeliver = input.DeliverEmail == "0" ? false : true;
            Shipment.EmailWhenPickup = input.PickupEmail == "0" ? false : true;
            Shipment.NumberOfPackages = input.NumberOfPackages;
            Shipment.Status = "Saved";
            Shipment.ServiceType = input.ServiceType;
            Shipment.Origin = input.Origin;
            Shipment.Destination = input.Destination;
            Shipment.RecipientBuildingAddress = input.RecipientBuildingAddress;
            Shipment.RecipientCityAddress = input.RecipientCityAddress;
            Shipment.RecipientStreetAddress = input.RecipientStreetAddress;
            Shipment.RecipientPostalCode = input.RecipientPostalCode;
            Shipment.PickupType = input.PickupType;
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