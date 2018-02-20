using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that map route values to <see cref="PathString"/> 
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values from the current Typed Routing middleware</typeparam>
	public interface ITypedRoutePathFactory<TRootRouteValues>
    {
		/// <summary>
		/// Maps route values to <see cref="PathString"/>
		/// </summary>
		/// <typeparam name="TRouteValues">Route values</typeparam>
		/// <param name="routeValues">Route values</param>
		/// <returns>Path mapped from route values</returns>
		PathString GetPath<TRouteValues>(TRouteValues routeValues);
    }

	internal class TypedRoutePathFactory<TRootRouteValues> : ITypedRoutePathFactory<TRootRouteValues>
	{
		private Dictionary<Type, Object> pathFuncMap = new Dictionary<Type, Object>(); // Object is Func<TRouteValues, PathString>

		public TypedRoutePathFactory() { }

		PathString ITypedRoutePathFactory<TRootRouteValues>.GetPath<TRouteValues>(TRouteValues routeValues)
		{
			if (!this.pathFuncMap.TryGetValue(typeof(TRouteValues), out var func) || !(func is Func<TRouteValues, PathString> pathFunc))
			{
				throw new InvalidOperationException("Route does not exist");
			}

			return pathFunc(routeValues);
		}

		public void AddPath<TRouteValues>(Func<TRouteValues, PathString> mapping)
		{
			this.pathFuncMap[typeof(TRouteValues)] = mapping;
		}
	}
}
