using Microsoft.Owin.Hosting;
using NLog;
using System;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Hosting
{
    public class WebService : IWebService, IDisposable
    {
        private readonly string _address;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IDisposable _webApp;

        private bool _disposed = false;

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
            Dispose();
            _logger.Debug("Web service stopped");
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _webApp?.Dispose();
        }
    }
}