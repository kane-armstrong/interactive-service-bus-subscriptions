using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages
{
    public class FirstResponse : Message
    {
        public Guid SomeIdentifier { get; set; }
        public decimal Balance { get; set; }
        public override MessageType MessageType => MessageType.A;
    }
}