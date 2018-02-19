using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal class Leaf<TRouteValues> : ITypedRouteHandler<TRouteValues>
	{
		private readonly ITypedRoutingEndpoint<TRouteValues> endpoint;

		public Leaf(ITypedRoutingEndpoint<TRouteValues> endpoint)
		{
			this.endpoint = endpoint;
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

			// This route does not have any subpaths.
			return false;
		}
	}
}
