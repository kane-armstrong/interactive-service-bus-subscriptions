using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public class MessageQueueManager : IMessageQueueManager
    {
        private readonly NamespaceManager _namespaceManager;

        public MessageQueueManager(NamespaceManager namespaceManager)
        {
            _namespaceManager = namespaceManager;
        }

        public async Task<List<string>> ListQueues()
        {
            var queues = await _namespaceManager.GetQueuesAsync();
            return queues.Select(x => x.Path).ToList();
        }

        public async Task CreateQueueIfNotExists(string queueName)
        {
            if (!_namespaceManager.QueueExists(queueName))
            {
                await _namespaceManager.CreateQueueAsync(queueName);
            }
        }

        public async Task DeleteQueueIfExists(string queueName)
        {
            if (await _namespaceManager.QueueExistsAsync(queueName))
            {
                await _namespaceManager.DeleteQueueAsync(queueName);
            }
        }

        public async Task Disable(string queueName)
        {
            var queue = await _namespaceManager.GetQueueAsync(queueName);
            if (queue.Status == EntityStatus.Disabled)
            {
                return;
            }
            queue.Status = EntityStatus.Disabled;
            await _namespaceManager.UpdateQueueAsync(queue);
        }

        public async Task Enable(string queueName)
        {
            var queue = await _namespaceManager.GetQueueAsync(queueName);
            if (queue.Status == EntityStatus.Active)
            {
                return;
            }
            queue.Status = EntityStatus.Active;
            await _namespaceManager.UpdateQueueAsync(queue);
        }
    }
}