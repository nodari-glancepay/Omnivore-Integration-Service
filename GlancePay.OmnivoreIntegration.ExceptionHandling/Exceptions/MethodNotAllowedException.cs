using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling.Exceptions
{
    [Serializable]
    public class MethodNotAllowedException : ApiException
    {
        public MethodNotAllowedException(string message)
            : base(HttpStatusCode.MethodNotAllowed, HttpStatusCode.MethodNotAllowed.ToString(), message) { }
    }
}
