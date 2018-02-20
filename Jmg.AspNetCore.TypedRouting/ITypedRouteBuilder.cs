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
		/// Adds a route that expects static literal text
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="literal">Literal text segment</param>
		/// <param name="getChildRouteValues">Function that creates the new route values</param>
		/// <param name="getParentRouteValues">Function that returns the parent's route values</param>
		/// <param name="options">Route options</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> AddLiteral<TChildRouteValues>(
			String literal, 
			Func<TRouteValues, TChildRouteValues> getChildRouteValues, 
			Func<TChildRouteValues, TRouteValues> getParentRouteValues, 
			TypedRouteOptions options = TypedRouteOptions.None);

		/// <summary>
		/// Adds a route that expects a variable number segment
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="getChildRouteValues">Function that creates the new route values</param>
		/// <param name="getParentRouteValues">Function that returns the parent's route values</param>
		/// <param name="getLastValue">Function the returns the new value for this route</param>
		/// <param name="options">Route options</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> AddInt32<TChildRouteValues>(
			Func<TRouteValues, Int32, TChildRouteValues> getChildRouteValues, 
			Func<TChildRouteValues, TRouteValues> getParentRouteValues, 
			Func<TChildRouteValues, Int32> getLastValue, 
			TypedRouteOptions options = TypedRouteOptions.None);

		/// <summary>
		/// Adds a route that expects a literal text segment followed by a variable Guid segment
		/// </summary>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="getChildRouteValues">Function that creates the new route values</param>
		/// <param name="getParentRouteValues">Function that returns the parent's route values</param>
		/// <param name="getLastValue">Function the returns the new value for this route</param>
		/// <param name="options">Route options</param>
		/// <returns>Route builder that handles the new route</returns>
		ITypedRouteBuilder<TChildRouteValues> AddGuid<TChildRouteValues>(
			Func<TRouteValues, Guid, TChildRouteValues> getChildRouteValues, 
			Func<TChildRouteValues, TRouteValues> getParentRouteValues, 
			Func<TChildRouteValues, Guid> getLastValue, 
			TypedRouteOptions options = TypedRouteOptions.None);

		/// <summary>
		/// Endpoint that handles requests for this path
		/// </summary>
		ITypedRoutingEndpoint<TRouteValues> Endpoint { get; set; }

		/// <summary>
		/// Builds the route handler for this route definition
		/// </summary>
		/// <returns>Route handler for this route definition</returns>
		ITypedRouteHandler<TRouteValues> Build();
	}
}
