using NUnit.Framework;
using SinExWebApp20328381.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinExWebApp20328381.Models;
using SinExWebApp20328381.ViewModels;

namespace SinExWebApp20328381.Controllers.Tests
{
    [TestFixture()]
    public class BaseControllerTests
    {
        Address tempAddress = new Address();
        Address inputAddress = new Address();
        Package tpkg = new Package();
        PackageInputViewModel tpkgVM= new PackageInputViewModel();
        Shipment ts = new Shipment();
        ShipmentInputViewModel tsVM = new ShipmentInputViewModel();
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
            //package part
            tpkg.PackageId = 1;
            tpkg.WaybillId = 5;
            tpkg.Weight = (decimal)3.0;
            tpkg.ActualWeight = (decimal)5.6;
            tpkg.Value = 135;
            tpkg.ValueCurrency = "TWD";
            tpkg.Description = "Spicy Strips.";
            tpkg.Cost = 567;
            tpkgVM.PackageId = 1;
            tpkgVM.Weight = (decimal)3.0;
            tpkgVM.ActualWeight = (decimal)5.6;
            tpkgVM.Value = 135;
            tpkgVM.ValueCurrency = "TWD";
            tpkgVM.Description = "Spicy Strips.";
            //shipment part
            ts.WaybillId = 0000000000000001;
            ts.ReferenceNumber = "hw";
            ts.ServiceType = "Same Day";
            ts.PickupType = "immediate";
            ts.ShippedDate = new DateTime(2011, 1, 5, 7, 12, 01);
            ts.DeliveredDate = new DateTime(2013, 2, 8, 11, 13, 06);
            ts.DeliveredPlace = "Hang Hau";
            ts.DeliveredPerson = "SB";
            ts.RecipientName = "GG";
            ts.NumberOfPackages = 5;
            ts.Origin = "Guangzhou";
            ts.Destination = "Beijing";
            ts.Status = "Delivered";
            ts.ShippingAccountId = 0000000000000002;
            ts.Tax = 50;
            ts.TaxCurrency = "CNY";
            ts.Duty = 100;
            ts.DutyCurrency = "TWD";
            ts.TaxAndDutyShippingAccountId = "0000000000000003";
            ts.ShipmentShippingAccountId = "0000000000000002";
            ts.EmailWhenPickup = true;
            ts.EmailWhenDeliver = false;
            ts.RecipientPhoneNumber = "66460123";
            ts.RecipientCompanyName = "HKUST";
            ts.RecipientDepartmentName = "CSE";
            ts.RecipientEmailAddress = "HIHIHI@adfs.com";
            ts.RecipientBuildingAddress = "hall 1";
            ts.RecipientStreetAddress = "HiHello Street";
            ts.RecipientPostalCode = "522446";
            ts.PickupAddress = "01, GG, Beijing, China";
            ts.TaxAuthorizationCode = "1256";
            ts.ShipmentAuthorizationCode = "1686";
            tsVM.ReferenceNumber = "hw";
            tsVM.RecipientName = "GG";
            tsVM.RecipientPhoneNumber = "66460123";
            tsVM.RecipientCompanyName = "HKUST";
            tsVM.RecipientDepartmentName = "CSE";
            tsVM.RecipientBuildingAddress = "hall 1";
            tsVM.RecipientStreetAddress = "HiHello Street";
            tsVM.RecipientCityAddress = "Beijing";
            tsVM.RecipientPostalCode = "522446";
            tsVM.PickupAddress = "01, GG, Beijing, China";
            tsVM.RecipientEmailAddress = "HIHIHI@adfs.com";
            tsVM.RecipientAccountId = "0000000000000003";
            tsVM.ServiceType = "Same Day";
            tsVM.NumberOfPackages = 5;
            tsVM.ShipmentPayer = "sender";
            tsVM.DaTPayer = "recipient";
            tsVM.PickupEmail = "1";
            tsVM.DeliverEmail = "0";
            tsVM.Origin = "Guangzhou";
            tsVM.Destination = "Beijing";
            tsVM.PickupType = "immediate";
            tsVM.ShippedDate = new DateTime(2011, 1, 5, 7, 12, 01);
            tsVM.DeliveredDate = new DateTime(2013, 2, 8, 11, 13, 06);
            tsVM.WaybillId = 0000000000000001;
            tsVM.Tax = 50;
            tsVM.TaxCurrency = "CNY";
            tsVM.Duty = 100;
            tsVM.DutyCurrency = "TWD";
            tsVM.TaxAuthorizationCode = "1256";
            tsVM.ShipmentAuthorizationCode = "1686";
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
        
        [Test()]
        public void PackageToPackageViewModelWithoutDBTest ()
        {
            PackageInputViewModel pkgVM = new PackageInputViewModel();
            bC.PackageToPackageViewModelWithoutDB(ref tpkg, ref pkgVM);
            Assert.AreEqual(tpkgVM.Weight, pkgVM.Weight);
            Assert.AreEqual(tpkgVM.ActualWeight, pkgVM.ActualWeight);
            Assert.AreEqual(tpkgVM.Value, pkgVM.Value);
            Assert.AreEqual(tpkgVM.ValueCurrency, pkgVM.ValueCurrency);
            Assert.AreEqual(tpkgVM.Description, pkgVM.Description);
            Assert.AreEqual(tpkgVM.PackageId, pkgVM.PackageId);
        }

