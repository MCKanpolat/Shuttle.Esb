﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public class DistributorPipeline : Pipeline
	{
		public DistributorPipeline(IServiceBus bus)
		{
            Guard.AgainstNull(bus, "bus");

            State.SetServiceBus(bus);

            State.SetWorkQueue(bus.Configuration.Inbox.WorkQueue);
            State.SetErrorQueue(bus.Configuration.Inbox.ErrorQueue);

            RegisterStage("Distribute")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnAfterDeserializeTransportMessage>()
				.WithEvent<OnHandleDistributeMessage>()
				.WithEvent<OnAfterHandleDistributeMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnAfterSerializeTransportMessage>()
				.WithEvent<OnDispatchTransportMessage>()
				.WithEvent<OnAfterDispatchTransportMessage>()
				.WithEvent<OnAcknowledgeMessage>()
				.WithEvent<OnAfterAcknowledgeMessage>();

			RegisterObserver(new GetWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DistributorMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new DispatchTransportMessageObserver());
			RegisterObserver(new AcknowledgeMessageObserver());

			RegisterObserver(new DistributorExceptionObserver()); // must be last
		}
    }
}