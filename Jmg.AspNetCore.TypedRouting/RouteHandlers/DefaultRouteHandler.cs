using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal class DefaultRouteHandler<TRouteValues> : ITypedRouteHandler<TRouteValues>
	{
		private readonly ITypedRoutingEndpoint<TRouteValues> endpoint;

		// Child routes
		private readonly Dictionary<String, IContainer<TRouteValues>> pathEntries = new Dictionary<String, IContainer<TRouteValues>>();
		private readonly IContainer<TRouteValues, Guid> guidContainer;
		private readonly IContainer<TRouteValues, Int32> numberContainer;

		public DefaultRouteHandler(
			ITypedRoutingEndpoint<TRouteValues> endpoint, 
			Dictionary<String, IContainer<TRouteValues>> pathEntries, 
			IContainer<TRouteValues, Guid> guidContainer, 
			IContainer<TRouteValues, Int32> numberContainer)
		{
			this.endpoint = endpoint;
			this.pathEntries = pathEntries;
			this.guidContainer = guidContainer;
			this.numberContainer = numberContainer;
		}

		async Task<Boolean> ITypedRouteHandler<TRouteValues>.TryInvokeAsync(HttpContext httpContext, TRouteValues routeValues, PathString path)
		{
			if (!path.HasValue || path == "/")
			{
				if (this.endpoint == null)
				{
					return false;
				}
				else
				{
					await this.endpoint.Run(httpContext, routeValues);
					return true;
				}
			}

			if (!path.TryGetStartingSegment(out var prefix, out var suffix))
			{
				return false;
			}

			if (pathEntries.TryGetValue(prefix, out var pathContainer))
			{
				return await pathContainer.TryInvokeAsync(httpContext, routeValues, suffix);
			}
			else if (guidContainer != null)
			{
				if (!Guid.TryParse(prefix, out var key))
				{
					return false;
				}

				return await guidContainer.TryInvokeAsync(httpContext, routeValues, key, suffix);
			}
			else if (numberContainer != null)
			{
				if (!Int32.TryParse(prefix, out var key))
				{
					return false;
				}

				return await numberContainer.TryInvokeAsync(httpContext, routeValues, key, suffix);
			}
			else
			{
				return false;
			}
		}
	}
}
