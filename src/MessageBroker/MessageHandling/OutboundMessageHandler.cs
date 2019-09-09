using Armsoft.Sandbox.InteractiveMessageBroker.Common.ExternalMessages;
using Armsoft.Sandbox.InteractiveMessageBroker.Common.InternalMessages;
using NLog;
using System.Net.Http;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.MessageHandling
{
    public class OutboundMessageHandler : IOutboundMessageHandler
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public async Task HandleMessageA(FirstRequest message)
        {
            var outbound = new FirstRequestOutbound
            {
                SomeIdentifier = message.SomeIdentifier.ToString()
            };
            _logger.Info($"Forwarding request (type: {outbound.GetType().Name})");
            await _httpClient.PostAsJsonAsync("http://localhost:9022/api/messages/a", outbound);
        }

        public async Task HandleMessageB(SecondRequest message)
        {
            var outbound = new SecondRequestOutbound
            {
                SomeIdentifier = message.SomeIdentifier.ToString()
            };
            _logger.Info($"Forwarding request (type: {outbound.GetType().Name})");
            await _httpClient.PostAsJsonAsync("http://localhost:9022/api/messages/b", outbound);
        }
    }
}