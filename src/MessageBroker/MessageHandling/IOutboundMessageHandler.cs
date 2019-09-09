using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.MessageHandling
{
    public interface IOutboundMessageHandler
    {
        Task HandleMessageA(FirstRequest message);

        Task HandleMessageB(SecondRequest message);
    }
}