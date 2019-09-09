using NLog;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Hosting
{
    public class WindowsService : IWindowsService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebService _webService;

        public WindowsService(IWebService webService)
        {
            _webService = webService;
        }

        public void Start()
        {
            _logger.Debug("Starting application");
            _webService.Start();
            _logger.Trace("Application started");
        }

        public void Stop()
        {
            _logger.Debug("Stopping application");
            _webService.Stop();
            _webService.Dispose();
            _logger.Trace("Application stopped");
        }
    }
}