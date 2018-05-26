using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GlancePay.OmnivoreIntegration.UI.Controllers;

namespace GlancePay.OmnivoreIntegration.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
