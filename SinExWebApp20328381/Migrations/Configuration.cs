namespace SinExWebApp20328381.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SinExWebApp20328381.Models.SinExDatabaseContext>
    {
        public Configuration()
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
                new PackageTypeSize { size = "250x350mm", limit = "Not Applicable", PackageTypeID = 1 },
                new PackageTypeSize { size = "350x400mm", limit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { size = "450x500mm", limit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { size = "1000x80mm", limit = "Not Applicable", PackageTypeID = 3 },
                new PackageTypeSize { size = "small - 300x250x150mm", limit = "10kg", PackageTypeID = 4 },
                new PackageTypeSize { size = "medium - 400x350x250mm", limit = "20kg", PackageTypeID = 4 },
                new PackageTypeSize { size = "large - 500x450x350mm", limit = "30kg", PackageTypeID = 4 }
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
                new ServicePackageFee { ServicePackageFeeID = 1, Fee = 160, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 2, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 3, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 4, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 5, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 10:30
                new ServicePackageFee { ServicePackageFeeID = 6, Fee = 140, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 7, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 8, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 9, Fee = 99, MinimumFee = 100, ServiceTypeID = 2, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 10, Fee = 99, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 12:00
                new ServicePackageFee { ServicePackageFeeID = 11, Fee = 130, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 12, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 13, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 14, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 15, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 15:00
                new ServicePackageFee { ServicePackageFeeID = 16, Fee = 120, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 17, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 18, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 19, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 20, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 5 }, // Customer
                                                                                                                                      // 2nd Day
                new ServicePackageFee { ServicePackageFeeID = 21, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 22, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 23, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 24, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 25, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 5 }, // Customer
                                                                                                                                     // Ground
                new ServicePackageFee { ServicePackageFeeID = 26, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 1 },// Envelope
                new ServicePackageFee { ServicePackageFeeID = 27, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 28, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 29, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 30, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 5 }  // Customer
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
                new Destination { City = "Beijing", ProvinceCode = "BJ", CurrencyCode = "CNY" },
                new Destination { City = "Changchun", ProvinceCode = "JL", CurrencyCode = "CNY" },
                new Destination { City = "Changsha", ProvinceCode = "HN", CurrencyCode = "CNY" },
                new Destination { City = "Chengdu", ProvinceCode = "SC", CurrencyCode = "CNY"  },
                new Destination { City = "Chongqing", ProvinceCode = "CQ", CurrencyCode = "CNY" },
                new Destination { City = "Fuzhou", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { City = "Golmud", ProvinceCode = "QH", CurrencyCode = "CNY" },
                new Destination { City = "Guangzhou", ProvinceCode = "GD", CurrencyCode = "CNY" },
                new Destination { City = "Guiyang", ProvinceCode = "GZ", CurrencyCode = "CNY" },
                new Destination { City = "Haikou", ProvinceCode = "HI", CurrencyCode = "CNY" },
                new Destination { City = "Hailar", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { City = "Hangzhou", ProvinceCode = "ZJ", CurrencyCode = "CNY" },
                new Destination { City = "Harbin", ProvinceCode = "HL", CurrencyCode = "CNY" },
                new Destination { City = "Hefei", ProvinceCode = "AH", CurrencyCode = "CNY" },
                new Destination { City = "Hohhot", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { City = "Hong Kong", ProvinceCode = "HK", CurrencyCode = "HKD" },
                new Destination { City = "Hulun Buir", ProvinceCode = "NM", CurrencyCode = "CNY" },
                new Destination { City = "Jinan", ProvinceCode = "SD", CurrencyCode = "CNY" },
                new Destination { City = "Kashi", ProvinceCode = "XJ", CurrencyCode = "CNY" },
                new Destination { City = "Kunming", ProvinceCode = "YN", CurrencyCode = "CNY" },
                new Destination { City = "Lanzhou", ProvinceCode = "GS", CurrencyCode = "CNY" },
                new Destination { City = "Lhasa", ProvinceCode = "XZ", CurrencyCode = "CNY" },
                new Destination { City = "Macau", ProvinceCode = "MC", CurrencyCode = "MOP" },
                new Destination { City = "Nanchang", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { City = "Nanjing", ProvinceCode = "JS", CurrencyCode = "CNY" },
                new Destination { City = "Nanning", ProvinceCode = "JX", CurrencyCode = "CNY" },
                new Destination { City = "Qiqihar", ProvinceCode = "HL", CurrencyCode = "CNY" },
                new Destination { City = "Shanghai", ProvinceCode = "SH", CurrencyCode = "CNY" },
                new Destination { City = "Shenyang", ProvinceCode = "LN", CurrencyCode = "CNY" },
                new Destination { City = "Shijiazhuang", ProvinceCode = "HE", CurrencyCode = "CNY" },
                new Destination { City = "Taipei", ProvinceCode = "TW", CurrencyCode = "TWD" },
                new Destination { City = "Taiyuan", ProvinceCode = "SX", CurrencyCode = "CNY" },
                new Destination { City = "Tianjin", ProvinceCode = "HE", CurrencyCode = "CNY" },
                new Destination { City = "Urumqi", ProvinceCode = "XJ", CurrencyCode = "CNY" },
                new Destination { City = "Wuhan", ProvinceCode = "HB", CurrencyCode = "CNY" },
                new Destination { City = "Xi'an", ProvinceCode = "SN", CurrencyCode = "CNY" },
                new Destination { City = "Xining", ProvinceCode = "QH", CurrencyCode = "CNY" },
                new Destination { City = "Yinchuan", ProvinceCode = "NX", CurrencyCode = "CNY" },
                new Destination { City = "Yumen", ProvinceCode = "GS", CurrencyCode = "CNY" },
                new Destination { City = "Zhengzhou", ProvinceCode = "HA", CurrencyCode = "CNY" }
                );

        }
    }
}
