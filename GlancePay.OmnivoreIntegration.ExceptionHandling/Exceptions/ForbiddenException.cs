using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message) 
            : base(HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString(), message) { }    
    }
}
