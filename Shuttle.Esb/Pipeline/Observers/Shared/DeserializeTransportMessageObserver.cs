﻿using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public class DeserializeTransportMessageObserver : IPipelineObserver<OnDeserializeTransportMessage>
	{
		private readonly ILog _log;

		public DeserializeTransportMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnDeserializeTransportMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var receivedMessage = state.GetReceivedMessage();
			var workQueue = state.GetWorkQueue();
			var errorQueue = state.GetErrorQueue();

			Guard.AgainstNull(receivedMessage, "receivedMessage");
			Guard.AgainstNull(workQueue, "workQueue");
			Guard.AgainstNull(errorQueue, "errorQueue");

			TransportMessage transportMessage;

			try
			{
				using (var stream = receivedMessage.Stream.Copy())
				{
					transportMessage =
						(TransportMessage) state.GetServiceBus().Configuration.Serializer.Deserialize(typeof (TransportMessage), stream);
				}
			}
			catch (Exception ex)
			{
				_log.Error(ex.ToString());
				_log.Error(string.Format(EsbResources.TransportMessageDeserializationException, workQueue.Uri, ex));

				pipelineEvent.Pipeline.Abort();

				state.GetServiceBus().Events.OnTransportMessageDeserializationException(this,
					new DeserializationExceptionEventArgs(
						pipelineEvent,
						workQueue,
						errorQueue,
						ex));

				return;
			}

			state.SetTransportMessage(transportMessage);
			state.SetMessageBytes(transportMessage.Message);

			transportMessage.AcceptInvariants();
		}
	}
}