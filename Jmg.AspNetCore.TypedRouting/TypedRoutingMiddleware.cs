﻿using Microsoft.AspNetCore.Http;
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
		private readonly TypedRouter<TRootRouteValues> typedRouter;

		/// <summary>
		/// Constructs the middleware
		/// </summary>
		/// <param name="config">Injected dependency for route configuration</param>
		public TypedRoutingMiddleware(ITypedRouteFactory<TRootRouteValues> config)
		{
			TypedRoutePathFactory pathBuilder = new TypedRoutePathFactory();
			ITypedRouteBuilder<TRootRouteValues> builder = new InternalBuilder<TRootRouteValues>(pathBuilder, null, TypedRouteOptions.None);

			config.Configure(builder, pathBuilder);

			var handler = builder.Build();			

			var typedRouter = new TypedRouter<TRootRouteValues>(handler, pathBuilder, config.RootRouteValues);
			this.typedRouter = typedRouter;
		}

		async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (!await typedRouter.TryInvokeAsync(context))
			{
				await next(context);
			}
		}
	}
}
