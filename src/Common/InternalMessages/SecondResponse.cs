using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages
{
    public class SecondResponse : Message
    {
        public Guid SomeIdentifier { get; set; }
        public bool IsInCredit { get; set; }
        public override MessageType MessageType => MessageType.B;
    }
}