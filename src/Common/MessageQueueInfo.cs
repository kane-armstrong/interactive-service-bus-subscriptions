using Microsoft.ServiceBus.Messaging;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public class MessageQueueInfo
    {
        public string Name { get; set; }
        public EntityStatus Stats { get; set; }
        public MessageCountDetails MessageCounts { get; set; }
    }
}