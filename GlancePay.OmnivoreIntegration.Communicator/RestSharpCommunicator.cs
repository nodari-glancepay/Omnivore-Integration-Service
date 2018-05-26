using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using RestSharp;
using RestSharp.Deserializers;

using GlancePay.OmnivoreIntegration.Dto;
using GlancePay.OmnivoreIntegration.Logging;
using GlancePay.OmnivoreIntegration.ExceptionHandling;

namespace GlancePay.OmnivoreIntegration.Communicator
{
    public class RestSharpCommunicator
    {
        //TODO: remove hardcoded value. Get OMNIVORE_API_KEY from suitable storage (TBD)
        const string OMNIVORE_API_KEY = "9ae84eb23c234a7ebdaaf07695fc872e"; //default development API key
        const string BASE_URI = "https://api.omnivore.io/1.0/locations/";

        public INLogManager Logger { get; set; }

        /// <summary>
        /// Add payment under the ticket using Glance Pay payment as an Omnivore 3rd Party Payment. 
        /// </summary>
        /// <param name="ticketId">Ticket unique identifier</param>
        /// <param name="amountToPay">the amount in cents excluding tips to be paid</param>
        /// <param name="tipToPay">The amount of tips to be paid</param>
        /// <param name="tenderType">Identifier of party payment type. It changes depending on POS location.</param>
        /// <param name="typeName">Configurable name of tender type. 3rd_party for Omnivar virtual POS.</param>
        /// <returns>PaymentCompleted DTO</returns>
        public async Task<PaymentCompleted> AddPayment(string locationIdentifier, long transactionId, long ticketId, 
            int amountToPay, int tipToPay, string tenderType, string typeName)
        {
            Stopwatch watch;
            string errorMessage = string.Empty;

#if DEBUG
            watch = Stopwatch.StartNew();
            Logger.LogTrace("RestClient creation.");
#endif

            string resource = string.Format("{0}/tickets/{1}/payments/", locationIdentifier, ticketId.ToString());

            RestClient restClient = new RestClient(BASE_URI);
            var restRequest = new RestRequest(resource, Method.POST);

            // Add HTTP Headers
            restRequest.AddHeader("Api-Key", OMNIVORE_API_KEY);
            restRequest.AddHeader("Content-Type", "application/json");
            
            //Add parameters to body as Json 
            restRequest.AddJsonBody(new
            {
                amount = amountToPay,
                tip = tipToPay,
                tender_type = tenderType,
                type = typeName
            });

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Logger.LogTrace("Omnivore API call start.");
            IRestResponse asyncResp = await restClient.ExecuteTaskAsync(restRequest, cancellationTokenSource.Token);
            Logger.LogTrace("Omnivore API call end.");

            if(asyncResp == null) throw new InternalServerErrorException("IRestResponse is null.");

            PaymentCompleted paymentCompleted = null;
            RestResponse response = new RestResponse();
            response.Content = asyncResp.Content;
            JsonDeserializer deserializer = new JsonDeserializer();
            paymentCompleted = deserializer.Deserialize<PaymentCompleted>(response);
            paymentCompleted.Successful = asyncResp.IsSuccessful;
            
#if DEBUG
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.LogTrace(string.Format("AddPayment time: {0} msec.", elapsedMs.ToString()));
#endif
            Logger.LogDebug("Communicator: payment end");
            if (!paymentCompleted.Successful) ThrowApiException(paymentCompleted);
            return paymentCompleted;
        }

        #region Private methods

        /// <summary>
        /// Handle errors returned by Omnivore.
        /// <ex>{ "errors": [{"description": "Invalid payment type", "error": "invalid_input", "fields": ["$.type"]}]}</ex>
        /// <ex>{ "errors": [{"description": "Cannot make ticket payment because the ticket is closed.", "error": "ticket_closed"}]}</ex>
        /// </summary>
        /// <param name="paymentCompleted">DTO with payment result data.</param>
        private void ThrowApiException(PaymentCompleted paymentCompleted)
        {
            if (paymentCompleted.Successful) return;
            LogErrorMessage(paymentCompleted);
            ErrorSlug errorSlaug = paymentCompleted.Errors.Where(x => !string.IsNullOrEmpty(x.Error)).FirstOrDefault();

            if(errorSlaug == null) throw new InternalServerErrorException("No error slug.");

            HttpStatusCode statusCode = errorSlaug.Error.ToHttpStatusCode();
            ApiException apiException = statusCode.ToApiException(errorSlaug.Description);
            throw apiException;                
        }

        private void LogErrorMessage(PaymentCompleted paymentCompleted)
        {
            string errorMessage = paymentCompleted.ToErrorMessage();
            if (!string.IsNullOrEmpty(errorMessage)) Logger.LogError(errorMessage);
        }

        #endregion Private methods
    }
}
