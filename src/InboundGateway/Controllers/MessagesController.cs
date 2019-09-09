using Armsoft.Sandbox.InteractiveMessageBroker.Common;
using Armsoft.Sandbox.InteractiveMessageBroker.Common.ExternalMessages;
using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

        public MessagesController()
        {
            var cs = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var factory = MessagingFactory.CreateFromConnectionString(cs);
            _subscriptionManager = new MessageQueueSubscriptionManager(factory);
        }

        [HttpPost]
        [Route("a")]
        public async Task<IHttpActionResult> ReceiveMessageA(FirstResponseInbound message)
        {
            _logger.Info($"Received message (a): {JsonConvert.SerializeObject(message)}");
            var mapped = new FirstResponse
            {
                Balance = message.Balance,
                Id = Guid.Parse(message.SomeIdentifier)
            };
            const string queue = QueueNames.Inbound;
            _logger.Info($"Publishing notification to queue '{queue}'");
            await _subscriptionManager.Publish(queue, mapped);
            return Ok();
        }

        [HttpPost]
        [Route("b")]
        public async Task<IHttpActionResult> ReceiveMessageB(SecondResponseInbound message)
        {
            _logger.Info($"Received message (b): {JsonConvert.SerializeObject(message)}");
            var mapped = new SecondResponse
            {
                IsInCredit = message.IsInCredit,
                Id = Guid.Parse(message.SomeIdentifier)
            };
            const string queue = QueueNames.Inbound;
            _logger.Info($"Publishing notification to queue '{queue}'");
            await _subscriptionManager.Publish(queue, mapped);
            return Ok();
        }
    }
}