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
    }
}
