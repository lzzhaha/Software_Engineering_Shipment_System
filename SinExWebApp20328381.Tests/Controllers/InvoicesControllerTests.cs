using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinExWebApp20328381.Models;
using System.Net.Mail;

namespace SinExWebApp20328381.Controllers.Tests
{
    [TestFixture()]
    public class InvoicesControllerTests
    {/*
        Invoice invoice = new Invoice();
        Package package1 = new Package();
        Package package2 = new Package();
        Package package3 = new Package();
        Shipment shipment = new Shipment();
        PackageType pType = new PackageType();

        PersonalShippingAccount account = new PersonalShippingAccount();
        InvoicesController IC = new InvoicesController();
        [SetUp]
        public void setUp()
        {
           
            package1.ActualWeight = 20;
            package1.Cost = 160;
            package1.Description = "1";
            package1.PackageId = 1;
            package1.PackageTypeID = 1;
            package1.PackageTypeSizeID = 1;
            package1.Value = 20;
            package1.ValueCurrency = "HKD";
            package1.WaybillId = 1;
            package1.Weight = 19;
            package2.ActualWeight = 14;
            package2.Cost = 180;
            package2.Description = "2";
            package2.PackageId = 2;
            package2.PackageTypeID = 1;
            package2.PackageTypeSizeID = 2;
            package2.Value = 30;
            package2.ValueCurrency = "HKD";
            package2.WaybillId = 1;
            package2.Weight = 15;
            package3.ActualWeight = 300;
            package3.Cost = 4000;
            package3.Description = "3";
            package3.PackageId = 3;
            package3.PackageTypeID = 2;
            package3.PackageTypeSizeID = 2;
            package3.Value = 12320;
            package3.ValueCurrency = "RMB";
            package3.WaybillId = 1;
            package3.Weight = 310;
            pType.Description = "sdaasd";
            pType.PackageTypeID = 1;
            pType.Type = "Box";
            package1.PackageType = pType;
            package2.PackageType = pType;
            package3.PackageType = pType;
            account.UserName = "lchenbk";
            account.ShippingAccountId = 1;
            account.PhoneNumber = "1234567";
            account.MailingAddressProvinceCode = "HK";
            account.LastName = "CHEN";
            account.FirstName = "Lian";
            account.EmailAddress = "lchenbk@connect.ust.hk";
            account.CreditCardType = "Visa";
            account.CreditCardNumber = "1234567890";
            account.CreditCardSecurityNumber = "189";
            account.CreditCardHolderName = "CHEN,Lian";
            account.CreditCardExpiryYear = "2020";
            account.CreditCardExpiryMonth = 10;
            account.MailingAddressBuilding = "AB";
            account.MailingAddressCity = "Hong Kong";
            account.MailingAddressPostalCode = "99077";
            account.MailingAddressProvinceCode = "HK";
            account.MailingAddressStreet = "HKUST";
            shipment.Destination = "Hong Kong";
            shipment.Duty = 300;
            shipment.DutyCurrency = "HKD";
            shipment.EmailWhenDeliver = true;
            shipment.EmailWhenPickup = true;
            shipment.NumberOfPackages = 3;
            shipment.Origin = "Beijing";
            shipment.RecipientBuildingAddress = "academic building"; 
            shipment.RecipientCityAddress = "Hong Kong";
            shipment.RecipientCompanyName = "HKUST";
            shipment.RecipientEmailAddress = "lchenbk@connect.ust.hk";
            shipment.RecipientName = "CHEN,Lian";
            shipment.RecipientPhoneNumber="12345678";
            shipment.RecipientPostalCode = "999077";
            shipment.RecipientStreetAddress = "HKUST";
            shipment.ServiceType = "Ground";
            shipment.ShipmentAuthorizationCode = "0079";
            shipment.ShipmentShippingAccountId = "1";
            shipment.ShippingAccount = account;
            shipment.ShippingAccountId = 1;
            shipment.Status = "Confirmed";
            shipment.Tax = 30;
            shipment.TaxAndDutyShippingAccountId = "1";
            shipment.TaxAuthorizationCode = "0083";
            shipment.TaxCurreny = "HKD";
            shipment.WaybillId = 1;
            List<Package> pList = new List<Package>();
            pList.Add(package1);
            pList.Add(package2);
            pList.Add(package3);
            shipment.Packages = pList;
            shipment.Packages.Add(package2);
            shipment.Packages.Add(package3);
            List<Shipment> sList = new List<Shipment>();
            sList.Add(shipment);
            account.Shipments = sList;
            invoice.InvoiceId = 1;
            invoice.shipment = shipment;
            invoice.ShippingAccountId = 1;
            invoice.TotalCost = 8900;
            invoice.TotalCostCurrency = "HKD";
            invoice.WaybillId = 1;
            shipment.invoice = invoice;

        }*/
        /*[Test()]
        public void IndexTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GenerateInvoiceReportTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DetailsTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SearchWayBillTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FillActualWeightTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FillActualWeightTest1()
        {
            Assert.Fail();
        }
        */
       /* [Test()]
        public void SetEmailTest()
        {
            setUp();
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress("comp3111_team105@cse.ust.hk");
            message.To.Add("lchenbk@connect.ust.hk");
            message.Body = "<!doctype html><html><head><meta charset = 'UTF-8'></head><div>Shipping Account ID: " + 1 + " </div> &nbsp; &nbsp;<div> WayBill ID:  " + 1 + "</div><br/><div>Ship Date: ";
            message.Body = message.Body   + " </div> &nbsp; &nbsp; &nbsp; &nbsp;<div>Service Type: " + "Ground" + "</div><br/>";
            message.Body = message.Body + " <div> Sender Name: " + "lchenbk" + "</div><br/><div> Sender Address: " +  "AB,HKUST,Hong Kong,HK"+ " </div><br/><div> Recipient Name: " + "CHEN,Lian" + "</div><br/><div> Recipient Address: " + "academic building,HKUST,Hong Kong,HK" + "</div><br/><div> Credit Card Type: " + "Visa" + "</div> &nbsp; &nbsp;<div> Credit Card Number: " + "1234567890" + "</div><br/>";
            message.Body = message.Body + " < table class=\"table\"><tr><th>Package Type</th><th>Customer Weight</th><th>Actual Weight</th><th></th></tr>";
            message.Body = message.Body + "<tr><td>Box</td><td> "+20+"</td><td>" + 160 + "HKD</td></tr>";
            message.Body = message.Body + "<tr><td>Box</td><td> " + 14 + "</td><td>" + 180 + "HKD</td></tr>";
            message.Body = message.Body + "<tr><td>Box</td><td> " + 300 + "</td><td>" + 4000 + "HKD</td></tr></table>";
            message.Subject = "Tax and Duty Invoice";
            message.Body = message.Body + "<div>Duties Amounts: " + 300 + "HKD</div> &nbsp;<div>Tax Amounts" +30 + "HKD</div> &nbsp;<div> Authorization Code: " + 0083 + "</div><br/>";

            Assert.That(IC.SetEmail("taxInvoice", invoice, account, "HK", "lchenbk"), Is.EqualTo(message));
        }*/

        /*[Test()]
        public void SendInvoiceTest()
        {
            Assert.Fail();
        }*/
    }
}