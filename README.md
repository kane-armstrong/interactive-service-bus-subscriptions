# Interactive Service Bus Subscriptions

This thing allows you to turn on/off predefined handlers that subscribe to messages on a queue. Pretty messy and naive 
in this state, quick prototype before shifting to a private repository and doing a better job of it (that fork isn't mine)

Check QueueNames.cs for a list of endpoints to setup in Azure Service Bus before running the application. 
You will also need to set the 'AzureServiceBus' connection string in four of the app settings files.

Set every project as a startup project except Common, then f5. Start listening on the consumers when 
ready, then press use the producer to fire off messages.