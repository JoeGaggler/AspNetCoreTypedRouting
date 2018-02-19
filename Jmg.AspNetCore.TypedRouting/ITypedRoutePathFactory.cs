﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
    public interface ITypedRoutePathFactory
    {
		PathString GetPath<TRouteValues>(TRouteValues routeValues);
    }

	internal class TypedRoutePathFactory : ITypedRoutePathFactory
	{
		private Dictionary<Type, Object> pathFuncMap = new Dictionary<Type, Object>(); // Object is Func<TRouteValues, PathString>

		PathString ITypedRoutePathFactory.GetPath<TRouteValues>(TRouteValues routeValues)
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
