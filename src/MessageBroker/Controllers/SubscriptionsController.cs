using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Controllers
{
    [RoutePrefix("api/subscriptions")]
    public class SubscriptionsController : ApiController
    {
        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

        public SubscriptionsController(IMessageQueueSubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult ListSubscriptions()
        {
            return Ok(_subscriptionManager.ListSubscribers());
        }

        [HttpPut]
        [Route("{id}/start")]
        public async Task<IHttpActionResult> Start(Guid id)
        {
            await _subscriptionManager.StartSubscriber(id);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/stop")]
        public IHttpActionResult Stop(Guid id)
        {
            _subscriptionManager.StopSubscriber(id);
            return Ok();
        }
    }
}