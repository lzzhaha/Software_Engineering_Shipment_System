namespace SinExWebApp20328381.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ShipmentInvoiceConfiguration : DbMigrationsConfiguration<SinExWebApp20328381.Models.SinExDatabaseContext>
    {
        public ShipmentInvoiceConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SinExWebApp20328381.Models.SinExDatabaseContext";
        }

        protected override void Seed(SinExWebApp20328381.Models.SinExDatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            // Add package type data.


            context.Shipments.AddOrUpdate(
                p => p.WaybillId,
                new Shipment { WaybillId = 1, ServiceType = "Next Day 10:30", RecipientName = "Monica Mok", RecipientBuildingAddress = "Flat A 15/F Tower 2 Golden Estate", RecipientStreetAddress = "12 Mandarin Drive", RecipientCityAddress = "Shanghai", Destination = "Shanghai", Origin = "Hong Kong", RecipientPostalCode = "207345", RecipientPhoneNumber = "862167890123", RecipientEmailAddress = "comp3111_team105@cse.ust.hk", ShipmentShippingAccountId = "000000000002", TaxAndDutyShippingAccountId = "000000000002", EmailWhenDeliver = false, EmailWhenPickup = false, PickupAddress = "Flat A, 20/F, Block A, Galaxia, 275 Fung Tak Rd, Hong Kong, HK", PickupType = "immediate", ShippedDate = new DateTime(2017, 04, 06, 12, 55, 00), Duty = 0, DutyCurrency = "HKD", Tax = 0, TaxCurreny = "HKD", TaxAuthorizationCode = "8261", ShipmentAuthorizationCode = "8261", DeliveredDate = new DateTime(1900, 1, 1, 0, 0, 0), ReferenceNumber = "", NumberOfPackages = 2, Status = "PickedUp", ShippingAccountId = 1 },
                new Shipment { WaybillId = 2, ServiceType = "2nd Day", RecipientName = "George Guo", RecipientStreetAddress = "333 Golden Terrace", RecipientCityAddress = "Lanzhou", Destination = "Lanzhou", Origin = "Hong Kong", RecipientPostalCode = "737373", RecipientPhoneNumber = "8693177770123", RecipientEmailAddress = "comp3111_team105@cse.ust.hk", ShipmentShippingAccountId = "000000000001", TaxAndDutyShippingAccountId = "000000000001", EmailWhenDeliver = false, EmailWhenPickup = false, PickupAddress = "Flat A, 20/F, Block A, Galaxia, 275 Fung Tak Rd, Hong Kong, HKSAR", PickupType = "immediate", ShippedDate = new DateTime(2017, 04, 10, 14, 35, 00), Duty = 100, DutyCurrency = "HKD", Tax = 0, TaxCurreny = "HKD", TaxAuthorizationCode = "7281", ShipmentAuthorizationCode = "7281", DeliveredDate = new DateTime(1900, 1, 1, 0, 0, 0), ReferenceNumber = "", NumberOfPackages = 1, Status = "PickedUp", ShippingAccountId = 1 },
                new Shipment { WaybillId = 3, ServiceType = "Same Day", RecipientName = "Sammy So", RecipientStreetAddress = "12 Blossom Drive", RecipientCityAddress = "Fuzhou", Destination = "Fuzhou", Origin = "Hong Kong", RecipientPostalCode = "356655", RecipientPhoneNumber = "8659166660123", RecipientEmailAddress = "comp3111_team105@cse.ust.hk", ShipmentShippingAccountId = "000000000001", TaxAndDutyShippingAccountId = "000000000001", EmailWhenDeliver = false, EmailWhenPickup = false, PickupAddress = "Flat A, 20/F, Block A, Galaxia, 275 Fung Tak Rd, Hong Kong, HKSAR", PickupType = "immediate", ShippedDate = new DateTime(2017, 04, 14, 07, 30, 00), Duty = 250, DutyCurrency = "HKD", Tax = 125, TaxCurreny = "HKD", TaxAuthorizationCode = "4312", ShipmentAuthorizationCode = "4312", DeliveredDate = new DateTime(1900, 1, 1, 0, 0, 0), ReferenceNumber = "", NumberOfPackages = 2, Status = "PickedUp", ShippingAccountId = 1 },
                new Shipment { WaybillId = 4, ServiceType = "Ground", RecipientName = "iGear Computing", RecipientStreetAddress = "18 Huaubaishu Road", RecipientCityAddress = "Wuhan", Destination = "Wuhan", Origin = "Hong Kong", RecipientPostalCode = "433456", RecipientPhoneNumber = "8659166660123", RecipientEmailAddress = "comp3111_team105@cse.ust.hk", ShipmentShippingAccountId = "000000000003", TaxAndDutyShippingAccountId = "000000000003", EmailWhenDeliver = false, EmailWhenPickup = false, PickupAddress = "Flat A, 20/F, Block A, Galaxia, 275 Fung Tak Rd, Hong Kong, HKSAR", PickupType = "immediate", ShippedDate = new DateTime(2017, 05, 02, 08, 00, 00), Duty = 0, DutyCurrency = "HKD", Tax = 0, TaxCurreny = "HKD", TaxAuthorizationCode = "9318", ShipmentAuthorizationCode = "9318", DeliveredDate = new DateTime(1900, 1, 1, 0, 0, 0), ReferenceNumber = "", NumberOfPackages = 4, Status = "PickedUp", ShippingAccountId = 1 }
            );

            context.Packages.AddOrUpdate(
                p => p.PackageId,
                new Package { PackageId = 1, WaybillId = 1, Weight = 1, ActualWeight = 1, PackageTypeID = 1, PackageTypeSizeID = 1, Value = 50, ValueCurrency = "HKD", Description = "Correspondence", Cost = 140 },
                new Package { PackageId = 2, WaybillId = 1, Weight = 1, ActualWeight = 1, PackageTypeID = 1, PackageTypeSizeID = 1, Value = 50, ValueCurrency = "HKD", Description = "Correspondence", Cost = 140 },
                new Package { PackageId = 3, WaybillId = 2, Weight = (decimal)0.4, ActualWeight = (decimal)0.5, PackageTypeID = 2, PackageTypeSizeID = 2, Value = 2600, ValueCurrency = "HKD", Description = "Apple iPad mini", Cost = 50 },
                new Package { PackageId = 4, WaybillId = 3, Weight = (decimal)0.6, ActualWeight = (decimal)0.5, PackageTypeID = 3, PackageTypeSizeID = 4, Value = 1000, ValueCurrency = "HKD", Description = "Painting", Cost = 160 },
                new Package { PackageId = 5, WaybillId = 3, Weight = (decimal)2.3, ActualWeight = (decimal)2.3, PackageTypeID = 4, PackageTypeSizeID = 5, Value = 1500, ValueCurrency = "HKD", Description = "Perfume", Cost = 253 },
                new Package { PackageId = 6, WaybillId = 4, Weight = (decimal)1, ActualWeight = (decimal)1, PackageTypeID = 1, PackageTypeSizeID = 1, Value = 50, ValueCurrency = "HKD", Description = "Manual", Cost = 25 },
                new Package { PackageId = 7, WaybillId = 4, Weight = (decimal)1.5, ActualWeight = (decimal)1.4, PackageTypeID = 2, PackageTypeSizeID = 2, Value = 200, ValueCurrency = "HKD", Description = "Samples", Cost = 35 },
                new Package { PackageId = 8, WaybillId = 4, Weight = (decimal)4.6, ActualWeight = (decimal)4.6, PackageTypeID = 2, PackageTypeSizeID = 3, Value = 200, ValueCurrency = "HKD", Description = "Samples", Cost = 115 },
                new Package { PackageId = 9, WaybillId = 4, Weight = (decimal)1, ActualWeight = (decimal)1, PackageTypeID = 3, PackageTypeSizeID = 4, Value = 50, ValueCurrency = "HKD", Description = "Design specifications", Cost = 25 }

                );
            
            
            context.ShipmentStatusHistories.AddOrUpdate(
                //p=>p.ShipmentStatusHistoryId,
               new ShipmentStatusHistory { ShipmentStatusHistoryId = 1, WaybillId = 1, Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 13, 35, 00), TimeValue = "13:35", Description = "Picked up ", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong", Remarks = "Vehicle 34 ", Status = "PickedUp" },

                new ShipmentStatusHistory { ShipmentStatusHistoryId = 2, WaybillId = 1, Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 16, 15, 00), TimeValue = "16:15", Description = "At local sort facility", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 3, WaybillId = 1, Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 18, 05, 00), TimeValue = "18:05", Description = "Left origin ", DeliveredPlace = null, DeliveredPerson = null, Location = "HKIA", Remarks = "CX0123 ", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 4, WaybillId = 1, Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 20, 18, 00), TimeValue = "20:18", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Pudong", Remarks = "", Status = "PickedUp" },

                new ShipmentStatusHistory { ShipmentStatusHistoryId = 5, WaybillId = 1, Date = new DateTime(2017, 04, 07), Time = new DateTime(2017, 04, 07, 06, 38, 00), TimeValue = "06:38", Description = "On vehicle for delivery", DeliveredPlace = null, DeliveredPerson = null, Location = "Pudong", Remarks = "Vehicle 1032", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 6, WaybillId = 1, Date = new DateTime(2017, 04, 07), Time = new DateTime(2017, 04, 07, 08, 48, 00), TimeValue = "08:48", Description = "Delivered ", DeliveredPlace = "Front door", DeliveredPerson = "Monica Mok", Location = "Shanghai ", Remarks = "", Status = "Delivered" },



                new ShipmentStatusHistory { ShipmentStatusHistoryId = 7, WaybillId = 2, Date = new DateTime(2017, 04, 10), Time = new DateTime(2017, 04, 10, 16, 45, 00), TimeValue = "16:45", Description = "Picked up ", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong", Remarks = "Vehicle 12", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 8, WaybillId = 2, Date = new DateTime(2017, 04, 10), Time = new DateTime(2017, 04, 10, 20, 10, 00), TimeValue = "20:10", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },

                new ShipmentStatusHistory { ShipmentStatusHistoryId = 9, WaybillId = 2, Date = new DateTime(2017, 04, 11), Time = new DateTime(2017, 04, 11, 10, 18, 00), TimeValue = "10:18", Description = "Left origin ", DeliveredPlace = null, DeliveredPerson = null, Location = "HKIA ", Remarks = "KA3845", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 10, WaybillId = 2, Date = new DateTime(2017, 04, 11), Time = new DateTime(2017, 04, 11, 15, 28, 00), TimeValue = "15:28", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Lanzhou ", Remarks = "", Status = "PickedUp" },

                new ShipmentStatusHistory { ShipmentStatusHistoryId = 11, WaybillId = 2, Date = new DateTime(2017, 04, 12), Time = new DateTime(2017, 04, 12, 07, 38, 00), TimeValue = "07:38", Description = "On vehicle for delivery ", DeliveredPlace = null, DeliveredPerson = null, Location = "Lanzhou ", Remarks = "Vehicle 82", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 12, WaybillId = 2, Date = new DateTime(2017, 04, 12), Time = new DateTime(2017, 04, 12, 10, 13, 00), TimeValue = "10:13", Description = "Delivered", DeliveredPlace = "Front door", DeliveredPerson = "George Guo", Location = "Lanzhou ", Remarks = "", Status = "Delivered" },




                new ShipmentStatusHistory { ShipmentStatusHistoryId = 13, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 07, 55, 00), TimeValue = "07:55", Description = "Picked up", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong ", Remarks = "Vehicle 13", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 14, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 09, 08, 00), TimeValue = "09:08", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 15, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 10, 18, 00), TimeValue = "10:18", Description = "Left origin", DeliveredPlace = null, DeliveredPerson = null, Location = "HKIA", Remarks = "KA3845 ", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 16, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 15, 28, 00), TimeValue = "15:28", Description = "At local sort facility", DeliveredPlace = null, DeliveredPerson = null, Location = "Fuzhou", Remarks = "", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 17, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 15, 50, 00), TimeValue = "15:50", Description = "On vehicle for delivery", DeliveredPlace = null, DeliveredPerson = null, Location = "Fuzhou", Remarks = "Vehicle 82 ", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 18, WaybillId = 3, Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 16, 53, 00), TimeValue = "15:53", Description = "Delivered", DeliveredPlace = " Front door", DeliveredPerson = "Sammy So", Location = "Fuzhou", Remarks = "", Status = "Delivered" },



                new ShipmentStatusHistory { ShipmentStatusHistoryId = 19, WaybillId = 4, Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 08, 30, 00), TimeValue = "08:30", Description = "Picked up", DeliveredPlace = null, DeliveredPerson = null, Location = "", Remarks = "Vehicle 12", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 20, WaybillId = 4, Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 10, 00, 00), TimeValue = "10:00", Description = "At local sort facility", DeliveredPlace = null, DeliveredPerson = null, Location = "", Remarks = "", Status = "PickedUp" },
                new ShipmentStatusHistory { ShipmentStatusHistoryId = 21, WaybillId = 4, Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 10, 35, 00), TimeValue = "10:35", Description = "Left origin ", DeliveredPlace = null, DeliveredPerson = null, Location = "", Remarks = "Vehicle 667 ", Status = "PickedUp" }
            );
            
            context.Invoices.AddOrUpdate(
            p => p.InvoiceId,
            new Invoice { InvoiceId = 1, WaybillId = 1, TotalCostCurrency = "CNY", TotalCost = (decimal)280.00, ShippingAccountId = 2 },
            new Invoice { InvoiceId = 2, WaybillId = 2, TotalCostCurrency = "CNY", TotalCost = (decimal)50.00, ShippingAccountId = 1 },
            new Invoice { InvoiceId = 3, WaybillId = 3, TotalCostCurrency = "CNY", TotalCost = (decimal)413.00, ShippingAccountId = 1 },
            new Invoice { InvoiceId = 4, WaybillId = 4, TotalCostCurrency = "CNY", TotalCost = (decimal)200.00, ShippingAccountId = 1 }
            );
            

        }
    }
}