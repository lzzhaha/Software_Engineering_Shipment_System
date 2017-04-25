using System;
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
                ViewBag.NoPickup = "yes";
                return View(invoice);
            }
            ViewBag.NoPickup = "No";
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
            foreach(var package in shipment.Packages)
            {
                package.ActualWeight = invoice.shipment.Packages.ToList()[i].ActualWeight;
                i = i + 1;
            }
            shipment.Tax = invoice.shipment.Tax;
            shipment.Duty = invoice.shipment.Duty;
            long shipmentShippingAccountId= Convert.ToInt64(shipment.ShipmentShippingAccountId);
            var provinceCode = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == shipmentShippingAccountId).MailingAddressProvinceCode;
            invoice.TotalCostCurrency = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode).CurrencyCode;
            long taxShippingAccountID = Convert.ToInt64(shipment.TaxAndDutyShippingAccountId );
            var provinceCode2 = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxShippingAccountID).MailingAddressProvinceCode;
            shipment.TaxCurreny = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode2).CurrencyCode;
            shipment.DutyCurrency = shipment.TaxCurreny;
            //calculate the total fee
            if (shipment.Packages.Count()!=0)
            {
                var ServiceTypeID = db.ServiceTypes.SingleOrDefault(s => s.Type == shipment.ServiceType).ServiceTypeID;
                int PackageTypeID;
                ServicePackageFee servicePackageFee;
                string packageTypelimit;
                Decimal fee, TotalFee, decimalweight;
                TotalFee = 0;
                foreach (var package in shipment.Packages)
                {
                    if (package.PackageTypeID !=0 && package.ActualWeight != null)
                    {
                        PackageTypeID = package.PackageTypeID;
                        servicePackageFee = db.ServicePackageFees.SingleOrDefault(s => (s.PackageTypeID == PackageTypeID && s.ServiceTypeID == ServiceTypeID));
                        decimalweight = decimal.Round((decimal)package.Weight, 1);
                        fee = (decimalweight * servicePackageFee.Fee < servicePackageFee.MinimumFee ? servicePackageFee.MinimumFee : decimalweight * servicePackageFee.Fee);
                        package.Cost = fee;
                        Regex reg = new Regex(@"([0-9]*).*");

                        if (package.PackageTypeSizeID != null)
                        {
                            packageTypelimit = db.PackageTypeSizes.SingleOrDefault(s => s.PackageTypeSizeID == package.PackageTypeSizeID).limit;
                            var result = reg.Match(packageTypelimit).Groups;
                            if (result[1].Value == "")
                            {
                                fee = servicePackageFee.Fee;
                            }
                            else if (package.Weight > decimal.Parse(result[1].Value))
                            {
                                fee += 500;
                            }
                        }
                        TotalFee += fee;
                        package.PackageType = db.PackageTypes.SingleOrDefault(s => s.PackageTypeID == package.PackageTypeID);
                    }
                    invoice.TotalCost = TotalFee;
                }
               
            }

            invoice.shipment = shipment;
            if (invoice.shipment.TaxAndDutyShippingAccountId != invoice.shipment.ShipmentShippingAccountId)
            {
                ShippingAccount shippingaccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == invoice.shipment.ShippingAccountId);
                string province = db.Destinations.FirstOrDefault(s => s.City == invoice.shipment.Destination).ProvinceCode;
                long taxid = Convert.ToInt64(invoice.shipment.TaxAndDutyShippingAccountId);
                ShippingAccount Taxaccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxid);
                var taxCode = creditCard_request(Taxaccount.CreditCardNumber, Taxaccount.CreditCardSecurityNumber, invoice.shipment.Tax).Item2;
                invoice.shipment.TaxAuthorizationCode = taxCode.ToString();
                invoice.shipment.ShipmentAuthorizationCode= creditCard_request(shippingaccount.CreditCardNumber, shippingaccount.CreditCardSecurityNumber, invoice.TotalCost).Item2.ToString();
                SendVoice("taxInvoice", invoice, shippingaccount, province, "lchenbk@connect.ust.hk");
                SendVoice("shipmentInvoice", invoice, shippingaccount, province, "lchenbk@connect.ust.hk");
            }
            else
            {
                ShippingAccount shippingaccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == invoice.shipment.ShippingAccountId);
                string province = db.Destinations.FirstOrDefault(s => s.City == invoice.shipment.Destination).ProvinceCode;
                long taxid = Convert.ToInt64(invoice.shipment.TaxAndDutyShippingAccountId);
                ShippingAccount Taxaccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId ==taxid );
                var taxCode = creditCard_request(Taxaccount.CreditCardNumber, Taxaccount.CreditCardSecurityNumber, invoice.shipment.Tax).Item2;
                invoice.shipment.TaxAuthorizationCode = taxCode.ToString();
                invoice.shipment.ShipmentAuthorizationCode = creditCard_request(shippingaccount.CreditCardNumber, shippingaccount.CreditCardSecurityNumber, invoice.TotalCost).Item2.ToString();
                SendVoice("CombinedInvoice", invoice, shippingaccount, province, "lchenbk@connect.ust.hk");
            }
           
            invoice.shipment.invoice = invoice;
            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
            return View(invoice);
        }
        public bool SendVoice(string invoiceType,Invoice invoice,ShippingAccount shippingaccount,string province,string emailAddress)
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
            string packagesContent = "<table class=\"table\"><tr><th>Package Type</th><th>Customer Weight</th><th>Actual Weight</th><th></th></tr>";
            foreach(var package in invoice.shipment.Packages)
            {
                packagesContent = packagesContent + "<tr><td>"+ package.PackageType.Type+ "  </td><td> ";

                packagesContent = packagesContent + package.ActualWeight;
                var cost = ConvertCurrency(invoice.TotalCostCurrency, package.Cost);
                packagesContent = packagesContent + "</td><td>" + cost + "</td></tr>";
                      
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
                        message.Body = message.Body + "<div>Duties Amounts: " + Math.Round(ConvertCurrency(invoice.shipment.DutyCurrency, invoice.shipment.Duty),2).ToString() + "</div> &nbsp;<div>Tax Amounts" + Math.Round(ConvertCurrency(invoice.shipment.TaxCurreny, invoice.shipment.Tax),2).ToString() + "</div> &nbsp;<div> Authorization Code: " + invoice.shipment.TaxAuthorizationCode + "</div><br/>";


                        break; }
                case "shipmentInvoice": {
                        message.Subject = "Shipment Invoice";
                        message.Body = message.Body + "<div>Total Cost: " + Math.Round(ConvertCurrency(invoice.TotalCostCurrency, invoice.TotalCost),2).ToString() + "</div> &nbsp; <div> Authorization Code: " + invoice.shipment.ShipmentAuthorizationCode + "</div><br/>";
                        break;
                    }
                case "CombinedInvoice": {
                        message.Subject = "Tax, Duty and Shipment  Invoice";
                        message.Body = message.Body + "<div>Duties Amounts: " + Math.Round(ConvertCurrency(invoice.shipment.DutyCurrency, invoice.shipment.Duty),2).ToString() + "</div>&nbsp;<div> Tax Amount: " + Math.Round(ConvertCurrency(invoice.shipment.TaxCurreny, invoice.shipment.Tax), 2).ToString() + "</div> &nbsp; <div> Authorization Code: " + invoice.shipment.ShipmentAuthorizationCode + "</div><br/>";
                        message.Body = message.Body + "<div>Total Cost: " + Math.Round(ConvertCurrency(invoice.TotalCostCurrency, invoice.TotalCost),2).ToString() + "</div><br/>";
                        break; }
            }
            message.Body = message.Body + "<body></body></html>";
            if (sendEmail(message))
            {
                return true;
            }
            return false;
        }
    }
}
