using System.Threading.Tasks;

using GlancePay.OmnivoreIntegration.Dto;

namespace GlancePay.OmnivoreIntegration.UI.Repository
{
    public interface ITicketRepository
    {
        Task<GSPRPaymentCallback> AddPaymentAsync(string locationIdentifier, long transactionId, 
            long ticketId, int amount, int tip, string tenderType, string type);
    }
}