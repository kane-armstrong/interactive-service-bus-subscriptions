using Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway.Handlers;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway
{
    public class WebApiConfig
    {
        public static HttpConfiguration Register()
        {
            var httpConfiguration = new HttpConfiguration();

            ConfigureCors(httpConfiguration);

            ConfigureFormatters(httpConfiguration);

            ConfigureRoutes(httpConfiguration);

            ConfigureSwagger(httpConfiguration);

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(DependencyConfig.Container);

            return httpConfiguration;
        }

        private static void ConfigureSwagger(HttpConfiguration httpConfiguration)
        {
            httpConfiguration
                .EnableSwagger(c => c.SingleApiVersion("v1", "External API consumer"))
                .EnableSwaggerUi();
        }

        private static void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MapHttpAttributeRoutes();

            httpConfiguration.Routes.MapHttpRoute(
                name: "swagger_root",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureFormatters(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            httpConfiguration.Formatters.Remove(httpConfiguration.Formatters.XmlFormatter);
        }

        private static void ConfigureCors(HttpConfiguration httpConfiguration)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            httpConfiguration.EnableCors(cors);
            httpConfiguration.MessageHandlers.Add(new PreflightsRequestHandler());
        }
    }
}