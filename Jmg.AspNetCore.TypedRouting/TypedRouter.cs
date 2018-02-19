using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
    public class TypedRouter
    {
		public InternalRouter<RootRouteValues> RootRoute { get; private set; }

		// TODO: Make private
		internal Dictionary<Type, Object> pathFuncMap = new Dictionary<Type, Object>(); // Object is Func<TRouteValues, PathString>

		public TypedRouter()
		{
			this.RootRoute = new InternalRouter<RootRouteValues>(this, null, TypedRouteOptions.None);
		}

		public PathString GetPath<TRouteValues>(TRouteValues routeValues)
		{
			if (!this.pathFuncMap.TryGetValue(typeof(TRouteValues), out var func) || !(func is Func<TRouteValues, PathString> pathFunc))
			{
				throw new InvalidOperationException("Route does not exist");
			}

			return pathFunc(routeValues);
		}
	}
}
