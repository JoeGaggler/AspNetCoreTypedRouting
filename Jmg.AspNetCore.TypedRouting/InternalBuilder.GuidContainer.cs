using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalBuilder<TRootRouteValues, TRouteValues>
	{
		private interface IGuidContainer
		{
			ITypedRouteHandler<TRouteValues> Build(ITypedRoutingEndpoint<TRouteValues> endpoint);
		}

		private class GuidContainer<TChildRouteValues> : IGuidContainer
		{
			private readonly ITypedRouteBuilder<TChildRouteValues> ChildBuilder;
			private readonly Func<TRouteValues, Guid, TChildRouteValues> ChildRouteValuesFunc;

			public GuidContainer(Func<TRouteValues, Guid, TChildRouteValues> childRouteValuesFunc, ITypedRouteBuilder<TChildRouteValues> childBuilder)
			{
				this.ChildBuilder = childBuilder;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			ITypedRouteHandler<TRouteValues> IGuidContainer.Build(ITypedRoutingEndpoint<TRouteValues> endpoint)
			{
				var next = this.ChildBuilder.Build();
				return new RouteHandlers.Guid<TRouteValues, TChildRouteValues>(endpoint, this.ChildRouteValuesFunc, next);
			}
		}
	}
}
