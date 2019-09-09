using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.MessageHandling
{
    public interface IInboundMessageHandler
    {
        Task HandleMessageA(FirstResponse message);

        Task HandleMessageB(SecondResponse message);
    }
}