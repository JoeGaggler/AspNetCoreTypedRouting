using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Builder
{
	/// <summary>
	/// Extensions for adding Typed Routing to the services collection
	/// </summary>
    public static class ServicesCollectionExtensions
    {
		/// <summary>
		/// Adds the Typed Routing middleware
		/// </summary>
		/// <typeparam name="TRootRouteValues">Type that represents the root route values</typeparam>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting<TRootRouteValues>(this IServiceCollection services)
		{
			return services
				.AddSingleton<ITypedRoutePathFactory<TRootRouteValues>>(new Internal.PathFactory<TRootRouteValues>())
				.AddSingleton<Internal.TypedRoutingMiddleware<RootRouteValues>>();
		}

		/// <summary>
		/// Adds the Typed Routing middleware for the root route
		/// </summary>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting(this IServiceCollection services) => AddTypedRouting<RootRouteValues>(services);

		/// <summary>
		/// Adds the Typed Routing middleware with a <see cref="ITypedRoutingConfig{TRootRouteValues}"/>
		/// </summary>
		/// <typeparam name="TRootRouteValues">Type that represents the root route values</typeparam>
		/// <typeparam name="TITypedRouteFactory">Type that implements <see cref="ITypedRoutingConfig{TRootRouteValues}"/></typeparam>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting<TRootRouteValues, TITypedRouteFactory>(this IServiceCollection services) where TITypedRouteFactory : class, ITypedRoutingConfig<TRootRouteValues>
		{
			return services
				.AddTypedRouting<TRootRouteValues>()
				.AddSingleton<ITypedRoutingConfig<TRootRouteValues>, TITypedRouteFactory>();
		}
	}
}
