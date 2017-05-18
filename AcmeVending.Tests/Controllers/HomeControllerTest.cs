using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcmeVending;
using AcmeVending.Controllers;
using AcmeVending.Repositories;
using Moq;

namespace AcmeVending.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        [TestMethod]
        public void Index()
        {

            // Arrange
            var mockProductService = new Mock<IProductService>();
            var mockCardProcessingRepository = new Mock<ICardProcessingRepository>();
            HomeController controller = new HomeController(mockProductService.Object, mockCardProcessingRepository.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var mockCardProcessingRepository = new Mock<ICardProcessingRepository>();
            HomeController controller = new HomeController(mockProductService.Object, mockCardProcessingRepository.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var mockCardProcessingRepository = new Mock<ICardProcessingRepository>();
            HomeController controller = new HomeController(mockProductService.Object, mockCardProcessingRepository.Object);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
