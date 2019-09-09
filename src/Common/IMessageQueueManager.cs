using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public interface IMessageQueueManager
    {
        Task<List<string>> ListQueues();

        Task CreateQueueIfNotExists(string queueName);

        Task DeleteQueueIfExists(string queueName);

        Task Disable(string queueName);

        Task Enable(string queueName);
    }
}