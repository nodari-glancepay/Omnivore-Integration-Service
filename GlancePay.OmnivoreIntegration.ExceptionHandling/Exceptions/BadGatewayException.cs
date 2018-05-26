using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class BadGatewayException : ApiException
    {
        public BadGatewayException(string message) 
            : base(HttpStatusCode.BadGateway, HttpStatusCode.BadGateway.ToString(), message) { }    
    }
}
