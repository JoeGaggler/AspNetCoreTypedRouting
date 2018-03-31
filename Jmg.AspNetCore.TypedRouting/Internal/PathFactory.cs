using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Internal
{
	/// <summary>
	/// Internal implementation of <see cref="ITypedRoutePathFactory{TRootRouteValues}"/>
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values</typeparam>
	public class PathFactory<TRootRouteValues> : ITypedRoutePathFactory<TRootRouteValues>
	{
		private Dictionary<Type, Object> pathFuncMap = new Dictionary<Type, Object>(); // Object is Func<TRouteValues, PathString>

		/// <summary>
		/// Constructs <see cref="PathFactory{TRootRouteValues}"/>
		/// </summary>
		public PathFactory() { }

		PathString ITypedRoutePathFactory<TRootRouteValues>.GetPath<TRouteValues>(TRouteValues routeValues)
		{
			if (!this.pathFuncMap.TryGetValue(typeof(TRouteValues), out var func) || !(func is Func<TRouteValues, PathString> pathFunc))
			{
				throw new InvalidOperationException("Route does not exist");
			}

			return pathFunc(routeValues);
		}

		/// <summary>
		/// Adds a new path mapping
		/// </summary>
		/// <typeparam name="TRouteValues">Source values for generating the path</typeparam>
		/// <param name="mapping">Mapping from route values to a path</param>
		public void AddPath<TRouteValues>(Func<TRouteValues, PathString> mapping)
		{
			this.pathFuncMap[typeof(TRouteValues)] = mapping;
		}
	}
}
