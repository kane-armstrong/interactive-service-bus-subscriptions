using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armsoft.Sandbox.InteractiveMessageBroker.Common
{
    public class MessageQueueSubscriptionManager : IMessageQueueSubscriptionManager, IDisposable
    {
        private readonly MessagingFactory _messagingFactory;
        private readonly ConcurrentDictionary<string, MessageSender> _messageSenders;
        private readonly IList<MessageSubscriber> _subscribers;

        public MessageQueueSubscriptionManager(MessagingFactory messagingFactory)
        {
            _messagingFactory = messagingFactory;
            _messageSenders = new ConcurrentDictionary<string, MessageSender>();
            _subscribers = new List<MessageSubscriber>();
        }

        public async Task Publish<T>(string queueName, T message)
        {
            var sender = _messageSenders.GetOrAdd(queueName, await _messagingFactory.CreateMessageSenderAsync(queueName));
            var brokeredMessage = new BrokeredMessage(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))))
            {
                ContentType = "application/json"
            };
            brokeredMessage.Properties.Add("messageType", message.GetType().Name);
            await sender.SendAsync(brokeredMessage);
        }

        public IList<MessageSubscriberInfo> ListSubscribers()
        {
            return _subscribers.Select(sub => new MessageSubscriberInfo
            {
                State = sub.State,
                Id = sub.Id,
                QueueName = sub.QueueName
            }).ToList();
        }

        public MessageSubscriberInfo GetSubscriber(Guid id)
        {
            var subscriber = _subscribers.FirstOrDefault(x => x.Id == id);
            if (subscriber == null)
            {
                return null;
            }
            return new MessageSubscriberInfo
            {
                State = subscriber.State,
                Id = subscriber.Id,
                QueueName = subscriber.QueueName
            };
        }

        public void AddSubscriber(MessageSubscriber subscriber)
        {
            if (_subscribers.Any(x => x.Id == subscriber.Id))
            {
                return;
            }

            _subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(Guid id)
        {
            var subscriber = _subscribers.First(x => x.Id == id);
            _subscribers.Remove(subscriber);
        }

        public async Task StartSubscriber(Guid id)
        {
            var subscriber = _subscribers.First(x => x.Id == id);
            await Task.Run(() => subscriber.StartSubscription());
        }

        public void StopSubscriber(Guid id)
        {
            var subscriber = _subscribers.First(x => x.Id == id);
            subscriber.StopSubscription();
        }

        public void Dispose()
        {
            if (!_messageSenders.IsEmpty)
            {
                foreach (var sender in _messageSenders.Values)
                {
                    if (!sender.IsClosed)
                    {
                        sender.Close();
                    }
                }
            }
        }
    }
}