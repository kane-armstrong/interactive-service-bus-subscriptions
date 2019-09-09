using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using System.Threading.Tasks;
using Console = System.Console;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerA
{
    public class Worker
    {
        public async Task DoWork()
        {
            var cs = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var factory = MessagingFactory.CreateFromConnectionString(cs);
            var namespaceManager = NamespaceManager.CreateFromConnectionString(cs);
            var subscriber = new ConsumerASubscriber(factory, namespaceManager);

            const string queue = QueueNames.InternalA;

            Console.WriteLine("========================");
            Console.WriteLine("Internal Consumer A");
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