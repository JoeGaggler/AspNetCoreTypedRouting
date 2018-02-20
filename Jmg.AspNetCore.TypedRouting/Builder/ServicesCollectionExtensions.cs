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
		/// <typeparam name="TITypedRouteFactory">Type that implements <see cref="ITypedRouteFactory{TRootRouteValues}"/></typeparam>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting<TRootRouteValues, TITypedRouteFactory>(this IServiceCollection services) where TITypedRouteFactory : class, ITypedRouteFactory<TRootRouteValues>
		{
			return services
				.AddSingleton<ITypedRoutePathFactory<TRootRouteValues>>(new TypedRoutePathFactory<TRootRouteValues>())
				.AddSingleton<ITypedRouteFactory<TRootRouteValues>, TITypedRouteFactory>()
				.AddSingleton<TypedRoutingMiddleware<RootRouteValues>>();
		}

		/// <summary>
		/// Adds the Typed Routing middleware
		/// </summary>
		/// <typeparam name="TRootRouteValues">Type that represents the root route values</typeparam>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting<TRootRouteValues>(this IServiceCollection services)
		{
			return services
				.AddSingleton<ITypedRoutePathFactory<TRootRouteValues>>(new TypedRoutePathFactory<TRootRouteValues>())
				.AddSingleton<TypedRoutingMiddleware<RootRouteValues>>();
		}

		/// <summary>
		/// Adds the Typed Routing middleware
		/// </summary>
		/// <param name="services">Services collection to extend</param>
		/// <returns>Services collection with the new service</returns>
		public static IServiceCollection AddTypedRouting(this IServiceCollection services) => AddTypedRouting<RootRouteValues>(services);
	}
}
