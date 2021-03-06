﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;
using System.Net.Mail;
using SinExWebApp20328381.ViewModels;
namespace SinExWebApp20328381.Controllers
{
    public class BusinessShippingAccountsController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();
        
        // GET: BusinessShippingAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessShippingAccounts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingAccountId,MailingAddressBuilding,MailingAddressStreet,MailingAddressCity,MailingAddressProvinceCode,MailingAddressPostalCode,PhoneNumber,EmailAddress,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardHolderName,CreditCardExpiryMonth,CreditCardExpiryYear,UserName,ContactPersonName,CompanyName,DepartmentName")] BusinessShippingAccount businessShippingAccount)
        {
            if (ModelState.IsValid)
            {
                //db.ShippingAccounts.Add(businessShippingAccount);
                //db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(businessShippingAccount);
        }

        // GET: BusinessShippingAccounts/Edit/5
        public ActionResult Edit()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("Edit", "PersonalShippingAccounts");
            }
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            */
            BusinessShippingAccount businessShippingAccount = (BusinessShippingAccount)shippingAccount;
            if (businessShippingAccount == null)
            {
                return HttpNotFound();
            }
            RegisterCustomerViewModel rem = new RegisterCustomerViewModel();
            rem.ProvinceList = PopulateProvinceDropdownList().ToList();
            rem.BusinessInformation = businessShippingAccount;
            return View(rem);
        }

        private SelectList PopulateProvinceDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var provinceQuery = db.Destinations.Select(s => s.ProvinceCode).Distinct().OrderBy(c => c);
            return new SelectList(provinceQuery);
        }
        // POST: BusinessShippingAccounts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterCustomerViewModel regist)
        {
            BusinessShippingAccount businessShippingAccount = regist.BusinessInformation;
            businessShippingAccount.UserName = System.Web.HttpContext.Current.User.Identity.Name;

            int y = int.Parse(businessShippingAccount.CreditCardExpiryYear);
            int m = businessShippingAccount.CreditCardExpiryMonth;
            if (y < DateTime.Now.Year)
            {
                ModelState.AddModelError("", "The card is expired.");

            }

            if (y == DateTime.Now.Year && m < DateTime.Now.Month)
                ModelState.AddModelError("", "The card is expired.");

            if (ModelState.IsValid)
            {
                db.Entry(businessShippingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            regist.ProvinceList = PopulateCurrenciesDropdownList().ToList();
            return View(regist);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AddRecipientAddress()
        {
            ViewBag.unique = "Yes";
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("AddRecipientAddress", "PersonalShippingAccounts");
            }
            else
            {

                var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
                ViewBag.ServiceCity = selectIetm;
                ViewBag.shipmentId = shippingAccount.ShippingAccountId;
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecipientAddress([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ServiceCity,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("AddRecipientAddress", "PersonalShippingAccounts");
            }
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            ViewBag.ServiceCity = selectIetm;
            if (ModelState.IsValid)
            {
                address.AddressType = "Recipient Address";

                var temp = shippingAccount.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                if (temp.Count() == 0)
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
            ShippingAccount shippingAccount = db.ShippingAccounts.Include("Addresses").SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
            }
            if (shippingAccount.Addresses.Where(s => s.AddressType == "Recipient Address").Count() == 0)
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
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("EditRecipientAddress", "PersonalShippingAccounts");
            }
            var address = db.Addresses.SingleOrDefault(s => s.AddressId == id);
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            selectIetm.Where(x => x.Value == address.ServiceCity).First().Selected = true;
            ViewBag.ServiceCity = selectIetm;
            TempData["nickname"] = address.NickName;
            TempData["addressId"] = address.AddressId;
            return View(address);
        }
        [HttpPost]
        public ActionResult EditRecipientAddress([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ServiceCity,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.Include("Addresses").SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("EditRecipientAddress", "PersonalShippingAccounts");
            }
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            selectIetm.Where(x => x.Value == address.ServiceCity).First().Selected = true;
            ViewBag.ServiceCity = selectIetm;
            if (ModelState.IsValid)
            {
                address.AddressType = "Recipient Address";
                long tempId = 0;
                if (TempData["addressId"] != null)
                {
                    tempId = (int)(TempData["addressId"]);
                }
                else
                {
                    tempId = address.AddressId;
                }
                var temp = shippingAccount.Addresses.SingleOrDefault(s => s.AddressId == tempId);
                var nickNameQuery = shippingAccount.Addresses.Where(s => (s.AddressType == address.AddressType && s.NickName == address.NickName)).ToList();
                if (temp != null && (((address.NickName == TempData["nickname"].ToString()) && (nickNameQuery.Count() > 0)) || (nickNameQuery.Count() == 0)))
                {

                    address.AddressId = temp.AddressId;
                    temp.ShippingAccountId = shippingAccount.ShippingAccountId;
                    AddressObjectMap(ref temp, address);
                    /*temp.NickName = address.NickName;
                    temp.PostalCode = address.PostalCode;
                    temp.ServiceCity = address.ServiceCity;
                  
                    temp.Street = address.Street;
                    temp.City = address.City;
                    temp.Building = address.Building;
                    temp.AddressType = address.AddressType;*/
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ManageRecipientAddress", "BusinessShippingAccounts");
                }
                TempData["nickname"] = TempData["nickname"].ToString();

                //var temp = db.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                /*if (temp.Count() == 0 || address.NickName == TempData["nickname"].ToString())
                {
                    address.AddressId = (int)TempData["addressId"];
                    db.Entry(address).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ManageRecipientAddress", "PersonalShippingAccounts");
                }*/
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
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("AddPickupLocation", "PersonalShippingAccounts");
            }
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            ViewBag.ServiceCity = selectIetm;
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("AddPickupLocation", "BusinessShippingAccounts");
            }
            else
            {

                ViewBag.shipmentId = shippingAccount.ShippingAccountId;
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPickupLocation([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ServiceCity,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("AddPickupLocation", "PersonalShippingAccounts");
            }
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            ViewBag.ServiceCity = selectIetm;
            if (ModelState.IsValid)
            {
                address.AddressType = "Pickup Location";
                var temp = db.Addresses.Where(s => (s.NickName == address.NickName) && (s.AddressType == address.AddressType));
                if (temp.Count() == 0)
                {
                    db.Addresses.Add(address);
                    db.SaveChanges();

                    return RedirectToAction("ManagePickupLocation", "BusinessShippingAccounts");
                }
            }
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            ViewBag.unique = "No";
            return View(address);
        }

        public ActionResult ManagePickupLocation()
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.Include("Addresses").SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("ManagePickupLocation", "PersonalShippingAccounts");
            }
            if (shippingAccount.Addresses.Where(s => s.AddressType == "Pickup Location").Count() == 0)
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
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("EditPickupLocation", "PersonalShippingAccounts");
            }
            var address = db.Addresses.SingleOrDefault(s => s.AddressId == id);
            ViewBag.shipmentId = shippingAccount.ShippingAccountId;
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            selectIetm.Where(x => x.Value == address.ServiceCity).First().Selected = true;
            ViewBag.ServiceCity = selectIetm;
            TempData["nickname"] = address.NickName;
            TempData["addressId"] = address.AddressId;
            return View(address);
        }
        [HttpPost]
        public ActionResult EditPickupLocation([Bind(Include = "NickName,AddressId,ShippingAccountId,Building,Street,City,ServiceCity,PostalCode")] Address address)
        {
            ShippingAccount shippingAccount = db.ShippingAccounts.Include("Addresses").SingleOrDefault(s => s.UserName == System.Web.HttpContext.Current.User.Identity.Name);
            if (shippingAccount is PersonalShippingAccount)
            {
                return RedirectToAction("EditPickupLocation", "PersonalShippingAccounts");
            }
            var selectIetm = db.Destinations.Select(x => new SelectListItem() { Value = x.City, Text = x.City }).ToList();
            selectIetm.Where(x => x.Value == address.ServiceCity).First().Selected = true;
            ViewBag.ServiceCity = selectIetm;
            if (ModelState.IsValid)
            {
                address.AddressType = "Pickup Location";
                long tempId = 0;
                if (TempData["addressId"] != null)
                {
                    tempId = (int)(TempData["addressId"]);
                }
                else
                {
                    tempId = address.AddressId;
                }
                var temp = shippingAccount.Addresses.SingleOrDefault(s => s.AddressId == tempId);
                var nickNameQuery = shippingAccount.Addresses.Where(s => (s.AddressType == address.AddressType && s.NickName == address.NickName)).ToList();
                if (temp != null && (((address.NickName == TempData["nickname"].ToString()) && (nickNameQuery.Count() > 0)) || (nickNameQuery.Count() == 0)))
                {

                    address.AddressId = temp.AddressId;
                    temp.ShippingAccountId = shippingAccount.ShippingAccountId;
                    /*temp.NickName = address.NickName;
                    temp.PostalCode = address.PostalCode;
                    temp.ServiceCity = address.ServiceCity;
                    temp.Street = address.Street;
                    temp.City = address.City;
                    temp.Building = address.Building;
                    temp.AddressType = address.AddressType;*/
                    AddressObjectMap(ref temp, address);
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ManagePickupLocation", "BusinessShippingAccounts");
                }
                TempData["nickname"] = TempData["nickname"].ToString();
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
            return RedirectToAction("ManagePickupLocation", "BusinessShippingAccounts");
        }
        
       
    }
}
