using Owin;

namespace Armsoft.Sandbox.InteractiveMessageBroker
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseWebApi(WebApiConfig.Register());
        }
    }
}