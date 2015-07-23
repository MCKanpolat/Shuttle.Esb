using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class StartupPipeline : Pipeline
	{
		public StartupPipeline(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			State.Add(bus);

			RegisterStage("Registration")
				.WithEvent<OnRegisterSharedConfiguration>()
				.WithEvent<OnAfterRegisterSharedConfiguration>()
				.WithEvent<OnRegisterControlInboxQueueConfiguration>()
				.WithEvent<OnAfterRegisterControlInboxQueueConfiguration>()
				.WithEvent<OnRegisterInboxQueueConfiguration>()
				.WithEvent<OnAfterRegisterInboxQueueConfiguration>()
				.WithEvent<OnRegisterOutboxQueueConfiguration>()
				.WithEvent<OnAfterRegisterOutboxQueueConfiguration>()
				.WithEvent<OnRegisterWorkerConfiguration>()
				.WithEvent<OnAfterRegisterWorkerConfiguration>()
				.WithEvent<OnRegisterModuleConfiguration>()
				.WithEvent<OnAfterRegisterModuleConfiguration>();

			RegisterStage("Initializing")
				.WithEvent<OnInitializeQueueFactories>()
				.WithEvent<OnAfterInitializeQueueFactories>()
				.WithEvent<OnCreateQueues>()
				.WithEvent<OnAfterCreateQueues>()
				.WithEvent<OnInitializeMessageHandlerFactory>()
				.WithEvent<OnAfterInitializeMessageHandlerFactory>()
				.WithEvent<OnInitializeMessageRouteProvider>()
				.WithEvent<OnAfterInitializeMessageRouteProvider>()
				.WithEvent<OnInitializePipelineFactory>()
				.WithEvent<OnAfterInitializePipelineFactory>()
				.WithEvent<OnInitializeSubscriptionManager>()
				.WithEvent<OnAfterInitializeSubscriptionManager>()
				.WithEvent<OnInitializeIdempotenceService>()
				.WithEvent<OnAfterInitializeIdempotenceService>()
				.WithEvent<OnInitializeTransactionScopeFactory>()
				.WithEvent<OnAfterInitializeTransactionScopeFactory>();

			RegisterStage("Start")
				.WithEvent<OnStartInboxProcessing>()
				.WithEvent<OnAfterStartInboxProcessing>()
				.WithEvent<OnStartControlInboxProcessing>()
				.WithEvent<OnAfterStartControlInboxProcessing>()
				.WithEvent<OnStartOutboxProcessing>()
				.WithEvent<OnAfterStartOutboxProcessing>()
				.WithEvent<OnStartDeferredMessageProcessing>()
				.WithEvent<OnAfterStartDeferredMessageProcessing>()
				.WithEvent<OnStartWorker>()
				.WithEvent<OnAfterStartWorker>();

			RegisterStage("Final")
				.WithEvent<OnStarting>();

			RegisterObserver(new ServiceBusStartupObserver(bus));
		}
	}
}