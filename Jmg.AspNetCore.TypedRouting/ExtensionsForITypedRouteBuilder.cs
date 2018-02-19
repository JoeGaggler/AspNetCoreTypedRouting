using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	public static class ExtensionsForITypedRouteBuilder
	{
		public static ITypedRouteBuilder<TChildRouteValues> AddNamedNumber<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String segment,
			Func<TRouteValues, Int32, TChildRouteValues> func,
			Func<TChildRouteValues, TRouteValues> reverseFunc,
			Func<TChildRouteValues, Int32> splitFunc)
		{
			return routeBuilder
				.AddLiteral(segment, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddInt32(
					(rv, v) => func(rv, v),
					(v) => reverseFunc(v),
					splitFunc);
		}

		public static ITypedRouteBuilder<TChildRouteValues> AddNamedGuid<TRouteValues, TChildRouteValues>(
			this ITypedRouteBuilder<TRouteValues> routeBuilder,
			String segment,
			Func<TRouteValues, Guid, TChildRouteValues> func,
			Func<TChildRouteValues, TRouteValues> reverseFunc,
			Func<TChildRouteValues, Guid> splitFunc)
		{
			return routeBuilder
				.AddLiteral(segment, p => p, c => c, TypedRouteOptions.IntermediateRoute)
				.AddGuid(
					(rv, v) => func(rv, v),
					(v) => reverseFunc(v),
					splitFunc);
		}
	}
}
