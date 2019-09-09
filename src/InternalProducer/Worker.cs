using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using Microsoft.ServiceBus.Messaging;
using NLog;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalProducer
{
    public class Worker
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private static readonly FirstRequest RequestA = new FirstRequest
        {
            Id = Guid.NewGuid(),
            SomeIdentifier = Guid.NewGuid()
        };

        private static readonly SecondRequest RequestB = new SecondRequest
        {
            Id = Guid.NewGuid(),
            SomeIdentifier = Guid.NewGuid()
        };

        public async Task DoWork()
        {
            var cs = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var factory = MessagingFactory.CreateFromConnectionString(cs);
            var manager = new MessageQueueSubscriptionManager(factory);

            const string queue = QueueNames.Outbound;

            try
            {
                Console.WriteLine("========================");
                Console.WriteLine("Producer (simulates requests from consumers)");
                Console.WriteLine("========================");
                Console.WriteLine("Options:");
                Console.WriteLine("1. Send request A");
                Console.WriteLine("2. Send request B");
                Console.WriteLine("3. Exit");
                Console.WriteLine("========================");

                while (true)
                {
                    Console.Write("Selection: ");
                    var input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.WriteLine($"Publishing notification to queue '{queue}'");
                        await manager.Publish(queue, RequestA);
                    }
                    else if (input == "2")
                    {
                        Console.WriteLine($"Publishing notification to queue '{queue}'");
                        await manager.Publish(queue, RequestB);
                    }
                    else if (input == "3")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized input!");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Something happened :(");
            }
        }
    }
}