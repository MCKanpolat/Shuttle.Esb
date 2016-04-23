﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public abstract class MessagePipeline : Pipeline
	{
		protected readonly IServiceBus _bus;

		protected MessagePipeline(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;

			State.Add(bus);
		}

		protected MessagePipeline()
		{
		}

		public virtual void Obtained()
		{
			State.Clear();
			State.Add(_bus);

			_bus.Events.OnPipelineObtained(this, new PipelineEventArgs(this));
		}

		public void Released()
		{
			_bus.Events.OnPipelineReleased(this, new PipelineEventArgs(this));
		}
	}
}