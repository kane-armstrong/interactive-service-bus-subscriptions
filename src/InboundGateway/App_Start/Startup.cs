using Owin;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseWebApi(WebApiConfig.Register());
        }
    }
}