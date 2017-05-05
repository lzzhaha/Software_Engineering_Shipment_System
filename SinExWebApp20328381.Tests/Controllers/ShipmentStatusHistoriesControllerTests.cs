using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Mail;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.Controllers.Tests
{
    [TestFixture()]
    public class ShipmentStatusHistoriesControllerTests
    {

        [Test()]
        public void PassIndexTest()
        {
            var controller = new ShipmentStatusHistoriesController();
            var result = controller.PassIndex() as ViewResult;

            Assert.AreEqual("000000000001", result.ViewData["WaybillNumber"]);
            Assert.AreEqual("HAHA", result.ViewData["DeliveredPerson"]);
            Assert.AreEqual("haha", result.ViewData["Company"]);
            Assert.AreEqual("HK", result.ViewData["DeliveredPlace"]);
            Assert.AreEqual("Delivered", result.ViewData["Status"]);
            Assert.AreEqual("Same Day", result.ViewData["ServiceType"]);
        }




        Shipment shipment = new Shipment();
        ShipmentStatusHistoriesController SC = new ShipmentStatusHistoriesController();
        [SetUp()]
        public void setup() {

            shipment.WaybillId = 1;

            shipment.ServiceType = "Same Day";

            shipment.PickupType = "Immediate";

        shipment.DeliveredPlace = "HK";
        shipment.DeliveredPerson = "HAHA" ;
        shipment.RecipientName = "HAHAHA";
        shipment.NumberOfPackages =1;
        shipment.Packages = new Package[] { new Package()};
        shipment.Origin = "BJ";
        shipment.Destination = "HK";
        shipment.Status = "PickedUp"; 
        shipment.ShippingAccountId =1;
        shipment.ShippedDate = new DateTime(2017,05,05);
        shipment.DeliveredDate = new DateTime(2017,06,06);
        shipment.TaxCurreny="CNY";
        
        shipment.DutyCurrency="";
        shipment.TaxAndDutyShippingAccountId = "111111111111";
        shipment.ShipmentShippingAccountId ="222222222222";
        shipment.EmailWhenPickup = true;
        shipment.EmailWhenDeliver = true;
        shipment.RecipientPhoneNumber ="111111111";
        shipment.RecipientCompanyName ="";
        shipment.RecipientDepartmentName="" ;
       
        shipment.RecipientEmailAddress= "lchenbk@connect.ust.hk";
        shipment.RecipientBuildingAddress="Academic" ;
        shipment.RecipientStreetAddress ="Clear Water Bay" ;
        shipment.RecipientCityAddress ="Hong Kong";
        shipment.RecipientPostalCode ="361000";
        shipment.PickupAddress ="AAA";
       
        shipment.TaxAuthorizationCode = "1000"; // payment information
        shipment.ShipmentAuthorizationCode="2000" ; // payment information
        Invoice invoice = new Invoice();
        shipment.ShipmentStatusHistory = new ShipmentStatusHistory[] { new ShipmentStatusHistory()};
        PersonalShippingAccount account = new PersonalShippingAccount();

            account.FirstName = "Tom";
            account.LastName = "Jerry";
            account.ShippingAccountId = 2;
            account.MailingAddressBuilding = "LSK";
            account.MailingAddressStreet = "Tian";
            account.MailingAddressCity = "Beijing";
            account.MailingAddressProvinceCode = "BJ";
            account.MailingAddressPostalCode = "11111";        
            account.PhoneNumber = "111111111";
            account.CreditCardType = "Visa";
            account.CreditCardSecurityNumber = "1111111111111111";
            account.CreditCardHolderName = "Tom Jerry";
            account.CreditCardExpiryMonth = 10;
            account.CreditCardExpiryYear = "2016";
            account.UserName = "zlinai";
            account.Shipments = new Shipment[] { new Shipment() };
            account.Addresses = new Address[] { new Address() };
            shipment.ShippingAccount = account;
    }


    [Test()]
        public void Send_EmailTest()
        {
            setup();
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress("comp3111_team105@cse.ust.hk");
            message.To.Add("lchenbk@connect.ust.hk");
            message.Subject = "Delivered Notification";
            message.Body = "<!doctype html><html><head><meta charset = 'UTF-8'></head>";

            message.Body +=
                    "<div>A shipment (WaybillId:&nbsp0000000000000001) for you has been successfully picked up.</div><br />" +
                    "<div>Sender:&nbspTom Jerry</div><br />" +
                   "<div>City:&nbspBeijing,</div> <br />" +
                   "<div>Street:&nbspTian</div><br />" +
                   " <div>Building:&nbspLSK</div><br />" +
                   "<div>Delivery Date:&nbsp05-05-2017</div>";
            
            message.Body += "< body ></ body ></ html >";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            MailMessage messageForTest = SC.Send_Email(shipment);
            Assert.That(messageForTest.Body, Is.EqualTo(message.Body));
            Assert.That(messageForTest.From, Is.EqualTo(message.From));
            Assert.That(messageForTest.BodyEncoding, Is.EqualTo(message.BodyEncoding));
        }
    }
}