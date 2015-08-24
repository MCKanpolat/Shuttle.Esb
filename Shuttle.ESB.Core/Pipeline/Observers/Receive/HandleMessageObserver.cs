using System;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class HandleMessageObserver :
		IPipelineObserver<OnHandleMessage>
	{
		private readonly ILog _log;

		public HandleMessageObserver()
		{
			_log = Log.For(this);
		}

		private void InvokeHandler(IServiceBus bus, IMessageHandler handler, TransportMessage transportMessage,
			object message, PipelineEvent pipelineEvent, Type messageType)
		{
			var state = pipelineEvent.Pipeline.State;
			var contextType = typeof (HandlerContext<>).MakeGenericType(new[] {messageType});
			var method = handler.GetType().GetMethod("ProcessMessage", new[] {contextType});

			Guard.Against<ProcessMessageMethodMissingException>(method == null,
				string.Format(
					ESBResources.ProcessMessageMethodMissingException,
					handler.GetType().FullName,
					transportMessage.MessageType));

			if (_log.IsTraceEnabled)
			{
				_log.Trace(string.Format(ESBResources.TraceCorrelationIdReceived, transportMessage.CorrelationId));

				foreach (var header in transportMessage.Headers)
				{
					_log.Trace(string.Format(ESBResources.TraceTransportHeaderReceived, header.Key, header.Value));
				}

				_log.Trace(string.Format(ESBResources.MessageHandlerInvoke,
					transportMessage.MessageType,
					transportMessage.MessageId,
					handler.GetType().FullName));
			}

			try
			{
				var handlerContext = Activator.CreateInstance(contextType, new[]
				{
					bus, transportMessage, message, state.GetActiveState()
				});

				pipelineEvent.Pipeline.State.SetHandlerContext((IMessageSender) handlerContext);

				method.Invoke(handler, new[]
				{
					handlerContext
				});
			}
			catch (Exception ex)
			{
				var exception = ex.TrimLeading<TargetInvocationException>();

				bus.Events.OnHandlerException(
					this,
					new HandlerExceptionEventArgs(
						pipelineEvent,
						handler,
						transportMessage,
						message,
						state.GetWorkQueue(),
						state.GetErrorQueue(),
						exception));

				throw exception;
			}
		}

		public void Execute(OnHandleMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();
			var transportMessage = state.GetTransportMessage();

			if (bus.Configuration.HasIdempotenceService)
			{
				try
				{
					var processingStatus = bus.Configuration.IdempotenceService.ProcessingStatus(transportMessage);

					state.SetProcessingStatus(processingStatus);

					if (processingStatus == ProcessingStatus.Ignore)
					{
						_log.Trace(string.Format(ESBResources.TraceMessageIgnored, transportMessage.MessageType,
							transportMessage.MessageId));

						return;
					}

					if (processingStatus == ProcessingStatus.MessageHandled)
					{
						_log.Trace(string.Format(ESBResources.TraceMessageHandled, transportMessage.MessageType,
							transportMessage.MessageId));

						return;
					}

					_log.Trace(string.Format(ESBResources.TraceMessageAssigned, transportMessage.MessageType,
						transportMessage.MessageId));
				}
				catch (Exception ex)
				{
					bus.Configuration.IdempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
				}
			}

			var message = state.GetMessage();
			var handler = bus.Configuration.MessageHandlerFactory.GetHandler(message);

			state.SetMessageHandler(handler);

			if (handler == null)
			{
				bus.Events.OnMessageNotHandled(this,
					new MessageNotHandledEventArgs(
						pipelineEvent,
						state.GetWorkQueue(),
						state.GetErrorQueue(),
						transportMessage,
						message));

				if (!bus.Configuration.RemoveMessagesNotHandled)
				{
					var error = string.Format(ESBResources.MessageNotHandledFailure, message.GetType().FullName,
						transportMessage.MessageId, state.GetErrorQueue().Uri.Secured());

					_log.Error(error);

					transportMessage.RegisterFailure(error);

					using (var stream = bus.Configuration.Serializer.Serialize(transportMessage))
					{
						state.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
					}
				}
				else
				{
					_log.Warning(string.Format(ESBResources.MessageNotHandledIgnored,
						message.GetType().FullName,
						transportMessage.MessageId));
				}
			}
			else
			{
				try
				{
					InvokeHandler(bus, handler, transportMessage, message, pipelineEvent, message.GetType());

					if (bus.Configuration.HasIdempotenceService)
					{
						bus.Configuration.IdempotenceService.MessageHandled(transportMessage);
					}
				}
				finally
				{
					bus.Configuration.MessageHandlerFactory.ReleaseHandler(handler);
				}
			}
		}
	}
}