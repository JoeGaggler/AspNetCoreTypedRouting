using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can locate and invoke an endpoint for a route based on its path
	/// </summary>
	/// <typeparam name="TRouteValues"></typeparam>
	public interface ITypedRouteHandler<TRouteValues>
	{
		/// <summary>
		/// Tries to locate and invoke and endpoint
		/// </summary>
		/// <param name="httpContext">Context for the current request</param>
		/// <param name="routeValues">Current route values</param>
		/// <param name="path">Remaining suffix path to be routed</param>
		/// <returns>True if an endpoint was located and invoked</returns>
		Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues routeValues, PathString path);
	}
}
