using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;
using SinExWebApp20328381.ViewModels;
using X.PagedList;

namespace SinExWebApp20328381.Controllers
{
    public class ShipmentsController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Shipments
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            return View(db.Shipments.Where(s => s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId).ToList());
        }
        [Authorize(Roles = "Customer")]
        public ActionResult Confirm(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.Include(s => s.Packages).SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            if (shipment == null)
            {
                return HttpNotFound();
            }
            if (shipment.Status != "Saved")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickupInformationInputViewModel Pickup = new PickupInformationInputViewModel();
            Pickup.SystemOutputSource = new DropdownListsViewModel();
            Pickup.SystemOutputSource = PopulateDrownLists(Pickup.SystemOutputSource);
            Pickup.PickupBuildingAddress = CurrentShippingAccount.MailingAddressBuilding;
            Pickup.PickupCityAddress = CurrentShippingAccount.MailingAddressCity;
            Pickup.PickupStreetAddress = CurrentShippingAccount.MailingAddressStreet;
            Pickup.ShippedDate = DateTime.Now;
            Pickup.PickupType = "immediate";
            return View(Pickup);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(PickupInformationInputViewModel PickupInformation, long id)
        {
            if (PickupInformation.PickupType == "prearranged" && ((PickupInformation.ShippedDate - DateTime.Now).TotalDays < 0 || (PickupInformation.ShippedDate - DateTime.Now).TotalDays > 5))
            {
                PickupInformation.SystemOutputSource = new DropdownListsViewModel();
                PickupInformation.SystemOutputSource = PopulateDrownLists(PickupInformation.SystemOutputSource);
                ModelState.AddModelError("ShippedDate", "Prearranged date should be up to 5 days in advance.");
                return View(PickupInformation);
            }
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            shipment.PickupType = PickupInformation.PickupType;
            shipment.PickupAddress = PickupInformation.PickupStreetAddress + ", " + PickupInformation.PickupCityAddress + ", " + PickupInformation.ServiceCity;
            if (PickupInformation.PickupBuildingAddress != null)
            {
                shipment.PickupAddress = PickupInformation.PickupBuildingAddress + ", " + shipment.PickupAddress;
            }
            //validate
            if (PickupInformation.PickupType == "immediate")
            {
                
                shipment.ShippedDate = DateTime.Now;
            }
            else
            {
                shipment.ShippedDate = PickupInformation.ShippedDate;
            }
            shipment.Status = "Confirmed";
            db.Entry(shipment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Customer")]
        // GET: Shipments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.Include(s => s.Packages).SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ShipmentInputViewModel Res = ShipmentToShipmentViewModel(shipment);
            Res.CurrentShippingAccount = CurrentShippingAccount;
            Res.Destination = db.Destinations.SingleOrDefault(s => s.City == Res.Destination).ProvinceCode; //Change the service city to province code
            Res.SystemOutputSource = new FeeCheckGenerateViewModel();
            Res.SystemOutputSource.Fees = new ServicePackageFeesController().ProcessFeeCheck(Res.ServiceType, Res.Packages);
            return View(Res);
        }

        // GET: Shipments/GenerateHistoryReport
        [Authorize(Roles = "Customer, Employee")]
        public ActionResult GenerateHistoryReport(long? ShippingAccountId, string sortOrder, int? CurrentShippingAccountId, int? page, DateTime? ShippedStartDate, DateTime? ShippedEndDate, DateTime? CurrentShippedStartDate, DateTime? CurrentShippedEndDate)
        {
            // Instantiate an instance of the ShipmentsReportViewModel and the ShipmentsSearchViewModel.
            var shipmentSearch = new ShipmentsReportViewModel();
            shipmentSearch.Shipment = new ShipmentsSearchViewModel();

            //Check the role
            if (System.Web.HttpContext.Current.User.IsInRole("Customer"))
            {
                string userName = System.Web.HttpContext.Current.User.Identity.Name;
                if (userName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == userName);
                if (shippingAccount == null)
                {
                    return HttpNotFound("There is no shipment with user name " + userName + ".");
                }
                ShippingAccountId = shippingAccount.ShippingAccountId;
            }

            // Populate the ShippingAccountId dropdown list.
            shipmentSearch.Shipment.ShippingAccounts = PopulateShippingAccountsDropdownList().ToList();

            ViewBag.CurrentSort = sortOrder;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (ShippingAccountId == null)
            {
                ShippingAccountId = CurrentShippingAccountId;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentShippingAccountId = ShippingAccountId;
            ShippedStartDate = ShippedStartDate == null ? CurrentShippedStartDate : ShippedStartDate;
            ShippedEndDate = ShippedEndDate == null ? CurrentShippedEndDate : ShippedEndDate;
            if (ShippedStartDate == null)
            {
                ViewBag.CurrentShippedStartDate = null;
            }
            else
            {
                ViewBag.CurrentShippedStartDate = Convert.ToDateTime(ShippedStartDate);
            }
            if (ShippedEndDate == null)
            {
                ViewBag.CurrentShippedEndDate = null;
            }
            else
            {
                ViewBag.CurrentShippedEndDate = Convert.ToDateTime(ShippedEndDate);
            }
            if ((Convert.ToDateTime(ShippedStartDate) > Convert.ToDateTime(ShippedEndDate)) || (ShippedStartDate == null && ShippedEndDate != null) || (ShippedStartDate != null && ShippedEndDate == null))
            {
                ViewBag.ErrorMessage = "Date range is invalid.";
                shipmentSearch.Shipments = (new ShipmentsListViewModel[0]).ToPagedList(pageNumber, pageSize);
                return View(shipmentSearch);
            }
            // Initialize the query to retrieve shipments using the ShipmentsListViewModel.
            var shipmentQuery = from s in db.Shipments
                                select new ShipmentsListViewModel
                                {
                                    WaybillId = s.WaybillId,
                                    ServiceType = s.ServiceType,
                                    ShippedDate = s.ShippedDate,
                                    DeliveredDate = s.DeliveredDate,
                                    RecipientName = s.RecipientName,
                                    NumberOfPackages = s.NumberOfPackages,
                                    Origin = s.Origin,
                                    Destination = s.Destination,
                                    ShippingAccountId = (long)s.ShippingAccountId
                                };
            //shipmentQuery.Where()
            // Add the condition to select a spefic shipping account if shipping account id is not null.
            if (ShippingAccountId != null || ShippedStartDate != null || ShippedEndDate != null)
            {

                // TODO: Construct the LINQ query to retrive only the shipments for the specified shipping account id.
                if (ShippingAccountId != null)
                    shipmentQuery = shipmentQuery.Where(c => c.ShippingAccountId == ShippingAccountId);
                if (ShippedStartDate != null)
                    shipmentQuery = shipmentQuery.Where(c => c.ShippedDate >= ShippedStartDate);
                if (ShippedEndDate != null)
                    shipmentQuery = shipmentQuery.Where(c => c.ShippedDate <= ShippedEndDate);
                //only picked up shipment will be displayed
                DateTime dt=new DateTime(1900, 1, 1, 0, 0, 0);
                shipmentQuery = shipmentQuery.Where(c => c.ShippedDate != dt);

                ViewBag.ServiceTypeParm = sortOrder == "ServiceType" ? "ServiceType_dest" : "ServiceType";
                ViewBag.ShippedDateParm = sortOrder == "ShippedDate" ? "ShippedDate_dest" : "ShippedDate";
                ViewBag.DeliveredDateParm = sortOrder == "DeliveredDate" ? "DeliveredDate_dest" : "DeliveredDate";
                ViewBag.RecipentNameParm = sortOrder == "RecipentName" ? "RecipentName_dest" : "RecipentName";
                ViewBag.OriginParm = sortOrder == "Origin" ? "Origin_dest" : "Origin";
                ViewBag.DestinationParm = sortOrder == "Destination" ? "Destination_dest" : "Destination";
                switch (sortOrder)
                {
                    case "ServiceType":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.ServiceType);
                        break;

                    case "ShippedDate":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.ShippedDate);
                        break;

                    case "DeliveredDate":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.DeliveredDate);
                        break;

                    case "RecipentName":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.RecipientName);
                        break;

                    case "Origin":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.Origin);
                        break;

                    case "Destination":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.Destination);
                        break;

                    case "ServiceType_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.ServiceType);
                        break;

                    case "ShippedDate_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippedDate);
                        break;

                    case "DeliveredDate_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.DeliveredDate);
                        break;

                    case "RecipentName_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.RecipientName);
                        break;

                    case "Origin_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.Origin);
                        break;

                    case "Destination_dest":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.Destination);
                        break;

                    default:
                        shipmentQuery = shipmentQuery.OrderBy(s => s.WaybillId);
                        break;


                }
                shipmentSearch.Shipments = shipmentQuery.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                // Return an empty result if no shipping account id has been selected.
                shipmentSearch.Shipments = (new ShipmentsListViewModel[0]).ToPagedList(pageNumber, pageSize);
            }

            return View(shipmentSearch);
        }
        
        private SelectList PopulateShippingAccountsDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var shippingAccountQuery = db.Shipments.Select(s => s.ShippingAccountId).Distinct().OrderBy(c => c);
            return new SelectList(shippingAccountQuery);
        }
        [Authorize(Roles = "Customer")]
        // GET: Shipments/Create
        public ActionResult Create()
        {
            var result = new ShipmentInputViewModel();
            result.NumberOfPackages = 1;
            result.ShipmentPayer = "sender";
            result.DaTPayer = "sender";
            result.PickupEmail = "0";
            result.DeliverEmail = "0";
            var OutputGenerater = new ServicePackageFeesController();
            result.SystemOutputSource = (FeeCheckGenerateViewModel)PopulateDrownLists(new FeeCheckGenerateViewModel());
            result.CurrentShippingAccount = GetCurrentShippingAccount();
            return View(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult Create(ShipmentInputViewModel NewShipment)
        {
            bool InputError = false;
            var OutputGenerater = new ServicePackageFeesController();
            NewShipment.SystemOutputSource = (FeeCheckGenerateViewModel)PopulateDrownLists(new FeeCheckGenerateViewModel());
            NewShipment.CurrentShippingAccount = GetCurrentShippingAccount();

            if (NewShipment.ShipmentPayer == "recipient" || NewShipment.DaTPayer == "recipient")
            {
                if (NewShipment.RecipientAccountId == null)
                {
                    InputError = true;
                }
            }
            if ((NewShipment.RecipientCompanyName == null && NewShipment.RecipientDepartmentName != null) || (NewShipment.RecipientCompanyName != null && NewShipment.RecipientDepartmentName == null))
            {
                ModelState.AddModelError("RecipientCompanyName", "You must either leave company name and department name empty or fill in both of them.");
                InputError = true;
            }

            if (!ModelState.IsValid || InputError == true)
            {
                return View(NewShipment);
            }
            
            NewShipment.SystemOutputSource.Fees = OutputGenerater.ProcessFeeCheck(NewShipment.ServiceType, NewShipment.Packages);
            var ShipmentObject = ShipmentViewModelToShipment(NewShipment);
            db.Shipments.Add(ShipmentObject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private ShipmentInputViewModel ShipmentToShipmentViewModel(Shipment input)
        {
            var Shipment = new ShipmentInputViewModel();
            Shipment.ReferenceNumber = input.ReferenceNumber;
            Shipment.RecipientName = input.RecipientName;
            Shipment.RecipientCompanyName = input.RecipientCompanyName;
            Shipment.RecipientDepartmentName = input.RecipientDepartmentName;
            Shipment.RecipientPhoneNumber = input.RecipientPhoneNumber;
            Shipment.RecipientEmailAddress = input.RecipientEmailAddress;
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
            Shipment.DeliverEmail = input.EmailWhenDeliver == false ? "0" : "1";
            Shipment.PickupEmail = input.EmailWhenPickup == false ? "0" : "1";
            Shipment.NumberOfPackages = input.NumberOfPackages;
            Shipment.Packages = new List<PackageInputViewModel>();
            foreach (var Package in input.Packages)
            {
                Shipment.Packages.Add(PackageToPackageViewModel(Package));
            }
            for (int i = Shipment.Packages.Count; i<10; i++)
            {
                Shipment.Packages.Add(new PackageInputViewModel());
            }
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
            Shipment.AuthorizationCode = input.TaxAuthorizationCode;
            Shipment.TaxCurrency = input.TaxCurreny;
            Shipment.DutyCurrency = input.DutyCurrency;
            Shipment.PickupType = input.PickupType;
            Shipment.PickupAddress = input.PickupAddress;
            Shipment.ShippedDate = input.ShippedDate;
            Shipment.DeliveredDate = input.DeliveredDate;
            return Shipment;
        }

        private Shipment ShipmentViewModelToShipment(ShipmentInputViewModel input)
        {
            var Shipment = new Shipment();
            ShipmentViewModelToShipment(input, ref Shipment);
            //undefined: PickupType, ShippedDate, DeliveredDate
            return Shipment;
        }

        private void ShipmentViewModelToShipment(ShipmentInputViewModel input, ref Shipment Shipment)
        {
            ShipmentViewModelToShipmentWithoutPackage(input, ref Shipment);
            Shipment.Packages = new List<Package>();
            for (int i = 0; i < input.NumberOfPackages; i++)
            {
                Shipment.Packages.Add(PackageViewModelToPackage(input.Packages[i]));
            }
        }

        private void ShipmentViewModelToShipmentWithoutPackage(ShipmentInputViewModel input, ref Shipment Shipment)
        {
            Shipment.ReferenceNumber = input.ReferenceNumber;
            Shipment.RecipientName = input.RecipientName;
            Shipment.RecipientCompanyName = input.RecipientCompanyName;
            Shipment.RecipientDepartmentName = input.RecipientDepartmentName;
            Shipment.RecipientPhoneNumber = input.RecipientPhoneNumber;
            Shipment.RecipientEmailAddress = input.RecipientEmailAddress;
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
            Shipment.EmailWhenDeliver = input.DeliverEmail == "0" ? false : true;
            Shipment.EmailWhenPickup = input.PickupEmail == "0" ? false : true;
            Shipment.NumberOfPackages = input.NumberOfPackages;
            Shipment.Status = "Saved";
            Shipment.ShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            Shipment.ServiceType = input.ServiceType;
            Shipment.Origin = input.Origin;
            Shipment.Destination = input.Destination;
            Shipment.RecipientBuildingAddress = input.RecipientBuildingAddress;
            Shipment.RecipientCityAddress = input.RecipientCityAddress;
            Shipment.RecipientStreetAddress = input.RecipientStreetAddress;
            Shipment.RecipientPostalCode = input.RecipientPostalCode;
            Shipment.PickupType = input.PickupType;
        }
        public JsonResult GetAddress(AddressJson Address)
        {
            var ShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            var AddRes = db.Addresses.SingleOrDefault(s => (s.ShippingAccountId == ShippingAccountId && s.NickName == Address.AddressNickName && s.AddressType == Address.AddressType));
            if (AddRes == null) return Json(null);
            Dictionary<string, string> res = new Dictionary<string, string>();
            res["Building"] = AddRes.Building;
            res["Street"] = AddRes.Street;
            res["City"] = AddRes.City;
            res["PostalCode"] = AddRes.PostalCode;
            res["ServiceCity"] = AddRes.ServiceCity;
            return Json(res);
        }
        private Package PackageViewModelToPackage (PackageInputViewModel input)
        {
            Package Package = new Package();
            Package.Weight = decimal.Round((decimal)input.Weight, 1);
            Package.Value = decimal.Round((decimal)input.Value, 1);
            Package.ValueCurrency = input.ValueCurrency;
            Package.Description = input.Description;
            Package.PackageTypeID = db.PackageTypes.SingleOrDefault(s => s.Type == input.PackageType).PackageTypeID;
            if (input.Size != null)
            {
                Package.PackageTypeSizeID = db.PackageTypeSizes.SingleOrDefault(s => (s.size == input.Size && s.PackageTypeID == Package.PackageTypeID)).PackageTypeSizeID;
            }
            else
            {
                Package.PackageTypeSizeID = 0;
            }
            if (input.PackageId != null)
                Package.PackageId = (int)input.PackageId;
            if (input.ActualWeight != null)
                Package.ActualWeight = (decimal)input.ActualWeight;
            else
                Package.ActualWeight = -1;
            return Package;
        }

        // GET: Shipments/Edit/5
        [Authorize(Roles = "Customer")]
        public ActionResult Edit(long? id)
        {
            ViewBag.WaybillId = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            Shipment shipment = db.Shipments.Include("Packages").SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccountId));
            if (shipment == null || shipment.Status != "Saved")
            {
                return HttpNotFound();
            }
            var NewShipment = ShipmentToShipmentViewModel(shipment);
            bool InputError = false;
            var OutputGenerater = new ServicePackageFeesController();
            NewShipment.SystemOutputSource = (FeeCheckGenerateViewModel)PopulateDrownLists(new FeeCheckGenerateViewModel());
            NewShipment.CurrentShippingAccount = GetCurrentShippingAccount();

            if (NewShipment.ShipmentPayer == "recipient" || NewShipment.DaTPayer == "recipient")
            {
                if (NewShipment.RecipientAccountId == null)
                {
                    InputError = true;
                }
            }
            if ((NewShipment.RecipientCompanyName == null && NewShipment.RecipientDepartmentName != null) || (NewShipment.RecipientCompanyName != null && NewShipment.RecipientDepartmentName == null))
            {
                ModelState.AddModelError("RecipientCompanyName", "You must either leave company name and department name empty or fill in both of them.");
                InputError = true;
            }

            if (!ModelState.IsValid || InputError == true)
            {
                return View(NewShipment);
            }
            NewShipment.SystemOutputSource.Fees = OutputGenerater.ProcessFeeCheck(NewShipment.ServiceType, NewShipment.Packages);
            return View(NewShipment);
        }

        // POST: Shipments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Edit(ShipmentInputViewModel NewShipment, long WaybillId)
        {
            bool InputError = false;
            var OutputGenerater = new ServicePackageFeesController();
            NewShipment.SystemOutputSource = (FeeCheckGenerateViewModel)PopulateDrownLists(new FeeCheckGenerateViewModel());
            NewShipment.CurrentShippingAccount = GetCurrentShippingAccount();
            ViewBag.WaybillId = WaybillId;
            if (NewShipment.ShipmentPayer == "recipient" || NewShipment.DaTPayer == "recipient")
            {
                if (NewShipment.RecipientAccountId == null)
                {
                    InputError = true;
                }
            }
            if ((NewShipment.RecipientCompanyName == null && NewShipment.RecipientDepartmentName != null) || (NewShipment.RecipientCompanyName != null && NewShipment.RecipientDepartmentName == null))
            {
                ModelState.AddModelError("RecipientCompanyName", "You must either leave company name and department name empty or fill in both of them.");
                InputError = true;
            }

            if (!ModelState.IsValid || InputError == true)
            {
                return View(NewShipment);
            }

            NewShipment.SystemOutputSource.Fees = OutputGenerater.ProcessFeeCheck(NewShipment.ServiceType, NewShipment.Packages);
            var DBShipment = db.Shipments.Include("Packages").SingleOrDefault(s => s.WaybillId == WaybillId);
            //ShipmentViewModelToShipmentWithoutPackage(NewShipment, ref DBShipment);
            //db.Entry(DBShipment).State = EntityState.Modified;
            var shipment = ShipmentViewModelToShipment(NewShipment);
            shipment.WaybillId = WaybillId;
            db.Entry(DBShipment).CurrentValues.SetValues(shipment);
            foreach (var existingPackage in DBShipment.Packages.ToList())
            {
                if (!shipment.Packages.Any(s => s.PackageId == existingPackage.PackageId))
                    db.Packages.Remove(existingPackage);
            }
            db.SaveChanges();
            foreach (var packageModel in shipment.Packages)
            {
                db.Entry(DBShipment).Reload();
                var existingPackage = DBShipment.Packages.SingleOrDefault(s => s.PackageId == packageModel.PackageId);
                packageModel.WaybillId = DBShipment.WaybillId;
                if (existingPackage != null)
                {
                    db.Entry(existingPackage).CurrentValues.SetValues(packageModel);
                }
                else
                {
                    var newPackage = new Package();
                    newPackage = packageModel;
                    DBShipment.Packages.Add(newPackage);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
            ViewBag.Message = "Successfully Updated.";
            //following code is going to update the packages (we need package id)
            db.Entry(DBShipment).Reload();
            NewShipment.Packages = new List<PackageInputViewModel>();
            foreach (var Package in DBShipment.Packages)
            {
                NewShipment.Packages.Add(PackageToPackageViewModel(Package));
            }
            for (int i = NewShipment.Packages.Count; i < 10; i++)
            {
                NewShipment.Packages.Add(new PackageInputViewModel());
            }
            return View(NewShipment);
        }

        // GET: Shipments/Delete/5
        [Authorize(Roles = "Customer")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.Include(s => s.Packages).SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            if (shipment == null)
            {
                return HttpNotFound();
            }
            if (shipment.Status != "Saved")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentInputViewModel Res = ShipmentToShipmentViewModel(shipment);
            Res.CurrentShippingAccount = CurrentShippingAccount;
            Res.Destination = db.Destinations.SingleOrDefault(s => s.City == Res.Destination).ProvinceCode; //Change the service city to province code
            Res.SystemOutputSource = new FeeCheckGenerateViewModel();
            Res.SystemOutputSource.Fees = new ServicePackageFeesController().ProcessFeeCheck(Res.ServiceType, Res.Packages);
            return View(Res);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteConfirmed(long id)
        {
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.Include(s => s.Packages).SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            db.Shipments.Remove(shipment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Cancel(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.Include(s => s.Packages).SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            if (shipment == null)
            {
                return HttpNotFound();
            }
            if (shipment.Status != "Confirmed")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentInputViewModel Res = ShipmentToShipmentViewModel(shipment);
            Res.CurrentShippingAccount = CurrentShippingAccount;
            Res.Destination = db.Destinations.SingleOrDefault(s => s.City == Res.Destination).ProvinceCode; //Change the service city to province code
            Res.SystemOutputSource = new FeeCheckGenerateViewModel();
            Res.SystemOutputSource.Fees = new ServicePackageFeesController().ProcessFeeCheck(Res.ServiceType, Res.Packages);
            return View(Res);
        }
        
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult CancelConfirmed(long id)
        {
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            Shipment shipment = db.Shipments.SingleOrDefault(s => (s.WaybillId == id && s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId));
            shipment.Status = "Cancelled";
            db.Entry(shipment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
