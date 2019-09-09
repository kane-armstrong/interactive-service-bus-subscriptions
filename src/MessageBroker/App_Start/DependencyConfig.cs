using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Hosting;
using Armsoft.Sandbox.InteractiveMessageBroker.MessageHandling;
using Armsoft.Sandbox.InteractiveMessageBroker.Subscribers;
using Armsoft.Sandbox.InteractiveMessageBroker.Subscribing;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;
using System.Reflection;
using Broker = Armsoft.Sandbox.InteractiveMessageBroker.Subscribing.Broker;
using IBroker = Armsoft.Sandbox.InteractiveMessageBroker.Subscribing.IBroker;

namespace Armsoft.Sandbox.InteractiveMessageBroker
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

        private const string ServiceBusConnectionStringName = "AzureServiceBus";

        private static void Initialize()
        {
            try
            {
                var builder = new ContainerBuilder();

                var cs = ConfigurationManager.ConnectionStrings[ServiceBusConnectionStringName].ConnectionString;
                builder
                    .RegisterType<MessageQueueManager>()
                    .As<IMessageQueueManager>()
                    .WithParameter("connectionString", cs)
                    .SingleInstance();

                builder
                    .RegisterType<MessageQueueSubscriptionManager>()
                    .As<IMessageQueueSubscriptionManager>()
                    .SingleInstance();

                builder.RegisterType<InboundQueueSubscriber>().AsSelf();
                builder.RegisterType<OutboundQueueSubscriber>().AsSelf();

                builder
                    .Register(c => MessagingFactory.CreateFromConnectionString(cs))
                    .As<MessagingFactory>()
                    .SingleInstance();

                builder
                    .Register(c => NamespaceManager.CreateFromConnectionString(cs))
                    .As<NamespaceManager>()
                    .SingleInstance();

                builder.RegisterType<InboundMessageHandler>().As<IInboundMessageHandler>();
                builder.RegisterType<OutboundMessageHandler>().As<IOutboundMessageHandler>();
                builder.RegisterType<SubscriptionFactory>().As<ISubscriptionFactory>();
                builder.RegisterType<Broker>().As<IBroker>();

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