        [Test()]
        public void PackageViewModelToPackageWithoutDBTest()
        {
            Package pkg = new Package();
            bC.PackageViewModelToPackageWithoutDB(ref tpkgVM, ref pkg);
            Assert.AreEqual(tpkg.Weight, pkg.Weight);
            Assert.AreEqual(tpkg.ActualWeight, pkg.ActualWeight);
            Assert.AreEqual(tpkg.Value, pkg.Value);
            Assert.AreEqual(tpkg.ValueCurrency, pkg.ValueCurrency);
            Assert.AreEqual(tpkg.Description, pkg.Description);
            Assert.AreEqual(tpkg.PackageId, pkg.PackageId);
        }

        [Test()]
        public void ShipmentToShipmentViewModelWithoutDBTest()
        {
            ShipmentInputViewModel sVM = new ShipmentInputViewModel();
            bC.ShipmentToShipmentViewModelWithoutDB(ref ts, ref sVM);
            Assert.AreEqual(tsVM.WaybillId, sVM.WaybillId);
            Assert.AreEqual(tsVM.ReferenceNumber, sVM.ReferenceNumber);
            Assert.AreEqual(tsVM.PickupType, sVM.PickupType);
            Assert.AreEqual(tsVM.ShippedDate, sVM.ShippedDate);
            Assert.AreEqual(tsVM.DeliveredDate, sVM.DeliveredDate);
            Assert.AreEqual(tsVM.RecipientName, sVM.RecipientName);
            Assert.AreEqual(tsVM.NumberOfPackages, sVM.NumberOfPackages);
            Assert.AreEqual(tsVM.Origin, sVM.Origin);
            Assert.AreEqual(tsVM.Destination, sVM.Destination);
            Assert.AreEqual(tsVM.Tax, sVM.Tax);
            Assert.AreEqual(tsVM.TaxCurrency, sVM.TaxCurrency);
            Assert.AreEqual(tsVM.Duty, sVM.Duty);
            Assert.AreEqual(tsVM.DutyCurrency, sVM.DutyCurrency);
            Assert.AreEqual(tsVM.PickupEmail, sVM.PickupEmail);
            Assert.AreEqual(tsVM.DeliverEmail, sVM.DeliverEmail);
            Assert.AreEqual(tsVM.RecipientPhoneNumber, sVM.RecipientPhoneNumber);
            Assert.AreEqual(tsVM.RecipientCompanyName, sVM.RecipientCompanyName);
            Assert.AreEqual(tsVM.RecipientDepartmentName, sVM.RecipientDepartmentName);
            Assert.AreEqual(tsVM.RecipientEmailAddress, sVM.RecipientEmailAddress);
            Assert.AreEqual(tsVM.RecipientBuildingAddress, sVM.RecipientBuildingAddress);
            Assert.AreEqual(tsVM.RecipientStreetAddress, sVM.RecipientStreetAddress);
            Assert.AreEqual(tsVM.RecipientPostalCode, sVM.RecipientPostalCode);
            Assert.AreEqual(tsVM.PickupAddress, sVM.PickupAddress);
            Assert.AreEqual(tsVM.TaxAuthorizationCode, sVM.TaxAuthorizationCode);
            Assert.AreEqual(tsVM.ShipmentAuthorizationCode, sVM.ShipmentAuthorizationCode);
        }

        [Test()]
        public void ShipmentViewModelToShipmentWithoutDBTest()
        {
            Shipment s = new Shipment();
            bC.ShipmentViewModelToShipmentWithoutDB(ref tsVM, ref s);
            Assert.AreEqual(ts.ReferenceNumber, s.ReferenceNumber);
            Assert.AreEqual(ts.PickupType, s.PickupType);
            Assert.AreEqual(ts.NumberOfPackages, s.NumberOfPackages);
            Assert.AreEqual(ts.RecipientName, s.RecipientName);
            Assert.AreEqual(ts.Origin, s.Origin);
            Assert.AreEqual(ts.Destination, s.Destination);
            Assert.AreEqual(ts.EmailWhenPickup, s.EmailWhenPickup);
            Assert.AreEqual(ts.EmailWhenDeliver, s.EmailWhenDeliver);
            Assert.AreEqual(ts.RecipientPhoneNumber, s.RecipientPhoneNumber);
            Assert.AreEqual(ts.RecipientCompanyName, s.RecipientCompanyName);
            Assert.AreEqual(ts.RecipientDepartmentName, s.RecipientDepartmentName);
            Assert.AreEqual(ts.RecipientEmailAddress, s.RecipientEmailAddress);
            Assert.AreEqual(ts.RecipientBuildingAddress, s.RecipientBuildingAddress);
            Assert.AreEqual(ts.RecipientStreetAddress, s.RecipientStreetAddress);
            Assert.AreEqual(ts.RecipientPostalCode, s.RecipientPostalCode);
        }
    }
}