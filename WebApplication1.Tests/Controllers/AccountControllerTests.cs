using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models.Identity;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController controller;
        private ViewResult result;
        [TestInitialize]
        public void SetupContext()
        {
            // Arrange
            controller = new AccountController();
        }
        [TestMethod()]
        public void LoginTest()
        {
            /* LoginViewModel admin = new LoginViewModel
             {
                 UserName = "admin",
                 Password = "123"
             };

             Assert.IsTrue(controller.Login(admin) is RedirectToRouteResult);*/
            Assert.Fail();
        }

        [TestMethod()]
        public void LoginTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogOffTest()
        {
            Assert.Fail();
        }
    }
}