﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
    public class DistributorMessageObserver :
	    IPipelineObserver<OnHandleDistributeMessage>,
        IPipelineObserver<OnAbortPipeline>
    {
        public void Execute(OnHandleDistributeMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();

	        transportMessage.RecipientInboxWorkQueueUri = state.GetAvailableWorker().InboxWorkQueueUri;

			state.SetTransportMessage(transportMessage);
			state.SetTransportMessageReceived(null);
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;

            state.GetServiceBus().Configuration
				.WorkerAvailabilityManager.ReturnAvailableWorker(state.GetAvailableWorker());
        }
    }
}