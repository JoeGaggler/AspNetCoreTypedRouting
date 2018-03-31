using Jmg.AspNetCore.TypedRouting.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal class Guid<TRouteValues, TChildRouteValues> : ITypedRouteHandler<TRouteValues>
	{
		private readonly ITypedRoutingEndpoint<TRouteValues> endpoint;
		private readonly Func<TRouteValues, Guid, TChildRouteValues> childRouteValuesFunc;
		private readonly ITypedRouteHandler<TChildRouteValues> next;

		public Guid(ITypedRoutingEndpoint<TRouteValues> endpoint, Func<TRouteValues, Guid, TChildRouteValues> childRouteValuesFunc, ITypedRouteHandler<TChildRouteValues> next)
		{
			this.endpoint = endpoint;
			this.childRouteValuesFunc = childRouteValuesFunc;
			this.next = next;
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
					await this.endpoint.RunAsync(httpContext, routeValues);
					return true;
				}
			}

			if (!path.TryGetStartingSegment(out var prefix, out var suffix))
			{
				return false;
			}

			if (!Guid.TryParse(prefix, out var guid))
			{
				return false;
			}

			var childRouteValues = this.childRouteValuesFunc(routeValues, guid);

			return await this.next.TryInvokeAsync(httpContext, childRouteValues, suffix);
		}
	}
}
