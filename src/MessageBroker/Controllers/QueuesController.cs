using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Controllers
{
    [RoutePrefix("api/messagequeues")]
    public class QueuesController : ApiController
    {
        private readonly IMessageQueueManager _messageQueueManager;
        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

        public QueuesController(IMessageQueueManager messageQueueManager)
        {
            _messageQueueManager = messageQueueManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> ListQueues()
        {
            return Ok(await _messageQueueManager.ListQueues());
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(CreateQueue request)
        {
            var queues = await _messageQueueManager.ListQueues();
            if (queues.Any(x => x.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return Conflict();
            }
            await _messageQueueManager.CreateQueueIfNotExists(request.Name);
            return Ok();
        }

        [HttpDelete]
        [Route("name")]
        public async Task<IHttpActionResult> Delete(string name)
        {
            var queues = await _messageQueueManager.ListQueues();
            if (!queues.Any(x => x.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return NotFound();
            }

            var subs = _subscriptionManager.ListSubscribers().Where(s => s.QueueName == name);
            foreach (var sub in subs)
            {
                _subscriptionManager.StopSubscriber(sub.Id);
            }

            await _messageQueueManager.DeleteQueueIfExists(name);
            foreach (var sub in subs)
            {
                _subscriptionManager.RemoveSubscriber(sub.Id);
            }
            return Ok();
        }

        [HttpPut]
        [Route("name/disable")]
        public async Task<IHttpActionResult> Stop(string name)
        {
            var queues = await _messageQueueManager.ListQueues();
            if (!queues.Any(x => x.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return NotFound();
            }

            var subs = _subscriptionManager.ListSubscribers().Where(s => s.QueueName == name);
            foreach (var sub in subs)
            {
                _subscriptionManager.StopSubscriber(sub.Id);
            }
            await _messageQueueManager.Disable(name);
            return Ok();
        }

        [HttpPut]
        [Route("name/enable")]
        public async Task<IHttpActionResult> Enable(string name)
        {
            var queues = await _messageQueueManager.ListQueues();
            if (!queues.Any(x => x.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return NotFound();
            }
            await _messageQueueManager.Enable(name);
            return Ok();
        }
    }
}