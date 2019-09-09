using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public interface IMessageQueueSubscriptionManager
    {
        Task Publish<T>(string queueName, T message);

        IList<MessageSubscriberInfo> ListSubscribers();

        MessageSubscriberInfo GetSubscriber(Guid id);

        void AddSubscriber(MessageSubscriber subscriber);

        void RemoveSubscriber(Guid id);

        Task StartSubscriber(Guid id);

        void StopSubscriber(Guid id);
    }
}