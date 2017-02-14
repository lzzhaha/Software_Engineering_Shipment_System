using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.Controllers
{
    public class DestinationsController : Controller
    {
        // GET: Destinations
        public ActionResult Index()
        {
            return View(PopulateDestination());
        }
        private List<Destination> PopulateDestination()
        {
            var destinations = new List<Destination>();
            destinations.Add(new Destination { City = "Beijing", ProvinceCode = "BJ" });
            destinations.Add(new Destination { City = "Changchun", ProvinceCode = "JL" });
            destinations.Add(new Destination { City = "Changsha", ProvinceCode = "HN" });
            destinations.Add(new Destination { City = "Chengdu", ProvinceCode = "SC" });
            destinations.Add(new Destination { City = "Chongqing", ProvinceCode = "CQ" });
            destinations.Add(new Destination { City = "Fuzhou", ProvinceCode = "JX" });
            destinations.Add(new Destination { City = "Golmud", ProvinceCode = "QH" });
            destinations.Add(new Destination { City = "Guangzhou", ProvinceCode = "GD" });
            destinations.Add(new Destination { City = "Guiyang", ProvinceCode = "GZ" });
            destinations.Add(new Destination { City = "Haikou", ProvinceCode = "HI" });
            destinations.Add(new Destination { City = "Hailar", ProvinceCode = "NM" });
            destinations.Add(new Destination { City = "Hangzhou", ProvinceCode = "ZJ" });
            destinations.Add(new Destination { City = "Harbin", ProvinceCode = "HL" });
            destinations.Add(new Destination { City = "Hefei", ProvinceCode = "AH" });
            destinations.Add(new Destination { City = "Hohhot", ProvinceCode = "NM" });
            destinations.Add(new Destination { City = "Hong Kong", ProvinceCode = "HK" });
            destinations.Add(new Destination { City = "Hulun Buir", ProvinceCode = "NM" });
            destinations.Add(new Destination { City = "Jinan", ProvinceCode = "SD" });
            destinations.Add(new Destination { City = "Kashi", ProvinceCode = "XJ" });
            destinations.Add(new Destination { City = "Kunming", ProvinceCode = "YN" });
            destinations.Add(new Destination { City = "Lanzhou", ProvinceCode = "GS" });
            destinations.Add(new Destination { City = "Lhasa", ProvinceCode = "XZ" });
            destinations.Add(new Destination { City = "Macau", ProvinceCode = "MC" });
            destinations.Add(new Destination { City = "Nanchang", ProvinceCode = "JX" });
            destinations.Add(new Destination { City = "Nanjing", ProvinceCode = "JS" });
            destinations.Add(new Destination { City = "Nanning", ProvinceCode = "JX" });
            destinations.Add(new Destination { City = "Qiqihar", ProvinceCode = "HL" });
            destinations.Add(new Destination { City = "Shanghai", ProvinceCode = "SH" });
            destinations.Add(new Destination { City = "Shenyang", ProvinceCode = "LN" });
            destinations.Add(new Destination { City = "Shijiazhuang", ProvinceCode = "HE" });
            destinations.Add(new Destination { City = "Taipei", ProvinceCode = "TW" });
            destinations.Add(new Destination { City = "Taiyuan", ProvinceCode = "SX" });
            destinations.Add(new Destination { City = "Tianjin", ProvinceCode = "HE" });
            destinations.Add(new Destination { City = "Urumqi", ProvinceCode = "XJ" });
            destinations.Add(new Destination { City = "Wuhan", ProvinceCode = "HB" });
            destinations.Add(new Destination { City = "Xi'an", ProvinceCode = "SN" });
            destinations.Add(new Destination { City = "Xining", ProvinceCode = "QH" });
            destinations.Add(new Destination { City = "Yinchuan", ProvinceCode = "NX" });
            destinations.Add(new Destination { City = "Yumen", ProvinceCode = "GS" });
            destinations.Add(new Destination { City = "Zhengzhou", ProvinceCode = "HA" });
            return (destinations);
        }

    }
}
