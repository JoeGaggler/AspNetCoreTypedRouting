using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalRouter<TRouteValues>
	{
		private interface INumberContainer
		{
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues, Int32 number);
		}

		private class NumberContainer<TChildRouteValues> : INumberContainer
		{
			private readonly IRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, Int32, TChildRouteValues> ChildRouteValuesFunc;

			public NumberContainer(Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc, IRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> INumberContainer.TryInvokeAsync(HttpContext httpContext, PathString right, TRouteValues leftValues, Int32 number)
			{
				var childValues = this.ChildRouteValuesFunc(leftValues, number);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, right, childValues);
			}
		}
	}
}
