using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message) 
            : base(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString(), message) { }    
    }
}
