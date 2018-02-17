using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that handles an <see cref="HttpContext"/> for a set of route values
	/// </summary>
	/// <typeparam name="TRouteValues">Route values</typeparam>
    public interface IEndpoint<TRouteValues>
    {
		/// <summary>
		/// Runs the endpoint
		/// </summary>
		/// <param name="httpContext">Current HTTP context</param>
		/// <param name="routeValues">Route values for the current request</param>
		/// <returns>Task that completes when the endpoint has returned an HTTP response</returns>
		Task Run(HttpContext httpContext, TRouteValues routeValues);
    }
}
