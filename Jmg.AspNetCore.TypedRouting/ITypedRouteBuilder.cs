using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can construct a routing table by adding new route values
	/// </summary>
	/// <typeparam name="TRouteValues">Route values that describes the base route</typeparam>
    public interface ITypedRouteBuilder<TRouteValues>
    {
		/// <summary>
		/// Adds a route that expects a literal text segment
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="segment">Literal text segment</param>
		/// <param name="func">Function that creates the new route values</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, TChildRouteValues> func);

		/// <summary>
		/// Adds a route that expects a literal text segment followed by a variable number segment
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="segment">Literal text segment</param>
		/// <param name="func">Function that creates the new route values</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, Int32, TChildRouteValues> func);

		/// <summary>
		/// Adds a route that expects a literal text segment followed by a variable Guid segment
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="segment">Literal text segment</param>
		/// <param name="func">Function that creates the new route values</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, Guid, TChildRouteValues> func);

		/// <summary>
		/// Endpoint that handles requests for this path
		/// </summary>
		ITypedRoutingEndpoint<TRouteValues> Endpoint { get; set; }
	}
}
