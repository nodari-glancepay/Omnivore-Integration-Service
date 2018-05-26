using System;
using System.Net;

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    [Serializable]
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) 
            : base(HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), message) { }    
    }
}
