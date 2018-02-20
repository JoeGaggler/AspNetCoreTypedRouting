using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Extensions
{
	/// <summary>
	/// Extensions for simplifying the creation of typical routes
	/// </summary>
	public static class ExtensionsForITypedRouteBuilder
	{
		/// <summary>
		/// Adds a route that expects a literal text segment followed by a variable number segment
		/// </summary>
		/// <typeparam name="TRouteValues"></typeparam>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="routeBuilder">Route builder to extend</param>
		/// <param name="literal"></param>
		/// <param name="getChildRouteValues">Function that creates the new route values</param>
		/// <param name="getParentRouteValues">Function that returns the parent's route values</param>
		/// <param name="getLastValue">Function the returns the new value for this route</param>
		/// <param name="options">Route options</param>
		/// <returns>Route builder that handles the new route</returns>
		public static ITypedRouteBuilder<TChildRouteValues> AddNamedNumber<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String literal,
			Func<TRouteValues, Int32, TChildRouteValues> getChildRouteValues,
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			Func<TChildRouteValues, Int32> getLastValue,
			TypedRouteOptions options = TypedRouteOptions.None)
		{
			return routeBuilder
				.AddLiteral(literal, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddInt32(
					(rv, v) => getChildRouteValues(rv, v),
					(v) => getParentRouteValues(v),
					getLastValue,
					options);
		}

		/// <summary>
		/// Adds a route that expects a literal text segment followed by a variable number segment
		/// </summary>
		/// <typeparam name="TRouteValues"></typeparam>
		/// <typeparam name="TChildRouteValues">Route values that represent the new route</typeparam>
		/// <param name="routeBuilder">Route builder to extend</param>
		/// <param name="literal"></param>
		/// <param name="getChildRouteValues">Function that creates the new route values</param>
		/// <param name="getParentRouteValues">Function that returns the parent's route values</param>
		/// <param name="getLastValue">Function the returns the new value for this route</param>
		/// <param name="options">Route options</param>
		/// <returns>Route builder that handles the new route</returns>
		public static ITypedRouteBuilder<TChildRouteValues> AddNamedGuid<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String literal,
			Func<TRouteValues, Guid, TChildRouteValues> getChildRouteValues,
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			Func<TChildRouteValues, Guid> getLastValue,
			TypedRouteOptions options = TypedRouteOptions.None)
		{
			return routeBuilder
				.AddLiteral(literal, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddGuid(
					(rv, v) => getChildRouteValues(rv, v),
					(v) => getParentRouteValues(v),
					getLastValue,
					options);
		}
	}
}
