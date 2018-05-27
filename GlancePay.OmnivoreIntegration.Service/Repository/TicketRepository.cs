using System.Threading.Tasks;

using GlancePay.OmnivoreIntegration.Communicator;
using GlancePay.OmnivoreIntegration.Dto;
using GlancePay.OmnivoreIntegration.Logging;

namespace GlancePay.OmnivoreIntegration.Service.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private RestSharpCommunicator communicator;
        private INLogManager logger;

        public TicketRepository(INLogManager nLogManager)
        {
            logger = nLogManager;
            communicator = new RestSharpCommunicator
            {
                Logger = nLogManager
            };
            logger.LogDebug("RestSharpCommunicator created.");
        }

        public async Task<GSPRPaymentCallback> AddPaymentAsync(string locationIdentifier, long transactionId, 
            long ticketId, int amount, int tip, string tenderType, string type)
        {
            string generalErrorMessage = "Cannot add payment.";

            PaymentCompleted response = await communicator.AddPayment(locationIdentifier, transactionId, ticketId, amount, tip, tenderType, type);

            GSPRPaymentCallback calbackDto = new GSPRPaymentCallback()
            {
                TransactionID = transactionId,
                Success = (response == null) ? false : response.Successful,
            };

            if (!calbackDto.Success)
            {
                logger.LogWarn(generalErrorMessage);
                calbackDto.ErrorMessage = generalErrorMessage;
            }
            
            return calbackDto;
        }
    }
}