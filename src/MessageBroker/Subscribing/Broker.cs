using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Subscribing
{
    public class Broker : IBroker
    {
        private readonly ISubscriptionFactory _subscriptionFactory;
        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

        public Broker(IMessageQueueSubscriptionManager subscriptionManager, ISubscriptionFactory subscriptionFactory)
        {
            _subscriptionManager = subscriptionManager;
            _subscriptionFactory = subscriptionFactory;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            var subs = _subscriptionFactory.CreateAllAvailableSubscriptions();
            var tasks = new List<Task>();
            foreach (var messageSubscriber in subs)
            {
                _subscriptionManager.AddSubscriber(messageSubscriber);
                tasks.Add(_subscriptionManager.StartSubscriber(messageSubscriber.Id));
            }

            await Task.WhenAll(tasks);
        }

        public void Stop()
        {
            var activeSubscriptions = _subscriptionManager
                .ListSubscribers()
                .Where(sub => sub.State == MessageSubscriberState.Started);
            foreach (var messageSubscriberInfo in activeSubscriptions)
            {
                _subscriptionManager.StopSubscriber(messageSubscriberInfo.Id);
            }
        }
    }
}