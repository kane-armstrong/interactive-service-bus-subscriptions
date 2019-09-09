using System.Threading;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Subscribing
{
    public interface IBroker
    {
        Task Start(CancellationToken cancellationToken);

        void Stop();
    }
}