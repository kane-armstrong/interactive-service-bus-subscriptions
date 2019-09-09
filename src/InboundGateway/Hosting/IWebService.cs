using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway.Hosting
{
    public interface IWebService : IDisposable
    {
        void Start();

        void Stop();
    }
}