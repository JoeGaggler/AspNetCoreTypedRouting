﻿using Microsoft.AspNetCore.Http;
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
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, PathString suffix);
		}

		private class LiteralContainer<TChildRouteValues> : IPathContainer
		{
			private readonly ITypedRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, TChildRouteValues> ChildRouteValuesFunc;

			public LiteralContainer(Func<TRouteValues, TChildRouteValues> childRouteValuesFunc, ITypedRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> IPathContainer.TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, PathString suffix)
			{
				var childValues = this.ChildRouteValuesFunc(prefix);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, childValues, suffix);
			}
		}
	}
}
