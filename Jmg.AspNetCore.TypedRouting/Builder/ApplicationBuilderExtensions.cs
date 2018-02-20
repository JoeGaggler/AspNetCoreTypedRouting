using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Builder
{
	/// <summary>
	/// Extensions to inserting Typed Routing into the pipeline
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		/// Adds Typed Routing middleware to the pipeline
		/// </summary>
		/// <typeparam name="TRootRouteValues">Type that represents the root route values</typeparam>
		/// <param name="app">Application builder to extend</param>
		/// <returns>Application with the Typed Routing middleware</returns>
		public static IApplicationBuilder UseTypedRouting<TRootRouteValues>(this IApplicationBuilder app) => 
			app.UseMiddleware<TypedRoutingMiddleware<TRootRouteValues>>();

		/// <summary>
		/// Adds Typed Routing middleware to the pipeline
		/// </summary>
		/// <param name="app">Application builder to extend</param>
		/// <returns>Application with the Typed Routing middleware</returns>
		public static IApplicationBuilder UseTypedRouting(this IApplicationBuilder app) => UseTypedRouting<RootRouteValues>(app);
	}
}
