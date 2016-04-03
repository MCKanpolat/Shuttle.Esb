using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public static class PipelineStateExtensions
	{
		public static IServiceBus GetServiceBus(this State<Pipeline> state)
		{
			return state.Get<IServiceBus>();
		}

		public static IThreadState GetActiveState(this State<Pipeline> state)
		{
			return state.Get<IThreadState>(StateKeys.ActiveState);
		}

		public static IQueue GetWorkQueue(this State<Pipeline> state)
		{
			return state.Get<IQueue>(StateKeys.WorkQueue);
		}

		public static IQueue GetDeferredQueue(this State<Pipeline> state)
		{
			return state.Get<IQueue>(StateKeys.DeferredQueue);
		}

		public static IQueue GetErrorQueue(this State<Pipeline> state)
		{
			return state.Get<IQueue>(StateKeys.ErrorQueue);
		}

		public static int GetMaximumFailureCount(this State<Pipeline> state)
		{
			return state.Get<int>(StateKeys.MaximumFailureCount);
		}

		public static TimeSpan[] GetDurationToIgnoreOnFailure(this State<Pipeline> state)
		{
			return state.Get<TimeSpan[]>(StateKeys.DurationToIgnoreOnFailure);
		}

		public static void SetTransportMessage(this State<Pipeline> state, TransportMessage value)
		{
			state.Replace(StateKeys.TransportMessage, value);
		}

		public static TransportMessage GetTransportMessage(this State<Pipeline> state)
		{
			return state.Get<TransportMessage>(StateKeys.TransportMessage);
		}

		public static void SetTransportMessageReceived(this State<Pipeline> state, TransportMessage value)
		{
			state.Replace(StateKeys.TransportMessageReceived, value);
		}

		public static TransportMessage GetTransportMessageReceived(this State<Pipeline> state)
		{
			return state.Get<TransportMessage>(StateKeys.TransportMessageReceived);
		}

		public static void SetProcessingStatus(this State<Pipeline> state, ProcessingStatus processingStatus)
		{
			state.Replace(StateKeys.ProcessingStatus, processingStatus);
		}

		public static ProcessingStatus GetProcessingStatus(this State<Pipeline> state)
		{
			return state.Get<ProcessingStatus>(StateKeys.ProcessingStatus);
		}

		public static void SetReceivedMessage(this State<Pipeline> state, ReceivedMessage receivedMessage)
		{
			state.Replace(StateKeys.ReceivedMessage, receivedMessage);
		}

		public static ReceivedMessage GetReceivedMessage(this State<Pipeline> state)
		{
			return state.Get<ReceivedMessage>(StateKeys.ReceivedMessage);
		}

		public static void SetTransportMessageStream(this State<Pipeline> state, Stream value)
		{
			state.Replace(StateKeys.TransportMessageStream, value);
		}

		public static Stream GetTransportMessageStream(this State<Pipeline> state)
		{
			return state.Get<Stream>(StateKeys.TransportMessageStream);
		}

		public static byte[] GetMessageBytes(this State<Pipeline> state)
		{
			return state.Get<byte[]>(StateKeys.MessageBytes);
		}

		public static void SetMessageBytes(this State<Pipeline> state, byte[] bytes)
		{
			state.Replace(StateKeys.MessageBytes, bytes);
		}

		public static object GetMessage(this State<Pipeline> state)
		{
			return state.Get<object>(StateKeys.Message);
		}

		public static void SetMessage(this State<Pipeline> state, object message)
		{
			state.Replace(StateKeys.Message, message);
		}

		public static void SetTransportMessageContext(this State<Pipeline> state, TransportMessageConfigurator configurator)
		{
			state.Replace(StateKeys.TransportMessageConfigurator, configurator);
		}

		public static void SetAvailableWorker(this State<Pipeline> state, AvailableWorker value)
		{
			state.Replace(StateKeys.AvailableWorker, value);
		}

		public static AvailableWorker GetAvailableWorker(this State<Pipeline> state)
		{
			return state.Get<AvailableWorker>(StateKeys.AvailableWorker);
		}

		public static bool GetTransactionComplete(this State<Pipeline> state)
		{
			return state.Get<bool>(StateKeys.TransactionComplete);
		}

		public static void SetTransactionComplete(this State<Pipeline> state)
		{
			state.Replace(StateKeys.TransactionComplete, true);
		}

		public static void SetWorking(this State<Pipeline> state)
		{
			state.Replace(StateKeys.Working, true);
		}

		public static bool GetWorking(this State<Pipeline> state)
		{
			return state.Get<bool>(StateKeys.Working);
		}

		public static void ResetWorking(this State<Pipeline> state)
		{
			state.Replace(StateKeys.Working, false);
		}

		public static ITransactionScope GetTransactionScope(this State<Pipeline> state)
		{
			return state.Get<ITransactionScope>(StateKeys.TransactionScope);
		}

		public static void SetTransactionScope(this State<Pipeline> state, ITransactionScope scope)
		{
			state.Replace(StateKeys.TransactionScope, scope);
		}

		public static void SetMessageHandler(this State<Pipeline> state, object handler)
		{
			state.Replace(StateKeys.MessageHandler, handler);
		}

		public static object GetMessageHandler(this State<Pipeline> state)
		{
			return state.Get<IMessageHandler>(StateKeys.MessageHandler);
		}

		public static void SetWorkQueue(this State<Pipeline> state, IQueue queue)
		{
			state.Add(StateKeys.WorkQueue, queue);
		}

		public static void SetDeferredQueue(this State<Pipeline> state, IQueue queue)
		{
			state.Add(StateKeys.DeferredQueue, queue);
		}

		public static void SetErrorQueue(this State<Pipeline> state, IQueue queue)
		{
			state.Add(StateKeys.ErrorQueue, queue);
		}

		public static void SetMaximumFailureCount(this State<Pipeline> state, int count)
		{
			state.Add(StateKeys.MaximumFailureCount, count);
		}

		public static void SetDurationToIgnoreOnFailure(this State<Pipeline> state, TimeSpan[] timeSpans)
		{
			state.Add(StateKeys.DurationToIgnoreOnFailure, timeSpans);
		}

		public static void SetActiveState(this State<Pipeline> state, IThreadState activeState)
		{
			state.Add(StateKeys.ActiveState, activeState);
		}

		public static IMessageSender GetHandlerContext(this State<Pipeline> state)
		{
			return state.Get<IMessageSender>(StateKeys.HandlerContext);
		}

		public static void SetHandlerContext(this State<Pipeline> state, IMessageSender handlerContext)
		{
			state.Replace(StateKeys.HandlerContext, handlerContext);
		}
	}
}