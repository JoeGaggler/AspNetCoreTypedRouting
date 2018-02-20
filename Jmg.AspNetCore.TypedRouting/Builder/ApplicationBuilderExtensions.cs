using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Builder
{
	/// <summary>
	/// Extensions to inserts Typed Routing into the pipeline
	/// </summary>
    public static class ApplicationBuilderExtensions
    {
		public static IServiceCollection AddTypedRouting<TRootRouteValues, TFac>(this IServiceCollection services) where TFac : class, ITypedRouteFactory<TRootRouteValues>
		{
			services.AddSingleton<ITypedRoutePathFactory<TRootRouteValues>>(new TypedRoutePathFactory<TRootRouteValues>());
			services.AddSingleton<ITypedRouteFactory<TRootRouteValues>, TFac>();
			return services.AddSingleton<TypedRoutingMiddleware<RootRouteValues>>();
		}

		public static IServiceCollection AddTypedRouting<TRootRouteValues>(this IServiceCollection services)
		{
			services.AddSingleton<ITypedRoutePathFactory<TRootRouteValues>>(new TypedRoutePathFactory<TRootRouteValues>());
			return services.AddSingleton<TypedRoutingMiddleware<RootRouteValues>>();
		}

		public static IServiceCollection AddTypedRouting(this IServiceCollection services) => AddTypedRouting<RootRouteValues>(services);


		public static IApplicationBuilder UseTypedRouting<TRootRouteValues>(this IApplicationBuilder app)
		{
			return app.UseMiddleware<TypedRoutingMiddleware<TRootRouteValues>>();
		}

		public static IApplicationBuilder UseTypedRouting(this IApplicationBuilder app) => UseTypedRouting<RootRouteValues>(app);
	}
}
