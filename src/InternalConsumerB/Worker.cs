using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NLog;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerB
{
    public class Worker
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private Func<BrokeredMessage, Task> OnReceivedAsync => async m =>
        {
            using (var sr = new StreamReader(m.GetBody<Stream>(), Encoding.UTF8))
            {
                var content = await sr.ReadToEndAsync();
                _logger.Info($"Internal consumer B received message: {content}");
            }
        };

        public async Task DoWork()
        {
            var cs = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var factory = MessagingFactory.CreateFromConnectionString(cs);
            var namespaceManager = NamespaceManager.CreateFromConnectionString(cs);
            var subscriber = new ConsumerBSubscriber(factory, namespaceManager);

            const string queue = QueueNames.InternalB;

            Console.WriteLine("========================");
            Console.WriteLine("Internal Consumer B");
            Console.WriteLine("========================");
            Console.WriteLine("Options:");
            Console.WriteLine($"1. Subscribe to queue '{queue}'");
            Console.WriteLine("2. Exit");
            Console.WriteLine("========================");

            while (true)
            {
                Console.Write("Selection: ");
                var input = Console.ReadLine();
                if (input == "1")
                {
                    Console.WriteLine($"Subscribing to '{queue}' notifications");
                    await subscriber.StartSubscription();
                }
                else if (input == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unrecognized input!");
                }
            }
        }
    }
}