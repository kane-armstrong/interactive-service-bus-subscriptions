using NLog;
using System.Configuration;
using Topshelf;
using Topshelf.Autofac;

namespace Armsoft.Sandbox.InteractiveMessageBroker.ExternalConsumer.Hosting
{
    public class WindowsServiceHost : IWindowsServiceHost
    {
        private const string ServiceNameKey = "WindowsService.Name";
        private const string ServiceDisplayNameKey = "WindowsService.DisplayName";
        private const string ServiceDescriptionKey = "WindowsService.Description";

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void StartHost()
        {
            var description = ConfigurationManager.AppSettings[ServiceDescriptionKey];
            var displayName = ConfigurationManager.AppSettings[ServiceDisplayNameKey];
            var serviceName = ConfigurationManager.AppSettings[ServiceNameKey];

            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(DependencyConfig.Container);

                x.Service<IWindowsService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.UseNLog();

                x.OnException(e =>
                {
                    _logger.Fatal(e, $"A fatal error has occurred, causing the application to stop: {e.Message}");
                });

                x.SetDescription(description);
                x.SetDisplayName(displayName);
                x.SetServiceName(serviceName);
            });
        }
    }
}