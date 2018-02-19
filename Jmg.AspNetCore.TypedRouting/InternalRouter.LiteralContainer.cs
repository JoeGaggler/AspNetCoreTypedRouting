using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalRouter<TRouteValues>
	{
		private interface ILiteralContainer
		{
			ITypedRouteHandler<TRouteValues> Build(String literal, ITypedRoutingEndpoint<TRouteValues> endpoint);
		}

		private class LiteralContainer<TChildRouteValues> : ILiteralContainer
		{
			private readonly ITypedRouteBuilder<TChildRouteValues> ChildBuilder;
			private readonly Func<TRouteValues, TChildRouteValues> ChildRouteValuesFunc;

			public String Literal { get; private set; }

			public LiteralContainer(String literal, Func<TRouteValues, TChildRouteValues> childRouteValuesFunc, ITypedRouteBuilder<TChildRouteValues> childBuilder)
			{
				this.Literal = literal;
				this.ChildBuilder = childBuilder;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			ITypedRouteHandler<TRouteValues> ILiteralContainer.Build(String literal, ITypedRoutingEndpoint<TRouteValues> endpoint)
			{
				var next = this.ChildBuilder.Build();
				return new RouteHandlers.SingleLiteral<TRouteValues, TChildRouteValues>(literal, endpoint, this.ChildRouteValuesFunc, next);
			}
		}
	}
}
