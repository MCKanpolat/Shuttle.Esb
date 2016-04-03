using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
    public class GetDeferredMessageObserver : IPipelineObserver<OnGetMessage>
    {
		private readonly ILog _log;

		public GetDeferredMessageObserver()
		{
			_log = Log.For(this);
		}

        public void Execute(OnGetMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
			var queue = state.GetDeferredQueue();

			Guard.AgainstNull(queue, "deferredQueue");

            var receivedMessage = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (receivedMessage == null)
            {
				state.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				state.SetWorking();
				state.SetReceivedMessage(receivedMessage);
            }
        }
    }
}