using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalBuilder<TRouteValues>
	{
		private interface INumberContainer
		{
			ITypedRouteHandler<TRouteValues> Build(ITypedRoutingEndpoint<TRouteValues> endpoint);
		}

		private class NumberContainer<TChildRouteValues> : INumberContainer
		{
			private readonly ITypedRouteBuilder<TChildRouteValues> ChildBuilder;
			private readonly Func<TRouteValues, Int32, TChildRouteValues> ChildRouteValuesFunc;

			public NumberContainer(Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc, ITypedRouteBuilder<TChildRouteValues> childBuilder)
			{
				this.ChildBuilder = childBuilder;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			ITypedRouteHandler<TRouteValues> INumberContainer.Build(ITypedRoutingEndpoint<TRouteValues> endpoint)
			{
				var next = this.ChildBuilder.Build();
				return new RouteHandlers.Number<TRouteValues, TChildRouteValues>(endpoint, this.ChildRouteValuesFunc, next);
			}
		}
	}
}
