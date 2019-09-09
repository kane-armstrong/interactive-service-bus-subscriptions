using NLog;

namespace Armsoft.Sandbox.InteractiveMessageBroker.InternalConsumerA
{
    public class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("Starting internal consumer A");
            var worker = new Worker();
            worker.DoWork().GetAwaiter().GetResult();
        }
    }
}