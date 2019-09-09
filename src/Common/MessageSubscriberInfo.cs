using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public class MessageSubscriberInfo
    {
        public Guid Id { get; set; }
        public string QueueName { get; set; }
        public MessageSubscriberState State { get; set; }
    }
}