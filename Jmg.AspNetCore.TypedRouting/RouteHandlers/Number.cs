using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jmg.AspNetCore.TypedRouting.Extensions;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal class Number<TRouteValues, TChildRouteValues> : ITypedRouteHandler<TRouteValues>
	{
		private readonly ITypedRoutingEndpoint<TRouteValues> endpoint;
		private readonly Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc;
		private readonly ITypedRouteHandler<TChildRouteValues> next;

		public Number(ITypedRoutingEndpoint<TRouteValues> endpoint, Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc, ITypedRouteHandler<TChildRouteValues> next)
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

			if (!Int32.TryParse(prefix, out var number))
			{
				return false;
			}

			var childRouteValues = this.childRouteValuesFunc(routeValues, number);

			return await this.next.TryInvokeAsync(httpContext, childRouteValues, suffix);
		}
	}
}
