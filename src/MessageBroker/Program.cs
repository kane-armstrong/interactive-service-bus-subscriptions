using Armsoft.Sandbox.InteractiveMessageBroker.Hosting;
using Autofac;

namespace Armsoft.Sandbox.InteractiveMessageBroker
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