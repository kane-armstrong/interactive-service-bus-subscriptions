using Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway.Hosting;
using Autofac;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = DependencyConfig.Container.Resolve<IWindowsServiceHost>();

            host.StartHost();
        }
    }
}