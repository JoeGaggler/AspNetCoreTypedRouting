using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.Internal
{
	/// <summary>
	/// Middleware that implements Typed Routing
	/// </summary>
	/// <typeparam name="TRootRouteValues">Route route values</typeparam>
	public class TypedRoutingMiddleware<TRootRouteValues> : IMiddleware
	{
		private readonly TRootRouteValues rootRouteValues;
		private readonly ITypedRouteHandler<TRootRouteValues> rootRouteHandler;

		/// <summary>
		/// Constructs the middleware
		/// </summary>
		/// <param name="config">Injected dependency for route configuration</param>
		/// <param name="pathFactory">Injected dependency for the path factory</param>
		public TypedRoutingMiddleware(ITypedRoutingConfig<TRootRouteValues> config, ITypedRoutePathFactory<TRootRouteValues> pathFactory)
		{
			this.rootRouteValues = config.RootRouteValues;

			ITypedRouteBuilder<TRootRouteValues> builder = new Internal.RouteBuilder<TRootRouteValues, TRootRouteValues>(pathFactory, null, TypedRouteOptions.None);

			// Callback for configuration
			config.BuildRoutes(builder);

			this.rootRouteHandler = builder.Build();
		}

		async Task IMiddleware.InvokeAsync(HttpContext httpContext, RequestDelegate next)
		{
			var path = httpContext.Request.Path;
			if (!await this.rootRouteHandler.TryInvokeAsync(httpContext, this.rootRouteValues, path))
			{
				await next(httpContext);
			}
		}
	}
}
