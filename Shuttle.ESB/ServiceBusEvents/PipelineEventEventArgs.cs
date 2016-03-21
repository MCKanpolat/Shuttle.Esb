﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public class PipelineEventEventArgs
	{
		public PipelineEventEventArgs(PipelineEvent pipelineEvent)
		{
			PipelineEvent = pipelineEvent;
		}

		public PipelineEvent PipelineEvent { get; private set; }
	}
}