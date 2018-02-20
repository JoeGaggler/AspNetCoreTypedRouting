using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can configure a Typed Route Builder
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values</typeparam>
    public interface ITypedRouteFactory<TRootRouteValues>
    {
		/// <summary>
		/// Callback for initializing the routes
		/// </summary>
		/// <param name="typedRouter"></param>
		void Configure(ITypedRouteBuilder<TRootRouteValues> typedRouter);

		/// <summary>
		/// Instance that represents the root route values
		/// </summary>
		TRootRouteValues RootRouteValues { get; }
	}
}
