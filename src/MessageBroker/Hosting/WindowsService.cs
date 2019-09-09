using Armsoft.Sandbox.InteractiveMessageBroker.Subscribing;
using NLog;
using System.Threading;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Hosting
{
    public class WindowsService : IWindowsService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebService _webService;
        private readonly IBroker _broker;

        public WindowsService(IWebService webService, IBroker broker)
        {
            _webService = webService;
            _broker = broker;
        }

        public void Start()
        {
            _logger.Debug("Starting application");
            _webService.Start();
            _broker.Start(CancellationToken.None);
            _logger.Trace("Application started");
        }

        public void Stop()
        {
            _logger.Debug("Stopping application");
            _webService.Stop();
            _webService.Dispose();
            _broker.Stop();
            _logger.Trace("Application stopped");
        }
    }
}