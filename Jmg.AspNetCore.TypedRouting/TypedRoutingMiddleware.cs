using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Middleware that implements Typed Routing
	/// </summary>
	/// <typeparam name="TRootRouteValues">Route route values</typeparam>
	public class TypedRoutingMiddleware<TRootRouteValues> : IMiddleware
	{
		private readonly ITypedRouteHandler<RootRouteValues> routeHander;

		/// <summary>
		/// Constructs the middleware
		/// </summary>
		/// <param name="config">Injected dependency for route configuration</param>
		public TypedRoutingMiddleware(ITypedRouteFactory<TRootRouteValues> config)
		{
			var routeHandler = new InternalRouter<RootRouteValues>();
			config.Configure(routeHandler);
			this.routeHander = routeHandler;
		}

		async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var router = this.routeHander;
			var path = context.Request.Path;
			if (!await router.TryInvokeAsync(context, RootRouteValues.Instance, path))
			{
				await next(context);
			}
		}
	}
}
