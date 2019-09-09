namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public enum MessageSubscriberState
    {
        Started = 0,
        Starting = 1,
        Stopping = 2,
        Stopped = 3,
        Faulted = 4
    }
}