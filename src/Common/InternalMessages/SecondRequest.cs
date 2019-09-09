using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages
{
    public class SecondRequest : Message
    {
        public Guid SomeIdentifier { get; set; }
        public override MessageType MessageType => MessageType.Y;
    }
}