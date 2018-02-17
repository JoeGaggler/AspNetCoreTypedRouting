using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalRouter<TRouteValues>
	{
		private interface IGuidContainer
		{
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, Guid guid, PathString suffix);
		}

		private class GuidContainer<TChildRouteValues> : IGuidContainer
		{
			private readonly ITypedRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, Guid, TChildRouteValues> ChildRouteValuesFunc;

			public GuidContainer(Func<TRouteValues, Guid, TChildRouteValues> childRouteValuesFunc, ITypedRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> IGuidContainer.TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, Guid guid, PathString suffix)
			{
				var childValues = this.ChildRouteValuesFunc(prefix, guid);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, childValues, suffix);
			}
		}
	}
}
