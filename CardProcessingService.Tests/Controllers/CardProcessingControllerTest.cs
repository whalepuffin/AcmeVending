using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardProcessingService.Controllers;
using System.Net.Http;
using System.Web.Http;

namespace CardProcessingService.Tests.Controllers
{
    [TestClass]
    public class CardProcessingControllerTest
    {

        [TestMethod]
        public void Process_With_Valid_CardNumber_OK()
        {
            const string cardNumber = "1234123412341234";

            // Arrange
            var controller = new CardProcessingController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var result = controller.Process(cardNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Process_With_Valid_Length_NotNumeral_BadRequest()
        {
            const string cardNumber = "1234fff412341234";

            // Arrange
            var controller = new CardProcessingController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var result = controller.Process(cardNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Process_With_Null_Or_Whitespace_BadRequest()
        {
            const string cardNumber = "";

            // Arrange
            var controller = new CardProcessingController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var result = controller.Process(cardNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
    }
}
