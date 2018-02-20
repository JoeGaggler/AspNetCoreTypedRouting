using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Extensions
{
	public static class ExtensionsForITypedRouteBuilder
	{
		public static ITypedRouteBuilder<TChildRouteValues> AddNamedNumber<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String literal,
			Func<TRouteValues, Int32, TChildRouteValues> func,
			Func<TChildRouteValues, TRouteValues> reverseFunc,
			Func<TChildRouteValues, Int32> splitFunc)
		{
			return routeBuilder
				.AddLiteral(literal, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddInt32(
					(rv, v) => func(rv, v),
					(v) => reverseFunc(v),
					splitFunc);
		}

		public static ITypedRouteBuilder<TChildRouteValues> AddNamedGuid<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String literal,
			Func<TRouteValues, Guid, TChildRouteValues> func,
			Func<TChildRouteValues, TRouteValues> reverseFunc,
			Func<TChildRouteValues, Guid> splitFunc)
		{
			return routeBuilder
				.AddLiteral(literal, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddGuid(
					(rv, v) => func(rv, v),
					(v) => reverseFunc(v),
					splitFunc);
		}
	}
}
