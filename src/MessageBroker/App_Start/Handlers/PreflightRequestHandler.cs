using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Handlers
{
    public class PreflightsRequestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Origin") || request.Method.Method != "OPTIONS")
            {
                return base.SendAsync(request, cancellationToken);
            }

            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Authorization");
            response.Headers.Add("Access-Control-Allow-Methods", "*");
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}