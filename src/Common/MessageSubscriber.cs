using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public abstract class MessageSubscriber : IDisposable
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public Guid Id { get; }
        public string QueueName { get; }
        protected abstract Func<BrokeredMessage, Task> Callback { get; }
        public MessageSubscriberState State { get; private set; } = MessageSubscriberState.Stopped;

        private TaskCompletionSource<int> _taskCompletionSource;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly MessagingFactory _messagingFactory;
        private readonly NamespaceManager _namespaceManager;

        private bool _disposed = false;

        protected MessageSubscriber(Guid id, string queueName, MessagingFactory messagingFactory, NamespaceManager namespaceManager)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }
            Id = id;

            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentException(nameof(queueName));
            }
            QueueName = queueName;

            _messagingFactory = messagingFactory;
            _namespaceManager = namespaceManager;
        }

        private async Task VerifyQueueExists()
        {
            if (!await _namespaceManager.QueueExistsAsync(QueueName))
            {
                State = MessageSubscriberState.Faulted;
                throw new InvalidOperationException($"Unable to subscribe to the queue '{QueueName}' as it does not exist in the current namespace.");
            }
        }

        private async Task<MessageReceiver> TryCreateRecevier()
        {
            try
            {
                var receiver = await _messagingFactory.CreateMessageReceiverAsync(QueueName, ReceiveMode.PeekLock);
                receiver.RetryPolicy = new RetryExponential(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), 5);
                return receiver;
            }
            catch (Exception)
            {
                State = MessageSubscriberState.Faulted;
                throw;
            }
        }

        public async Task StartSubscription()
        {
            if (State == MessageSubscriberState.Started)
            {
                return;
            }

            State = MessageSubscriberState.Starting;

            await VerifyQueueExists();

            _taskCompletionSource = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

            _cancellationTokenSource = new CancellationTokenSource();

            var receiver = await TryCreateRecevier();

            State = MessageSubscriberState.Started;

            _cancellationTokenSource.Token.Register(async () =>
            {
                try
                {
                    await receiver.CloseAsync();
                    _logger.Info($"Subscription {Id} stopped receiving messages for queue: {QueueName}");
                    _taskCompletionSource.SetResult(0);
                }
                catch (MessagingEntityNotFoundException)
                {
                    // Queue was deleted before shutting the subscription down
                    State = MessageSubscriberState.Stopped;
                    _taskCompletionSource.SetResult(1);
                }
                catch (Exception e)
                {
                    State = MessageSubscriberState.Faulted;
                    _taskCompletionSource.SetException(e);
                }
            });
            receiver.OnMessageAsync(async message =>
            {
                try
                {
                    _logger.Debug($"Subscription {Id} received a new message from queue: {QueueName}");
                    await Callback(message);
                    await message.CompleteAsync();
                }
                catch (TaskCanceledException)
                {
                    State = MessageSubscriberState.Stopped;
                }
                catch (Exception e)
                {
                    await message.AbandonAsync();
                    _logger.Error(e, $"Subscription {Id} encountered an error while handling message {message.MessageId}: {e.Message}");
                }
            }, new OnMessageOptions { AutoComplete = false, MaxConcurrentCalls = 1 });
            await _taskCompletionSource.Task;
        }

        public void StopSubscription()
        {
            State = MessageSubscriberState.Stopping;
            try
            {
                _cancellationTokenSource.Cancel(true);
                State = MessageSubscriberState.Stopped;
                _logger.Info($"Subscription {Id} for queue {QueueName} shut down successfully.");
            }
            catch (TaskCanceledException)
            {
                State = MessageSubscriberState.Stopped;
            }
            catch (Exception e)
            {
                State = MessageSubscriberState.Faulted;
                _logger.Error(e, $"Subscription {Id} for queue {QueueName} encountered an error while shutting down: {e.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _cancellationTokenSource.Dispose();
            _disposed = true;
        }
    }
}