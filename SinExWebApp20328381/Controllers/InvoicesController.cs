﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using SinExWebApp20328381.ViewModels;
using X.PagedList;

namespace SinExWebApp20328381.Controllers
{
    public class InvoicesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.shipment);
            return View(invoices.ToList());
        }

        public ActionResult GenerateInvoiceReport(long? ShippingAccountId, string sortOrder, int? CurrentShippingAccountId, int? page, DateTime? ShippedStartDate, DateTime? ShippedEndDate, DateTime? CurrentShippedStartDate, DateTime? CurrentShippedEndDate,decimal? CurrentTotalInvoiceAmount)
        {
            // Instantiate an instance of the ShipmentsReportViewModel and the ShipmentsSearchViewModel.
            var invoiceSearch = new InvoiceReportViewModel();
            invoiceSearch.Invoice = new InvoiceSearchViewModel();

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
            invoiceSearch.Invoice.ShippingAccounts = PopulateShippingAccountsDropdownList().ToList();

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
                invoiceSearch.Invoices = (new InvoiceListViewModel[0]).ToPagedList(pageNumber, pageSize);
                return View(invoiceSearch);
            }
            // Initialize the query to retrieve shipments using the ShipmentsListViewModel.
            var invoiceQuery = from s in db.Invoices
                                select new InvoiceListViewModel()
                                {
                                    WaybillId = s.WaybillId,
                                    ServiceType = s.shipment.ServiceType,
                                    ShippedDate = s.shipment.ShippedDate,
                                    DeliveredDate = s.shipment.DeliveredDate,
                                    RecipientName = s.shipment.RecipientName,
                                    TotalInvoiceAmount = s.TotalCost,
                                    Origin = s.shipment.Origin,
                                    Destination = s.shipment.Destination,
                                    ShippingAccountId = (long)s.ShippingAccountId
                                };
            //shipmentQuery.Where()
            // Add the condition to select a spefic shipping account if shipping account id is not null.
            if (ShippingAccountId != null || ShippedStartDate != null || ShippedEndDate != null)
            {

                // TODO: Construct the LINQ query to retrive only the shipments for the specified shipping account id.
                if (ShippingAccountId != null)
                    invoiceQuery = invoiceQuery.Where(c => c.ShippingAccountId == ShippingAccountId);
                if (ShippedStartDate != null)
                    invoiceQuery = invoiceQuery.Where(c => c.ShippedDate >= ShippedStartDate);
                if (ShippedEndDate != null)
                    invoiceQuery = invoiceQuery.Where(c => c.ShippedDate <= ShippedEndDate);
                //only picked up shipment will be displayed
                DateTime dt = new DateTime(1900, 1, 1, 0, 0, 0);
                invoiceQuery = invoiceQuery.Where(c => c.ShippedDate != dt);

                ViewBag.ServiceTypeParm = sortOrder == "ServiceType" ? "ServiceType_dest" : "ServiceType";
                ViewBag.ShippedDateParm = sortOrder == "ShippedDate" ? "ShippedDate_dest" : "ShippedDate";
                ViewBag.DeliveredDateParm = sortOrder == "DeliveredDate" ? "DeliveredDate_dest" : "DeliveredDate";
                ViewBag.RecipentNameParm = sortOrder == "RecipentName" ? "RecipentName_dest" : "RecipentName";
                ViewBag.OriginParm = sortOrder == "Origin" ? "Origin_dest" : "Origin";
                ViewBag.DestinationParm = sortOrder == "Destination" ? "Destination_dest" : "Destination";
                ViewBag.CostParm = sortOrder == "Cost" ? "Cost_dest" : "Cost";
                switch (sortOrder)
                {
                    case "Cost":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.TotalInvoiceAmount);
                        break;
                    case "ServiceType":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.ServiceType);
                        break;

                    case "ShippedDate":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.ShippedDate);
                        break;

                    case "DeliveredDate":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.DeliveredDate);
                        break;

                    case "RecipentName":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.RecipientName);
                        break;

                    case "Origin":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.Origin);
                        break;

                    case "Destination":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.Destination);
                        break;
                    case "Cost_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.TotalInvoiceAmount);
                        break;
                    case "ServiceType_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.ServiceType);
                        break;

                    case "ShippedDate_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.ShippedDate);
                        break;

                    case "DeliveredDate_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.DeliveredDate);
                        break;

                    case "RecipentName_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.RecipientName);
                        break;

                    case "Origin_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.Origin);
                        break;

                    case "Destination_dest":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.Destination);
                        break;

                    default:
                        invoiceQuery = invoiceQuery.OrderBy(s => s.WaybillId);
                        break;


                }
                invoiceSearch.Invoices = invoiceQuery.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                // Return an empty result if no shipping account id has been selected.
                invoiceSearch.Invoices = (new InvoiceListViewModel[0]).ToPagedList(pageNumber, pageSize);
            }

            return View(invoiceSearch);
        }

        private SelectList PopulateShippingAccountsDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var shippingAccountQuery = db.Shipments.Select(s => s.ShippingAccountId).Distinct().OrderBy(c => c);
            return new SelectList(shippingAccountQuery);
        }
        // GET: Invoices/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,WaybillId,TotalCostCurrency,TotalCost,ShippingAccountId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,WaybillId,TotalCostCurrency,TotalCost,ShippingAccountId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
        //[Authorize(Roles = "Employee")]
        public ActionResult SearchWayBill()
        {
            var selectIetm = db.Shipments.Select(x => new SelectListItem() { Value = x.WaybillId.ToString(), Text = x.WaybillId.ToString() }).ToList();
            ViewBag.waybillID = selectIetm;
            Invoice invoice = new Invoice();
            return View(invoice);
        }
        [HttpGet]
        public ActionResult FillActualWeight(string WaybillId)
        {
            long waybillID = Convert.ToInt64(WaybillId);
            Shipment shipment=db.Shipments.Include("Packages").SingleOrDefault(s => s.WaybillId == waybillID);
            
            Invoice invoice = new Invoice();
            if (shipment.Status == "Saved" || shipment.Status == " Confirmed")
            {
                ViewBag.NoPickup = "Yes";
                return View(invoice);
            }
            else
            {
                ViewBag.NoPickup = "No";
            }
            //check if the waybillId already invoiced
            Invoice invoicecheck = db.Invoices.SingleOrDefault(s => s.WaybillId == waybillID);
            if (invoicecheck != null)
            {
                ViewBag.IsInvoiced = "Yes";
                return View(invoice);
            }
            else
            {
                ViewBag.IsInvoiced = "No";
            }
           
            
            foreach (var package in shipment.Packages)
            {
                package.PackageType = db.PackageTypes.SingleOrDefault(s => s.PackageTypeID == package.PackageTypeID);
            }
            invoice.shipment = shipment;
            invoice.WaybillId = waybillID;
            ViewData["waybillID"] = WaybillId;
            return View(invoice);
        }

        [HttpPost]
        public ActionResult FillActualWeight(Invoice invoice)
        {
            long waybillID = Convert.ToInt64(invoice.WaybillId);
            Shipment shipment = db.Shipments.Include("Packages").SingleOrDefault(s => s.WaybillId == waybillID);

            invoice.ShippingAccountId = shipment.ShippingAccountId;
            int i = 0;
            foreach (var package in shipment.Packages)
            {
                package.ActualWeight = invoice.shipment.Packages.ToList()[i].ActualWeight;
                i = i + 1;
            }
            shipment.Tax = invoice.shipment.Tax;
            shipment.Duty = invoice.shipment.Duty;
            long shipmentShippingAccountId = Convert.ToInt64(shipment.ShipmentShippingAccountId);
            var provinceCode = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == shipmentShippingAccountId).MailingAddressProvinceCode;
            invoice.TotalCostCurrency = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode).CurrencyCode;
            long taxShippingAccountID = Convert.ToInt64(shipment.TaxAndDutyShippingAccountId);
            var provinceCode2 = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxShippingAccountID).MailingAddressProvinceCode;
            shipment.TaxCurreny = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode2).CurrencyCode;
            shipment.DutyCurrency = shipment.TaxCurreny;
            List<PackageInputViewModel> Packages = new List<PackageInputViewModel>();
            foreach (var item in shipment.Packages)
            {
                var temp = PackageToPackageViewModel(item);
                Packages.Add(temp);
            }
            var fees = new ServicePackageFeesController().ProcessFeeCheck(shipment.ServiceType, Packages);
            int feeCount = 0;
            foreach (var item in shipment.Packages)
            {
                item.Cost = fees[feeCount];
                item.PackageType = db.PackageTypes.SingleOrDefault(s => s.PackageTypeID == item.PackageTypeID);
                feeCount = feeCount + 1;
            }
            invoice.TotalCost = fees.Last();
            invoice.shipment = shipment;
            if (invoice.shipment.TaxAndDutyShippingAccountId != invoice.shipment.ShipmentShippingAccountId)
            {
                long shipment_fee_id = Convert.ToInt64(invoice.shipment.ShipmentShippingAccountId);
                ShippingAccount shipment_fee_account = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == shipment_fee_id);
                string province = db.Destinations.FirstOrDefault(s => s.City == invoice.shipment.Destination).ProvinceCode;
                long taxid = Convert.ToInt64(invoice.shipment.TaxAndDutyShippingAccountId);
                ShippingAccount tax_account = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxid);
                var taxCode = creditCard_request(tax_account.CreditCardNumber, tax_account.CreditCardSecurityNumber, invoice.shipment.Tax).Item2;
                invoice.shipment.TaxAuthorizationCode = taxCode.ToString();
                invoice.shipment.ShipmentAuthorizationCode = creditCard_request(shipment_fee_account.CreditCardNumber, shipment_fee_account.CreditCardSecurityNumber, invoice.TotalCost).Item2.ToString();
                SendInvoice(SetEmail("taxInvoice", invoice, shipment_fee_account, province, tax_account.EmailAddress));
                SendInvoice(SetEmail("shipmentInvoice", invoice, shipment_fee_account, province, shipment_fee_account.EmailAddress));
            }
            else
            {
                long shipment_fee_id = Convert.ToInt64(invoice.shipment.ShipmentShippingAccountId);
                ShippingAccount shipment_fee_account = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == shipment_fee_id);
                string province = db.Destinations.FirstOrDefault(s => s.City == invoice.shipment.Destination).ProvinceCode;

                /*Unecessary?
                long taxid = Convert.ToInt64(invoice.shipment.TaxAndDutyShippingAccountId);
                ShippingAccount tax_account = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxid);
                */

                var taxCode = creditCard_request(shipment_fee_account.CreditCardNumber, shipment_fee_account.CreditCardSecurityNumber, invoice.shipment.Tax).Item2;
                invoice.shipment.TaxAuthorizationCode = taxCode.ToString();
                invoice.shipment.ShipmentAuthorizationCode = creditCard_request(shipment_fee_account.CreditCardNumber, shipment_fee_account.CreditCardSecurityNumber, invoice.TotalCost).Item2.ToString();
                SendInvoice(SetEmail("CombinedInvoice", invoice, shipment_fee_account, province, shipment_fee_account.EmailAddress));
            }

            invoice.shipment.invoice = invoice;
            invoice.InvoiceId = invoice.shipment.WaybillId;
            db.Invoices.Add(invoice);
            db.SaveChanges();
             return RedirectToAction("SearchWayBill");
        }
        public MailMessage SetEmail(string invoiceType,Invoice invoice,ShippingAccount shippingaccount,string province,string emailAddress)
        {
            var shipmentShippingAccountId = shippingaccount.ShippingAccountId;
            var shippedDate = invoice.shipment.ShippedDate;
            var WaybillId = invoice.shipment.WaybillId;
            var serviceType = invoice.shipment.ServiceType;
            var referenceNumber = invoice.shipment.ReferenceNumber;
            string senderName ="" ;
            if (shippingaccount is PersonalShippingAccount)
            {
                PersonalShippingAccount Paccount = (PersonalShippingAccount)(shippingaccount);
                senderName = Paccount.FirstName + " " + Paccount.LastName;
            }
            else
            {
                BusinessShippingAccount Baccount = (BusinessShippingAccount)(shippingaccount);
                senderName = Baccount.CompanyName;
            }
            senderName = shippingaccount.UserName;
            
            var senderAddress = shippingaccount.MailingAddressBuilding + " , " + shippingaccount.MailingAddressStreet + shippingaccount.MailingAddressCity + " , " + shippingaccount.MailingAddressProvinceCode;
            var recipientName = invoice.shipment.RecipientName;
            var recipientAddress = invoice.shipment.RecipientBuildingAddress+" , "+invoice.shipment.RecipientStreetAddress + " , " + invoice.shipment.RecipientCityAddress+" , "+province;
            var creditCardType = shippingaccount.CreditCardType;
            var creditCardNumber = shippingaccount.CreditCardSecurityNumber;
            string packagesContent = "<table class=\"table\"><tr><th>Package Type</th><th>Customer Weight</th><th>Actual Weight</th><th>Cost</th></tr>";
            foreach(var package in invoice.shipment.Packages)
            {
                packagesContent = packagesContent + "<tr><td>"+ package.PackageType.Type+ "  </td><td> ";
                packagesContent = packagesContent + package.Weight + "  </td><td> ";
                packagesContent = packagesContent + package.ActualWeight;
                var cost = ConvertCurrency(invoice.TotalCostCurrency, package.Cost);
                packagesContent = packagesContent + "</td><td>" + Math.Round(cost,2) + invoice.TotalCostCurrency+"</td></tr>";
                      
             }
            packagesContent = packagesContent + " </table>";
            MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
            message.From = new MailAddress("comp3111_team105@cse.ust.hk");
            message.To.Add(emailAddress);
            message.Body = "<!doctype html><html><head><meta charset = 'UTF-8'></head><div>Shipping Account ID: " + shipmentShippingAccountId + " </div> &nbsp; &nbsp;<div> WayBill ID:  " + WaybillId+ "</div><br/><div>Ship Date: ";
            message.Body = message.Body + shippedDate + " </div> &nbsp; &nbsp; &nbsp; &nbsp;<div>Service Type: " + serviceType + "</div><br/>";
            if (invoice.shipment.ReferenceNumber != null) {
                message.Body = message.Body + "< div> Sender Reference Number:  " + referenceNumber + "</div>";

            }
               message.Body = message.Body + " <div> Sender Name: "+ senderName +"</div><br/><div> Sender Address: "+senderAddress+" </div><br/><div> Recipient Name: "+ recipientName +"</div><br/><div> Recipient Address: "+recipientAddress +"</div><br/><div> Credit Card Type: "+ creditCardType+"</div> &nbsp; &nbsp;<div> Credit Card Number: "+creditCardNumber+"</div><br/>";
            message.Body = message.Body + packagesContent;
            switch (invoiceType)
            {
                case "taxInvoice": {
                        message.Subject = "Tax and Duty Invoice";
                        message.Body = message.Body + "<div>Duties Amounts: " + Math.Round(ConvertCurrency(invoice.shipment.DutyCurrency, invoice.shipment.Duty),2).ToString() + invoice.shipment.DutyCurrency+"</div> &nbsp;<div>Tax Amounts" + Math.Round(ConvertCurrency(invoice.shipment.TaxCurreny, invoice.shipment.Tax),2).ToString() + invoice.shipment.TaxCurreny + "</div> &nbsp;<div> Authorization Code: " + invoice.shipment.TaxAuthorizationCode + "</div><br/>";


                        break; }
                case "shipmentInvoice": {
                        message.Subject = "Shipment Invoice";
                        message.Body = message.Body + "<div>Total Cost: " + Math.Round(ConvertCurrency(invoice.TotalCostCurrency, invoice.TotalCost),2).ToString() + invoice.TotalCostCurrency+"</div> &nbsp; <div> Authorization Code: " + invoice.shipment.ShipmentAuthorizationCode + "</div><br/>";
                        break;
                    }
                case "CombinedInvoice": {
                        message.Subject = "Tax, Duty and Shipment  Invoice";
                        message.Body = message.Body + "<div>Duties Amounts: " + Math.Round(ConvertCurrency(invoice.shipment.DutyCurrency, invoice.shipment.Duty), 2).ToString() + invoice.shipment.DutyCurrency + "</div> &nbsp;<div>Tax Amounts" + Math.Round(ConvertCurrency(invoice.shipment.TaxCurreny, invoice.shipment.Tax), 2).ToString() + invoice.shipment.TaxCurreny + "</div> &nbsp;<div> Authorization Code: " + invoice.shipment.TaxAuthorizationCode + "</div><br/>";
                        message.Body = message.Body + "<div>Total Cost: " + Math.Round(ConvertCurrency(invoice.TotalCostCurrency, invoice.TotalCost),2).ToString() + "</div><br/>";
                        break; }
            }
            message.Body = message.Body + "<body></body></html>";
            return message;
            
        }
        public bool SendInvoice(MailMessage message)
        {
            if (sendEmail(message))
            {
                return true;
            }
            return false;
        }
    }
}
