﻿using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
	public class DefaultUriResolver : IUriResolver
	{
		private readonly Dictionary<string, Uri> _uris = new Dictionary<string, Uri>();

        public Uri GetTarget(Uri resolverUri)
		{
			var key = resolverUri.OriginalString.ToLower();

			return _uris.ContainsKey(key) ? _uris[key] : null;
		}

	    public void Add(Uri resolverUri, Uri targetUri)
		{
			Guard.AgainstNull(resolverUri, "resolverUri");
			Guard.AgainstNull(targetUri, "targetUri");

			_uris.Add(resolverUri.OriginalString.ToLower(), targetUri);
		}
	}
}