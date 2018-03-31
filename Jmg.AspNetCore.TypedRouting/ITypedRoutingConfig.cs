using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can configure Typed Routing middleware
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values</typeparam>
    public interface ITypedRoutingConfig<TRootRouteValues>
    {
		/// <summary>
		/// Callback for initializing the routes
		/// </summary>
		/// <param name="routeBuilder"></param>
		void BuildRoutes(ITypedRouteBuilder<TRootRouteValues> routeBuilder);

		/// <summary>
		/// Instance that represents the root route values
		/// </summary>
		TRootRouteValues RootRouteValues { get; }
	}
}
