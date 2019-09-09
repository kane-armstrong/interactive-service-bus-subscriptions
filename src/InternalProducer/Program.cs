using NLog;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalProducer
{
    public class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("Starting producer");
            var worker = new Worker();
            worker.DoWork().GetAwaiter().GetResult();
        }
    }
}