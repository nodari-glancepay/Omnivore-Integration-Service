using System.Threading.Tasks;

namespace GlancePay.OmnivoreIntegration.UI
{
    public interface IPaymentWorker
    {
        Task Make3PartyPaymentAsync(string locationIdentifier,
            long transactionId, long ticketId, int amount, int tip, string tenderType, string type);
    }
}
