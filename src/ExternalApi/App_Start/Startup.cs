using Owin;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseWebApi(WebApiConfig.Register());
        }
    }
}