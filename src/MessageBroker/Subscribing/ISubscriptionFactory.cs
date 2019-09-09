using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using System.Collections.Generic;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Subscribing
{
    public interface ISubscriptionFactory
    {
        IList<MessageSubscriber> CreateAllAvailableSubscriptions();
    }
}