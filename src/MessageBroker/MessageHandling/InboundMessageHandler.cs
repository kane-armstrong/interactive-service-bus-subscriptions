using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using NLog;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.MessageHandling
{
    public class InboundMessageHandler : IInboundMessageHandler
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

        public InboundMessageHandler(IMessageQueueSubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        public async Task HandleMessageA(FirstResponse message)
        {
            _logger.Info($"Forwarding request (type: {message.GetType().Name})");
            await _subscriptionManager.Publish(QueueNames.InternalA, message);
        }

        public async Task HandleMessageB(SecondResponse message)
        {
            _logger.Info($"Forwarding request (type: {message.GetType().Name})");
            await _subscriptionManager.Publish(QueueNames.InternalB, message);
        }
    }
}