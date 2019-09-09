using NLog;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerB
{
    public class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("Starting consumer A");
            var worker = new Worker();
            worker.DoWork().GetAwaiter().GetResult();
        }
    }
}