using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.Controllers
{
    public class PersonalShippingAccountsController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: PersonalShippingAccounts
  
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonalShippingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingAccountId,MailingAddressBuilding,MailingAddressStreet,MailingAddressCity,MailingAddressProvinceCode,MailingAddressPostalCode,PhoneNumber,EmailAddress,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardHolderName,CreditCardExpiryMonth,CreditCardExpiryYear,FirstName,LastName")] PersonalShippingAccount personalShippingAccount)
        {
            if (ModelState.IsValid)
            {
                //db.ShippingAccounts.Add(personalShippingAccount);
                //db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(personalShippingAccount);
        }

        // GET: PersonalShippingAccounts/Edit/5
        public ActionResult Edit()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is BusinessShippingAccount)
            {
                return RedirectToAction("Edit", "BusinessShippingAccounts");
            }
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            */
            PersonalShippingAccount personalShippingAccount = (PersonalShippingAccount)shippingAccount;
            if (personalShippingAccount == null)
            {
                return HttpNotFound();
            }
            return View(personalShippingAccount);
        }

        // POST: PersonalShippingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShippingAccountId,MailingAddressBuilding,MailingAddressStreet,MailingAddressCity,MailingAddressProvinceCode,MailingAddressPostalCode,PhoneNumber,EmailAddress,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardHolderName,CreditCardExpiryMonth,CreditCardExpiryYear,FirstName,LastName")] PersonalShippingAccount personalShippingAccount)
        {
            personalShippingAccount.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.Entry(personalShippingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(personalShippingAccount);
        }

        // POST: PersonalShippingAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalShippingAccount personalShippingAccount = (PersonalShippingAccount)db.ShippingAccounts.Find(id);
            db.ShippingAccounts.Remove(personalShippingAccount);
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
        [HttpGet]
        public ActionResult AddRecipientAddress()
        {
            ViewBag.unique = "Yes";
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is BusinessShippingAccount)
            {
                return RedirectToAction("Edit", "BusinessShippingAccounts");
            }
            else
            {
                
                ViewBag.shipmentId = shippingAccount.ShippingAccountId;
                return View();
            }

            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecipientAddress([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (ModelState.IsValid)
            {
                address.AddressType = "Recipient Address";
                var temp=db.Addresses.Where(s => (s.NickName == address.NickName) && ( s.AddressType == address.AddressType));
                if ( temp.Count()==0)
                {
                    db.Addresses.Add(address);
                    db.SaveChanges();
                    
                    return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
                }
            }
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            ViewBag.unique = "No";
            return View(address);
        }

        public ActionResult ManageRecipientAddress()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount.Addresses.Count == 0)
            {
                ViewBag.addressCount = "Empty";
            }
            else
            {
                ViewBag.addressCount = "NotEmpty";
            }
            return View(shippingAccount.Addresses.ToList());
        }
        [HttpGet]
        public ActionResult EditRecipientAddress(int id)
        {
            ViewBag.unique = "Yes";
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            var address = db.Addresses.SingleOrDefault(s => s.AddressId == id);
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            TempData["nickname"] = address.NickName;
            TempData["addressId"] = address.AddressId;
            return View(address);
        }
        [HttpPost]
        public ActionResult EditRecipientAddress([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (ModelState.IsValid)
            {
                address.AddressType = "Recipient Address";
                var temp = db.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                if (temp.Count() == 0 || address.NickName == TempData["nickname"].ToString())
                {
                    address.AddressId = (int)TempData["addressId"];
                    db.Entry(address).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
                }
            }
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            ViewBag.unique = "No";
            return View(address);
        }
        [HttpGet]
        public ActionResult DeleteRecipientAddress(int id)
        {
            Address address = (Address)db.Addresses.Find(id);
            return View(address);
        }
        [HttpPost, ActionName("DeleteRecipientAddress")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedDeleteRecipientAddress(int id)
        {
            Address address = (Address)db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
        }
        [HttpGet]
        public ActionResult AddPickupLocation()
        {
            ViewBag.unique = "Yes";
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is BusinessShippingAccount)
            {
                return RedirectToAction("Edit", "BusinessShippingAccounts");
            }
            else
            {
                ViewBag.shipmentId = shippingAccount.ShippingAccountId;
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPickupLocation([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (ModelState.IsValid)
            {
                address.AddressType = "Pickup Location";
                var temp = db.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                if (temp.Count() == 0)
                {
                    db.Addresses.Add(address);
                    db.SaveChanges();

                    return RedirectToAction("ManagePickupLocation", "PersonalShippingAccounts");
                }
            }
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            ViewBag.unique = "No";
            return View(address);
        }

        public ActionResult ManagePickupLocation()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount.Addresses.Count == 0)
            {
                ViewBag.addressCount = "Empty";
            }
            else
            {
                ViewBag.addressCount = "NotEmpty";
            }
            return View(shippingAccount.Addresses.ToList());
        }
        [HttpGet]
        public ActionResult EditPickupLocation(int id)
        {
            ViewBag.unique = "Yes";
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            var address =db.Addresses.SingleOrDefault(s =>s.AddressId==id);
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            TempData["nickname"] = address.NickName;
            TempData["addressId"] = address.AddressId;
            return View(address);
        }
        [HttpPost]
        public ActionResult EditPickupLocation([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (ModelState.IsValid)
            {
                address.AddressType = "Pickup Location";
                var temp = db.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                if (temp.Count() == 0 || address.NickName== TempData["nickname"].ToString())
                {
                    address.AddressId = (int)TempData["addressId"];
                    db.Entry(address).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ManagePickupLocation", "PersonalShippingAccounts");
                }
            }
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            ViewBag.unique = "No";
            return View(address);
        }
        [HttpGet]
        public ActionResult DeletePickupLocation(int id)
        {
            Address address = (Address)db.Addresses.Find(id);
            return View(address);
        }
        [HttpPost, ActionName("DeletePickupLocation")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedDeletePickupLocation(int id)
        {
            Address address = (Address)db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
        }
        /*public ActionResult AddAddress()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is BusinessShippingAccount)
            {
                return RedirectToAction("Edit", "BusinessShippingAccounts");
            }
            else
            {
                shippingAccount.RecipientAddresses
            }
        }*/
    }
}
