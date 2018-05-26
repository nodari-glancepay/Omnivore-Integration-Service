using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class InternalServerErrorException : ApiException
    {
        public InternalServerErrorException(string message)
        : base(HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(), message) { }
    }
}
