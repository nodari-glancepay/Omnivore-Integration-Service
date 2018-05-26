using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) 
            : base(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), message) { }    
    }
}
