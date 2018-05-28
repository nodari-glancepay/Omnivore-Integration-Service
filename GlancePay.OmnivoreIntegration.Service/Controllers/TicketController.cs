using System;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

using GlancePay.OmnivoreIntegration.Business;
using GlancePay.OmnivoreIntegration.ExceptionHandling;

namespace GlancePay.OmnivoreIntegration.Service.Controllers
{
    [RoutePrefix("api")]
    public class TicketController : BaseApiController
    {
        private IPaymentWorker paymentWorker;

        public TicketController(IPaymentWorker paymentWorker) : base()
        {
            this.paymentWorker = paymentWorker;
        }

        [Route("payment")]
        public HttpResponseMessage Post([FromBody]GSPRPaymentRequest paymentRequest)
        {
            Logger.LogDebug("Start.");
            if (!ModelState.IsValid)
            {
                string errorMessage = ModelState.FirstErrorMessage();
                Logger.LogError(string.Format("{0} Transaction ID: {1}", errorMessage, paymentRequest.TransactionID));
                throw new BadRequestException(errorMessage.ToCombinedErrorMessage(paymentRequest.TransactionID));
            }

            ThirdPartyPayment payment = new ThirdPartyPayment(paymentRequest.BillAmount, paymentRequest.TipAmount)
            {
                TicketId = paymentRequest.BillNo,
                TransactionId = paymentRequest.TransactionID,
                LocationIdentifier = paymentRequest.LocationIdentifier,
                Amount = paymentRequest.BillAmount,
                Tip = paymentRequest.TipAmount,
                TenderTypeId = paymentRequest.PaymentTypeId,
                Type = paymentRequest.PaymentTypeName                
            };

            HostingEnvironment.QueueBackgroundWorkItem(async cancelationToken => 
            {
                Logger.LogDebug("Background worker start.");
                try
                {
                    await paymentWorker.Make3PartyPaymentAsync(payment.LocationIdentifier, payment.TransactionId,
                                             payment.TicketId, payment.Amount, payment.Tip, payment.TenderTypeId, payment.Type);
                }
                catch (Exception ex)
                {
                    //TODO: add logging and exception handling
                    Logger.LogError("Cannot add payment: " + ex.Message, ex);
                }
                Logger.LogDebug("Background worker end.");
            });

            Logger.LogDebug("End.");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}