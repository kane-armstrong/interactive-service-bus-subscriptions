using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Subscribers;
using System.Collections.Generic;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Subscribing
{
    public class SubscriptionFactory : ISubscriptionFactory
    {
        private readonly InboundQueueSubscriber _inboundQueueSubscriber;
        private readonly OutboundQueueSubscriber _outboundQueueSubscriber;

        public SubscriptionFactory(
            InboundQueueSubscriber inboundQueueSubscriber,
            OutboundQueueSubscriber outboundQueueSubscriber)
        {
            _inboundQueueSubscriber = inboundQueueSubscriber;
            _outboundQueueSubscriber = outboundQueueSubscriber;
        }

        public IList<MessageSubscriber> CreateAllAvailableSubscriptions() => new List<MessageSubscriber>
        {
            _inboundQueueSubscriber,
            _outboundQueueSubscriber
        };
    }
}