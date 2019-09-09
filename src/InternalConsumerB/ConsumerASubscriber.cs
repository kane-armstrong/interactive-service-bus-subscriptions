using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerB
{
    public class ConsumerBSubscriber : MessageSubscriber
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConsumerBSubscriber(MessagingFactory messagingFactory, NamespaceManager namespaceManager)
            : base(Guid.Parse("94a5c530-b83d-40b9-b527-816e2d80a477"), QueueNames.InternalB, messagingFactory, namespaceManager)
        {
        }

        protected override Func<BrokeredMessage, Task> Callback => async m =>
        {
            using (var sr = new StreamReader(m.GetBody<Stream>(), Encoding.UTF8))
            {
                var content = await sr.ReadToEndAsync();
                _logger.Info($"Internal consumer B received message: {content}");
            }
        };
    }
}