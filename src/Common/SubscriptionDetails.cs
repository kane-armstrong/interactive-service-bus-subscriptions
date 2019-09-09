using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public class SubscriptionDetails
    {
        public Guid Id { get; set; }
        public string QueueName { get; set; }
    }
}