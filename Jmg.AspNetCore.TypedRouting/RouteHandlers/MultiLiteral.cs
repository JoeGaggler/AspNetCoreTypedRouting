using Jmg.AspNetCore.TypedRouting.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal interface IMultiLiteralContainer<TRouteValues>
	{
		String Literal { get; }

		Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues routeValues, PathString suffix);
	}

	internal class MultiLiteralContainer<TRouteValues, TChildRouteValues> : IMultiLiteralContainer<TRouteValues>
	{
		private readonly Func<TRouteValues, TChildRouteValues> ChildRouteValuesFunc;
		private readonly ITypedRouteHandler<TChildRouteValues> Next;

		public MultiLiteralContainer(String literal, Func<TRouteValues, TChildRouteValues> func, ITypedRouteHandler<TChildRouteValues> next)
		{
			this.ChildRouteValuesFunc = func;
			this.Next = next;
			this.Literal = literal;
		}

		public String Literal { get; private set; }

		Task<Boolean> IMultiLiteralContainer<TRouteValues>.TryInvokeAsync(HttpContext httpContext, TRouteValues routeValues, PathString suffix)
		{
			return this.Next.TryInvokeAsync(httpContext, this.ChildRouteValuesFunc(routeValues), suffix);
		}
	}

	internal class MultiLiteral<TRouteValues> : ITypedRouteHandler<TRouteValues>
	{
		private readonly ITypedRoutingEndpoint<TRouteValues> endpoint;
		private readonly Dictionary<String, IMultiLiteralContainer<TRouteValues>> entries;

		public MultiLiteral(ITypedRoutingEndpoint<TRouteValues> endpoint, IEnumerable<IMultiLiteralContainer<TRouteValues>> items)
		{
			this.endpoint = endpoint;
			this.entries = items.ToDictionary(i => i.Literal, i => i);
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

			if (!entries.TryGetValue(prefix, out var container))
			{
				return false;
			}

			return await container.TryInvokeAsync(httpContext, routeValues, suffix);
		}
	}
}
