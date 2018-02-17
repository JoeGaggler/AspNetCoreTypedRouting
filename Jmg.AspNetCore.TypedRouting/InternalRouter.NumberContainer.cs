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
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, Int32 number, PathString suffix);
		}

		private class NumberContainer<TChildRouteValues> : INumberContainer
		{
			private readonly ITypedRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, Int32, TChildRouteValues> ChildRouteValuesFunc;

			public NumberContainer(Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc, ITypedRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> INumberContainer.TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, Int32 number, PathString suffix)
			{
				var childValues = this.ChildRouteValuesFunc(prefix, number);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, childValues, suffix);
			}
		}
	}
}
