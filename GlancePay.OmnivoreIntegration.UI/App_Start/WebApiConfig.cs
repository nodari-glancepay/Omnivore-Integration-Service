using System.Web.Http;

using GlancePay.OmnivoreIntegration.UI.Filters;

namespace GlancePay.OmnivoreIntegration.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Add Exception filter
            config.Filters.Add(new ApiExceptionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
