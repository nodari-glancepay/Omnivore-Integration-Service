using System.Threading.Tasks;

using Unity.Attributes;

using GlancePay.OmnivoreIntegration.Dto;
using GlancePay.OmnivoreIntegration.Logging;
using GlancePay.OmnivoreIntegration.UI.Repository;
using GlancePay.OmnivoreIntegration.ExceptionHandling;

namespace GlancePay.OmnivoreIntegration.UI.Controllers
{
    public class PaymentWorker : IPaymentWorker
    {
        private ITicketRepository ticketRepository;

        public PaymentWorker(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [Dependency]
        protected INLogManager Logger { get; set; }

        public async Task Make3PartyPaymentAsync(string locationIdentifier,
            long transactionId, long ticketId, int amount, int tip, string tenderType, string type)
        {
            GSPRPaymentCallback callbackDto = new GSPRPaymentCallback();
            try
            {
                callbackDto = await ticketRepository.AddPaymentAsync(
                    locationIdentifier, transactionId, ticketId, amount, tip, tenderType, type);
                Logger.LogInfo("Payment added.");
            }
            catch(ApiException ex)
            {
                callbackDto = new GSPRPaymentCallback()
                {
                    ErrorMessage = ex.Message,
                    Success = false,
                    TransactionID = transactionId
                };
                Logger.LogError("Cannot ad payment.", ex);
            }

            //TODO: add call to the endpoint(TBD) and pass to it GSPRPaymentCallback
        }
    }
}