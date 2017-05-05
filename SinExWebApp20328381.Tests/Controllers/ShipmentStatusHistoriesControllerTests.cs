using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
    }
}