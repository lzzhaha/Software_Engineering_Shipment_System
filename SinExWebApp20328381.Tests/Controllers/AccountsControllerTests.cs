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
    class AccountsControllerTests
    {
        [Test()]
        public void CreditCardExpiryCheckTest()
        {
            var controller = new AccountController();
            //past year
            string year1 = "2010";
            int month1 = 7;
            //same year, past month
            string year2 = DateTime.Now.Year.ToString();
            int month2 = DateTime.Now.Month-1;

            var result = controller.CreditCardExpiryCheck(year1,month1) as ViewResult;
            Assert.AreEqual("The card is expired.", result.ViewBag.ExpiryMessage);

            result = controller.CreditCardExpiryCheck(year2, month2) as ViewResult;
            Assert.AreEqual("The card is expired.", result.ViewBag.ExpiryMessage);
        }
    }
}
