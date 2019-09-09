using Microsoft.Owin.Hosting;
using NLog;
using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InboundGateway.Hosting
{
    public class WebService : IWebService
    {
        private readonly string _address;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IDisposable _webApp;

        public WebService(string address)
        {
            _address = address;
        }

        /// <summary>
        /// Starts the web api
        /// </summary>
        public void Start()
        {
            _logger.Debug($"Starting web server on address: {_address}");

            _webApp = WebApp.Start<Startup>(_address);
        }

        /// <summary>
        /// Stops the web api
        /// </summary>
        public void Stop()
        {
            _logger.Debug("Web service stopped");
        }

        public void Dispose()
        {
            _webApp?.Dispose();
        }
    }
}