using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerA
{
    public class ConsumerASubscriber : MessageSubscriber
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConsumerASubscriber(MessagingFactory messagingFactory, NamespaceManager namespaceManager)
            : base(Guid.Parse("c9a93504-a420-480a-802d-87d76217b886"), QueueNames.InternalA, messagingFactory, namespaceManager)
        {
        }

        protected override Func<BrokeredMessage, Task> Callback => async m =>
        {
            using (var sr = new StreamReader(m.GetBody<Stream>(), Encoding.UTF8))
            {
                var content = await sr.ReadToEndAsync();
                _logger.Info($"Internal consumer A received message: {content}");
            }
        };
    }
}