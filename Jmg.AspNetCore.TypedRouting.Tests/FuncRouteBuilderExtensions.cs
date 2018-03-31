using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.Tests
{
    public static class FuncRouteBuilderExtensions
    {
		public static void AttachAsyncFunc<TRouteValues>(this ITypedRouteBuilder<TRouteValues> builder, Func<TRouteValues, Task> func)
		{
			builder.Endpoint = new Endpoint<TRouteValues>(func);
		}

		public static void AttachAction<TRouteValues>(this ITypedRouteBuilder<TRouteValues> builder, Action<TRouteValues> action)
		{
			builder.AttachAsyncFunc((rv) => { action(rv); return Task.CompletedTask; });
		}

		private class Endpoint<TRouteValues> : ITypedRoutingEndpoint<TRouteValues>
		{
			private readonly Func<TRouteValues, Task> func;

			public Endpoint(Func<TRouteValues, Task> func)
			{
				this.func = func;
			}

			public Task RunAsync(HttpContext httpContext, TRouteValues routeValues) => this.func(routeValues);
		}
	}
}
