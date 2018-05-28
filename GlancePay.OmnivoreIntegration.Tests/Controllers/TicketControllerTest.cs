using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GlancePay.OmnivoreIntegration.Business;
using GlancePay.OmnivoreIntegration.Service.Controllers;
using GlancePay.OmnivoreIntegration.Service.Repository;
using System.Web.Http;
using GlancePay.OmnivoreIntegration.ExceptionHandling;

namespace GlancePay.OmnivoreIntegration.Tests.Controllers
{
    [TestClass]
    public class TicketControllerTest
    {
        [TestMethod]
        [ExpectedException (typeof(BadRequestException), "GSPRPaymentRequest is invalid")]
        public void TicketController_Throws_BadRequestException_When_Payment_Is_Missed()
        {
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(null);
            PaymentWorker paymentWorker = new PaymentWorker(ticketRepository);
            TicketController controller = new TicketController(paymentWorker);
            GSPRPaymentRequest paymentRequest = new
                GSPRPaymentRequest()
            {
                
                BillNo = 123,
                PaymentTypeId = "100",                
                TipAmount = 0,
                TransactionID = 123,
                LocationIdentifier = "ABC",
                PaymentTypeName = "3rd_party",
                GlancePayPOSPaymentID = 123
            };

            MockControllerRequest(controller);

            // Act
            controller.Validate(paymentRequest);
            HttpResponseMessage result = controller.Post(paymentRequest) as HttpResponseMessage;
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException), "GSPRPaymentRequest is invalid")]
        public void TicketController_Throws_BadRequestException_When_Payment_Is_Zero()
        {
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(null);
            PaymentWorker paymentWorker = new PaymentWorker(ticketRepository);
            TicketController controller = new TicketController(paymentWorker);
            GSPRPaymentRequest paymentRequest = new
                GSPRPaymentRequest()
            {
                BillNo = 123,
                BillAmount = 0,
                PaymentTypeId = "100",
                TipAmount = 0,
                TransactionID = 123,
                LocationIdentifier = "ABC",
                PaymentTypeName = "3rd_party",
                GlancePayPOSPaymentID = 123
            };

            MockControllerRequest(controller);

            // Act
            controller.Validate(paymentRequest);
            HttpResponseMessage result = controller.Post(paymentRequest) as HttpResponseMessage;
        }

        #region Helper Methods

        private void MockControllerRequest(TicketController controller)
        {
            //Mock Http request:
            HttpConfiguration config = new HttpConfiguration();
            HttpRequestMessage request = new HttpRequestMessage();
            controller.Request = request;
            controller.Request.Properties["MS_HttpConfiguration"] = config;
        }

        #endregion Helper Methods
    }
}
