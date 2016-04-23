namespace Shuttle.Esb
{
	public class OutboxProcessor : QueueProcessor<OutboxPipeline>
	{
		public OutboxProcessor(IServiceBus bus)
			: base(bus, bus.Configuration.ThreadActivityFactory.CreateOutboxThreadActivity(bus))
		{
		}
	}
}