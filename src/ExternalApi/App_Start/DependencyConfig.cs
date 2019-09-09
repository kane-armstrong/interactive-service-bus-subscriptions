using Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Hosting;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Configuration;
using System.Reflection;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer
{
    public static class DependencyConfig
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    Initialize();
                }
                return _container;
            }
        }

        private static void Initialize()
        {
            try
            {
                var builder = new ContainerBuilder();

                RegisterHostDependencies(builder);

                _container = builder.Build();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private const string WebAddressKey = "Web.SelfHostAddress";

        private static void RegisterHostDependencies(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var webAddress = ConfigurationManager.AppSettings[WebAddressKey];
            builder.RegisterType<WebService>().As<IWebService>()
                .WithParameter("address", webAddress)
                .SingleInstance();
            builder.RegisterType<WindowsService>().As<IWindowsService>().SingleInstance();
            builder.RegisterType<WindowsServiceHost>().As<IWindowsServiceHost>().SingleInstance();
        }
    }
}