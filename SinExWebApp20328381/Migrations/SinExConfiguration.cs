namespace SinExWebApp20328381.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class SinExConfiguration : DbMigrationsConfiguration<SinExWebApp20328381.Models.SinExDatabaseContext>
    {
        public SinExConfiguration()
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
            context.PackageTypes.AddOrUpdate(
                p => p.PackageTypeID,
                new PackageType { PackageTypeID = 1, Type = "Envelope", Description = "for correspondence and documents only with no commercial value" },
                new PackageType { PackageTypeID = 2, Type = "Pak", Description = "for flat, non-breakable articles including heavy documents" },
                new PackageType { PackageTypeID = 3, Type = "Tube", Description = "for larger documents, such as blueprints, which should be rolled rather than folded" },
                new PackageType { PackageTypeID = 4, Type = "Box", Description = "for bulky items, such as electronic parts and textile samples" },
                new PackageType { PackageTypeID = 5, Type = "Customer", Description = "for packaging provided by customer" }
                );

            context.PackageTypeSizes.AddOrUpdate(
                p => p.PackageTypeSizeID,
                new PackageTypeSize { PackageTypeSizeID = 1, size = "250x350mm", limit = "Not Applicable", PackageTypeID = 1 },
                new PackageTypeSize { PackageTypeSizeID = 2, size = "350x400mm", limit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 3, size = "450x500mm", limit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 4, size = "1000x80mm", limit = "Not Applicable", PackageTypeID = 3 },
                new PackageTypeSize { PackageTypeSizeID = 5, size = "small - 300x250x150mm", limit = "10kg", PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 6, size = "medium - 400x350x250mm", limit = "20kg", PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 7, size = "large - 500x450x350mm", limit = "30kg", PackageTypeID = 4 }
                );
            //context.Currencies.AddOrUpdate()
            // Add service type data.
            context.ServiceTypes.AddOrUpdate(
                p => p.ServiceTypeID,
                new ServiceType { ServiceTypeID = 1, Type = "Same Day", CutoffTime = "10:00 a.m.", DeliveryTime = "Same day" },
                new ServiceType { ServiceTypeID = 2, Type = "Next Day 10:30", CutoffTime = "3:00 p.m.", DeliveryTime = "Next day 10:30 a.m." },
                new ServiceType { ServiceTypeID = 3, Type = "Next Day 12:00", CutoffTime = "6:00 p.m.", DeliveryTime = "Next day 12:00 p.m." },
                new ServiceType { ServiceTypeID = 4, Type = "Next Day 15:00", CutoffTime = "9:00 p.m.", DeliveryTime = "Next day 15:00 p.m." },
                new ServiceType { ServiceTypeID = 5, Type = "2nd Day", CutoffTime = "", DeliveryTime = "5:00 p.m. second business day" },
                new ServiceType { ServiceTypeID = 6, Type = "Ground", CutoffTime = "", DeliveryTime = "1 to 5 business days" }
                );

            // Add service and package type fees.
            context.ServicePackageFees.AddOrUpdate(
                p => p.ServicePackageFeeID,
                // Same Day
                new ServicePackageFee { ServicePackageFeeID = 1, Fee = 160, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 2, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 3, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 4, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 5, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // Next Day 10:30
                new ServicePackageFee { ServicePackageFeeID = 6, Fee = 140, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 7, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 8, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 9, Fee = 99, MinimumFee = 100, ServiceTypeID = 2, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 10, Fee = 99, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // Next Day 12:00
                new ServicePackageFee { ServicePackageFeeID = 11, Fee = 130, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 12, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 13, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 14, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 15, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // Next Day 15:00
                new ServicePackageFee { ServicePackageFeeID = 16, Fee = 120, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 17, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 18, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 19, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 20, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // 2nd Day
                new ServicePackageFee { ServicePackageFeeID = 21, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 22, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 23, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 24, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 25, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                     // Ground
                new ServicePackageFee { ServicePackageFeeID = 26, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 1, Penalty = 500 },// Envelope
                new ServicePackageFee { ServicePackageFeeID = 27, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 28, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 29, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 30, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 5, Penalty = 500 }  // Customer
                );

            context.Currencies.AddOrUpdate(
                p => p.CurrencyCode,
                new Currency { CurrencyCode = "CNY", ExchangeRate = 1.00 },
                new Currency { CurrencyCode = "HKD", ExchangeRate = 1.13 },
                new Currency { CurrencyCode = "MOP", ExchangeRate = 1.16 },
                new Currency { CurrencyCode = "TWD", ExchangeRate = 4.56 }
                );

            context.Destinations.AddOrUpdate(
                p => p.DestinationKey,
                new Destination { DestinationKey = 1, City = "Beijing", ProvinceCode = "BJ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 2, City = "Changchun", ProvinceCode = "JL", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 3, City = "Changsha", ProvinceCode = "HN", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 4, City = "Chengdu", ProvinceCode = "SC", CurrencyCode = "CNY"  },
                new Destination { DestinationKey = 5, City = "Chongqing", ProvinceCode = "CQ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 6, City = "Fuzhou", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 7, City = "Golmud", ProvinceCode = "QH", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 8, City = "Guangzhou", ProvinceCode = "GD", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 9, City = "Guiyang", ProvinceCode = "GZ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 10, City = "Haikou", ProvinceCode = "HI", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 11, City = "Hailar", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 12, City = "Hangzhou", ProvinceCode = "ZJ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 13, City = "Harbin", ProvinceCode = "HL", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 14, City = "Hefei", ProvinceCode = "AH", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 15, City = "Hohhot", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 16, City = "Hong Kong", ProvinceCode = "HK", CurrencyCode = "HKD" },
                new Destination { DestinationKey = 17, City = "Hulun Buir", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 18, City = "Jinan", ProvinceCode = "SD", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 19, City = "Kashi", ProvinceCode = "XJ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 20, City = "Kunming", ProvinceCode = "YN", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 21, City = "Lanzhou", ProvinceCode = "GS", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 22, City = "Lhasa", ProvinceCode = "XZ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 23, City = "Macau", ProvinceCode = "MC", CurrencyCode = "MOP" },
                new Destination { DestinationKey = 24, City = "Nanchang", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 25, City = "Nanjing", ProvinceCode = "JS", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 26, City = "Nanning", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 27, City = "Qiqihar", ProvinceCode = "HL", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 28, City = "Shanghai", ProvinceCode = "SH", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 29, City = "Shenyang", ProvinceCode = "LN", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 30, City = "Shijiazhuang", ProvinceCode = "HE", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 31, City = "Taipei", ProvinceCode = "TW", CurrencyCode = "TWD" },
                new Destination { DestinationKey = 32, City = "Taiyuan", ProvinceCode = "SX", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 33, City = "Tianjin", ProvinceCode = "HE", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 34, City = "Urumqi", ProvinceCode = "XJ", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 35, City = "Wuhan", ProvinceCode = "HB", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 36, City = "Xi'an", ProvinceCode = "SN", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 37, City = "Xining", ProvinceCode = "QH", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 38, City = "Yinchuan", ProvinceCode = "NX", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 39, City = "Yumen", ProvinceCode = "GS", CurrencyCode = "CNY" },
                new Destination { DestinationKey = 40, City = "Zhengzhou", ProvinceCode = "HA", CurrencyCode = "CNY" }
                );
            /*
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
                    new ShipmentStatusHistory { ShipmentStatusHistoryId = 1, WaybillId = , Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 13, 35, 00), TimeValue = "13:35", Description = "Picked up ", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong", Remarks = "Vehicle 34 ", Status = "PickedUp" },

                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 2, WaybillId = , Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 16, 15, 00), TimeValue ="16:15" , Description = "At local sort facility", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 3, WaybillId = , Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 18, 05, 00), TimeValue = "18:05", Description = "Left origin ", DeliveredPlace = null,  DeliveredPerson = null, Location = "HKIA", Remarks = "CX0123 ", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 4, WaybillId = , Date = new DateTime(2017, 04, 06), Time = new DateTime(2017, 04, 06, 20, 18, 00), TimeValue = "20:18", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Pudong", Remarks = "", Status = "PickedUp" },

                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 5, WaybillId = , Date = new DateTime(2017, 04, 07), Time = new DateTime(2017, 04, 07, 06, 38, 00), TimeValue = "06:38", Description = "On vehicle for delivery", DeliveredPlace = null, DeliveredPerson = null, Location = "Pudong", Remarks = "Vehicle 1032", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 6, WaybillId = , Date = new DateTime(2017, 04, 07), Time = new DateTime(2017, 04, 07, 08, 48, 00), TimeValue = "08:48", Description = "Delivered ", DeliveredPlace = "Front door", DeliveredPerson = "Monica Mok", Location = "Shanghai ", Remarks = "", Status = "Delivered" },



                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 7, WaybillId = , Date = new DateTime(2017, 04, 10), Time = new DateTime(2017, 04, 10, 16, 45, 00), TimeValue = "16:45", Description = "Picked up ", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong", Remarks = "Vehicle 12", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 8, WaybillId = , Date = new DateTime(2017, 04, 10), Time = new DateTime(2017, 04, 10, 20, 10, 00), TimeValue = "20:10", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },

                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 9, WaybillId = , Date = new DateTime(2017, 04, 11), Time = new DateTime(2017, 04, 11, 10, 18, 00), TimeValue ="10:18" , Description = "Left origin ", DeliveredPlace = null, DeliveredPerson = null, Location = "HKIA ", Remarks = "KA3845", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 10, WaybillId = , Date = new DateTime(2017, 04, 11), Time = new DateTime(2017, 04, 11, 15, 28, 00), TimeValue = "15:28", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Lanzhou ", Remarks = , Status = "PickedUp" },

                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 11, WaybillId = , Date = new DateTime(2017, 04, 12), Time = new DateTime(2017, 04, 12, 07, 38, 00), TimeValue ="07:38" , Description = "On vehicle for delivery ", DeliveredPlace  = null, DeliveredPerson = null, Location = "Lanzhou ", Remarks = "Vehicle 82", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 12, WaybillId = , Date = new DateTime(2017, 04, 12), Time = new DateTime(2017, 04, 12, 10, 13, 00), TimeValue = "10:13", Description = "Delivered", DeliveredPlace = "Front door", DeliveredPerson = "George Guo", Location = "Lanzhou ", Remarks = "", Status ="Delivered" },




                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 13, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 07, 55, 00), TimeValue ="07:55" , Description = "Picked up", DeliveredPlace = null, DeliveredPerson = null, Location = "Hong Kong ", Remarks = "Vehicle 13", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 14, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 09, 08, 00), TimeValue = "09:08", Description = "At local sort facility ", DeliveredPlace = null, DeliveredPerson = null, Location = "Tung Chung", Remarks = "", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 15, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 10, 18, 00), TimeValue = "10:18", Description = "Left origin", DeliveredPlace = null, DeliveredPerson = null, Location = "HKIA", Remarks = "KA3845 ", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 16, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 15, 28, 00), TimeValue = "15:28", Description = "At local sort facility", DeliveredPlace = null, DeliveredPerson = null, Location = "Fuzhou", Remarks = "", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 17, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 15, 50, 00), TimeValue = "15:50", Description = "On vehicle for delivery", DeliveredPlace = null, DeliveredPerson = null, Location = "Fuzhou", Remarks = "Vehicle 82 ", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 18, WaybillId = , Date = new DateTime(2017, 04, 14), Time = new DateTime(2017, 04, 14, 16, 53, 00), TimeValue = "15:53", Description = "Delivered", DeliveredPlace = " Front door", DeliveredPerson = "Sammy So", Location = "Fuzhou", Remarks = "", Status = "Delivered" },



                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 19, WaybillId = , Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 08, 30, 00), TimeValue = "08:30", Description = "Picked up", DeliveredPlace = null, DeliveredPerson = null, Location = "", Remarks = "Vehicle 12", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 20, WaybillId = , Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 10, 00, 00), TimeValue = "10:00", Description = "At local sort facility", DeliveredPlace =null , DeliveredPerson = null, Location = "", Remarks = "", Status = "PickedUp" },
                     new ShipmentStatusHistory { ShipmentStatusHistoryId = 21, WaybillId = , Date = new DateTime(2017, 05, 02), Time = new DateTime(2017, 05, 02, 10, 35, 00), TimeValue = "10:35", Description = "Left origin ", DeliveredPlace = null, DeliveredPerson = null , Location = "", Remarks = "Vehicle 667 ", Status = "PickedUp" }
                 );
                 
            context.Invoices.AddOrUpdate(
            p => p.ServiceTypeID,
            new Invoice { InvoiceId = 1,WaybillId = 1, TotalCostCurrency = "CNY", TotalCost=(decimal)280.00,ShippingAccountId=2 },
            new Invoice { InvoiceId = 2, WaybillId = 2, TotalCostCurrency = "CNY", TotalCost = (decimal)50.00, ShippingAccountId = 1 },
            new Invoice { InvoiceId = 3, WaybillId = 3, TotalCostCurrency = "CNY", TotalCost = (decimal)413.00, ShippingAccountId = 1 },
            new Invoice { InvoiceId = 4, WaybillId = 4, TotalCostCurrency = "CNY", TotalCost = (decimal)200.00, ShippingAccountId = 3 }
            );*/
        }
    }
}