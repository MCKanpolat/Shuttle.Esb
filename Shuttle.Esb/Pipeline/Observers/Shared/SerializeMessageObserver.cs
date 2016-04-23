﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public class SerializeMessageObserver : IPipelineObserver<OnSerializeMessage>
	{
		public void Execute(OnSerializeMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var message = state.GetMessage();
			var transportMessage = state.GetTransportMessage();
			var bytes = state.GetServiceBus()
				.Configuration.Serializer
				.Serialize(message).ToBytes();

			state.SetMessageBytes(bytes);

			transportMessage.Message = bytes;
		}
	}
}