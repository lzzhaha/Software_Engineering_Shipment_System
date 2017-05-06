using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinExWebApp20328381.Models;
using System.Net.Mail;
using System.Web.Mvc;

namespace SinExWebApp20328381.Tests.Controllers
{
    class ShipmentsControllerTests
    {
        [Test()]
        public void GenerateShipmentReportInvalidDateCheckTest()
        {
            var controller = new ShipmentsController();
            DateTime StartDate = new DateTime(2018, 7, 10, 23, 49, 0);
            DateTime EndDate = new DateTime(2016, 7, 10, 23, 49, 0);

            var resultInvalid = controller.GenerateShipmentReportInvalidDateCheck(null, null, StartDate, EndDate) as ViewResult;

            Assert.AreEqual(new DateTime(2018, 7, 10, 23, 49, 0), resultInvalid.ViewBag.CurrentShippedStartDate);
            Assert.AreEqual(new DateTime(2016, 7, 10, 23, 49, 0), resultInvalid.ViewBag.CurrentShippedEndDate);
            Assert.AreEqual("Date range is invalid.", resultInvalid.ViewBag.ErrorMessage);

        }
        [Test()]
        public void SwitchSortOrderTest()
        {
            var controller = new ShipmentsController();
            string sortOrder;
            sortOrder = "ServiceType";
            var result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType_dest", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination", result.ViewBag.DestinationParm);

            sortOrder = "ShippedDate";
            result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate_dest", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination", result.ViewBag.DestinationParm);

            sortOrder = "DeliveredDate";
            result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate_dest", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination", result.ViewBag.DestinationParm);
            

            sortOrder = "RecipentName";
            result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName_dest", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination", result.ViewBag.DestinationParm);
            

            sortOrder = "Origin";
            result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin_dest", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination", result.ViewBag.DestinationParm);
            

            sortOrder = "Destination";
            result = controller.SwitchSortOrder(sortOrder) as ViewResult;
            Assert.AreEqual("ServiceType", result.ViewBag.ServiceTypeParm);
            Assert.AreEqual("ShippedDate", result.ViewBag.ShippedDateParm);
            Assert.AreEqual("DeliveredDate", result.ViewBag.DeliveredDateParm);
            Assert.AreEqual("RecipentName", result.ViewBag.RecipentNameParm);
            Assert.AreEqual("Origin", result.ViewBag.OriginParm);
            Assert.AreEqual("Destination_dest", result.ViewBag.DestinationParm);
            

            

        }
    }
}
