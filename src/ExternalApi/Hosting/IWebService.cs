using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Hosting
{
    public interface IWebService : IDisposable
    {
        void Start();

        void Stop();
    }
}