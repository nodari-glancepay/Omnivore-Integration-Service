namespace GlancePay.OmnivoreIntegration.Dto
{
    public class GSPRPaymentCallback
    {
        public long TransactionID { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
