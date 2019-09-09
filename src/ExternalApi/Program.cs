using Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Hosting;
using Autofac;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer
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