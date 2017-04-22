﻿using System;
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
    [Authorize(Roles = "Employee, Customer")]
    public class ShipmentsController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Shipments
        public ActionResult Index()
        {
            ShippingAccount CurrentShippingAccount = GetCurrentShippingAccount();
            return View(db.Shipments.Where(s => s.ShippingAccountId == CurrentShippingAccount.ShippingAccountId).ToList());
        }

        // GET: Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: Shipments/GenerateHistoryReport
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
            else if ((NewShipment.RecipientCompanyName == null && NewShipment.RecipientDepartmentName != null) || (NewShipment.RecipientCompanyName != null && NewShipment.RecipientDepartmentName == null))
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
            ViewBag.Message = "Successfully Updated.";
            return View(NewShipment);
        }

        private Shipment ShipmentViewModelToShipment(ShipmentInputViewModel input)
        {
            var Shipment = new Shipment();
            Shipment.ReferenceNumber = input.ReferenceNumber;
            Shipment.RecipientName = input.RecipientName;
            Shipment.RecipientCompanyName = input.RecipientCompanyName;
            Shipment.RecipientDepartmentName = input.RecipientDepartmentName;
            Shipment.RecipientPhoneNumber = input.RecipientPhoneNumber;
            Shipment.RecipientEmailAddress = input.RecipientEmailAddress;
            if (input.ShipmentPayer == "sender")
            {
                Shipment.ShipmentShippingAccountId = GetCurrentShippingAccount().ShippingAccountId.ToString();
            }
            else
            {
                Shipment.ShipmentShippingAccountId = input.RecipientAccountId;
            }
            if (input.DaTPayer == "sender")
            {
                Shipment.TaxAndDutyShippingAccountId = GetCurrentShippingAccount().ShippingAccountId.ToString();
            }
            else
            {
                Shipment.TaxAndDutyShippingAccountId = input.RecipientAccountId;
            }
            Shipment.EmailWhenDeliver = input.DeliverEmail == "0" ? false : true;
            Shipment.EmailWhenPickup = input.PickupEmail == "0" ? false : true;
            Shipment.NumberOfPackages = input.NumberOfPackages;
            Shipment.Packages = new List<Package>();
            for (int i = 0; i < input.NumberOfPackages; i++)
            {
                Shipment.Packages.Add(PackageViewModelToPackage(input.Packages[i]));
            }
            Shipment.Status = "Saved";
            Shipment.ShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            Shipment.ServiceType = input.ServiceType;
            Shipment.Origin = input.Origin;
            Shipment.Destination = input.Destination;
            Shipment.RecipientBuildingAddress = input.RecipientBuildingAddress;
            Shipment.RecipientCityAddress = input.RecipientCityAddress;
            Shipment.RecipientStreetAddress = input.RecipientStreetAddress;
            //undefined: PickupType, ShippedDate, DeliveredDate
            return Shipment;
        }
        public JsonResult GetAddress(AddressJson Address)
        {
            var ShippingAccountId = GetCurrentShippingAccount().ShippingAccountId;
            var AddRes = db.Addresses.SingleOrDefault(s => (s.ShippingAccountId == ShippingAccountId && s.NickName == Address.AddressNickName));
            if (AddRes == null) return Json(null);
            Dictionary<string, string> res = new Dictionary<string, string>();
            res["Building"] = AddRes.Building;
            res["Street"] = AddRes.Street;
            res["City"] = AddRes.City;
            res["PostalCode"] = AddRes.PostalCode;
            return Json(res);
        }
        private Package PackageViewModelToPackage (PackageInputViewModel input)
        {
            Package Package = new Package();
            Package.Weight = (decimal)input.Weight;
            Package.Value = (decimal)input.Value;
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
            return Package;
        }

        private PackageInputViewModel PackageToPackageViewModel(Package input)
        {
            PackageInputViewModel PackageInputViewModel = new PackageInputViewModel();
            PackageInputViewModel.Weight = input.Weight;
            PackageInputViewModel.Value = input.Value;
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
            return PackageInputViewModel;
        }
        // POST: Shipments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WaybillId,ReferenceNumber,ServiceType,ShippedDate,DeliveredDate,RecipientName,NumberOfPackages,Origin,Destination,Status,ShippingAccountId,Packages")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipment);
        }
        */
        
        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WaybillId,ReferenceNumber,ServiceType,ShippedDate,DeliveredDate,RecipientName,NumberOfPackages,Origin,Destination,Status,ShippingAccountId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
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
