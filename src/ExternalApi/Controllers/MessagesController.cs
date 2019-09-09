using Armsoft.Sandbox.InteractiveMessageBroker.Common.ExternalMessages;
using Newtonsoft.Json;
using NLog;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly HttpClient _httpClient = new HttpClient();

        [HttpPost]
        [Route("a")]
        public async Task<IHttpActionResult> ReceiveMessageA(FirstRequestOutbound request)
        {
            _logger.Info($"Received message (a): {JsonConvert.SerializeObject(request)}");
            var response = new FirstResponseInbound
            {
                Balance = 0.25m,
                SomeIdentifier = request.SomeIdentifier
            };
            const string url = "http://localhost:9023/api/messages/a";
            _logger.Info($"Sending response - POST {url}");
            await _httpClient.PostAsJsonAsync(url, response);

            return Ok();
        }

        [HttpPost]
        [Route("b")]
        public async Task<IHttpActionResult> ReceiveMessageB(SecondRequestOutbound request)
        {
            _logger.Info($"Received message (b): {JsonConvert.SerializeObject(request)}");
            var response = new SecondResponseInbound
            {
                IsInCredit = true,
                SomeIdentifier = request.SomeIdentifier
            };
            const string url = "http://localhost:9023/api/messages/b";
            _logger.Info($"Sending response - POST {url}");

            await _httpClient.PostAsJsonAsync(url, response);

            return Ok();
        }
    }
}