using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
    partial class InternalRouter<TRouteValues>
    {
		private interface IPathContainer
		{
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues);
		}

		private class PathContainer<TChildRouteValues> : IPathContainer
		{
			private readonly IRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, TChildRouteValues> ChildRouteValuesFunc;

			public PathContainer(Func<TRouteValues, TChildRouteValues> childRouteValuesFunc, IRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> IPathContainer.TryInvokeAsync(HttpContext httpContext, PathString right, TRouteValues leftValues)
			{
				var childValues = this.ChildRouteValuesFunc(leftValues);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, right, childValues);
			}
		}
	}
}
