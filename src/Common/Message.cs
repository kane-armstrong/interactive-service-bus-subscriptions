using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public abstract class Message
    {
        public Guid Id { get; set; }
        public abstract MessageType MessageType { get; }
    }
}