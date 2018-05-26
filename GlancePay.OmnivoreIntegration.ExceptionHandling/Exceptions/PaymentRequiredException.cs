using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling.Exceptions
{
    [Serializable]
    public class PaymentRequiredException : ApiException
    {
        public PaymentRequiredException(string message)
            : base(HttpStatusCode.PaymentRequired, HttpStatusCode.PaymentRequired.ToString(), message) { }
    }
}
