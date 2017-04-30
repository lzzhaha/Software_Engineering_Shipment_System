using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.Controllers.Tests
{
    [TestFixture()]
    public class BaseControllerTests
    {
        Address tempAddress = new Address();
        Address inputAddress = new Address();
        BaseController bC = new BaseController();
        [SetUp]
        public void SetUp()
        {
            inputAddress.AddressId = 1;
            inputAddress.AddressType = "Pickup Location";
            inputAddress.Building = "academic building";
            inputAddress.City = "Hong Kong";
            inputAddress.NickName = "lalal";
            inputAddress.PostalCode = "999077";
            inputAddress.ServiceCity = "Hong Kong";
            inputAddress.ShippingAccountId = 1;
            inputAddress.Street = "HKUST";
            tempAddress.AddressId = 1;
            tempAddress.ShippingAccountId = 1;
        }
        [Test()]
        public void AddressObjectMapTest()
        {
            bC.AddressObjectMap(ref tempAddress, inputAddress);

            Assert.That(tempAddress.AddressId, Is.EqualTo(inputAddress.AddressId));
            Assert.That(tempAddress.AddressType, Is.EqualTo(inputAddress.AddressType));
            Assert.That(tempAddress.Building, Is.EqualTo(inputAddress.Building));
            Assert.That(tempAddress.City, Is.EqualTo(inputAddress.City));
            Assert.That(tempAddress.NickName, Is.EqualTo(inputAddress.NickName));
            Assert.That(tempAddress.PostalCode, Is.EqualTo(inputAddress.PostalCode));
            Assert.That(tempAddress.ServiceCity, Is.EqualTo(inputAddress.ServiceCity));
            Assert.That(tempAddress.ShippingAccountId, Is.EqualTo(inputAddress.ShippingAccountId));
            Assert.That(tempAddress.Street, Is.EqualTo(inputAddress.Street));
        }

        [Test()]
        public void joinAddressTest()
        {
            Assert.That(bC.joinAddress(inputAddress), Is.EqualTo("academic building,Hong Kong,Hong Kong"));
            Assert.That(bC.joinAddress(tempAddress), Is.EqualTo("academic building,Hong Kong,Hong Kong"));
        }
    }
}