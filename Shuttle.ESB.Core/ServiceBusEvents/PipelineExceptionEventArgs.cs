﻿using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class PipelineExceptionEventArgs : EventArgs
	{
		public Pipeline Pipeline { get; private set; }

		public PipelineExceptionEventArgs(Pipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}