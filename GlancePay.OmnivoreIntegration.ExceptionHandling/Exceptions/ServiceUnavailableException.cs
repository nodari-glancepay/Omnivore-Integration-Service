using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class ServiceUnavailableException : ApiException
    {
        public ServiceUnavailableException(string message) 
            : base(HttpStatusCode.ServiceUnavailable, HttpStatusCode.ServiceUnavailable.ToString(), message) { }    
    }
}
