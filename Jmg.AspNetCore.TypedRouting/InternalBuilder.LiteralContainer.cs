using Jmg.AspNetCore.TypedRouting.RouteHandlers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	partial class InternalBuilder<TRootRouteValues, TRouteValues>
	{
		private interface ILiteralContainer
		{
			ITypedRouteHandler<TRouteValues> BuildSingle(String literal, ITypedRoutingEndpoint<TRouteValues> endpoint);
			IMultiLiteralContainer<TRouteValues> BuildMulti(String key);
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

			ITypedRouteHandler<TRouteValues> ILiteralContainer.BuildSingle(String literal, ITypedRoutingEndpoint<TRouteValues> endpoint)
			{
				var next = this.ChildBuilder.Build();
				return new RouteHandlers.SingleLiteral<TRouteValues, TChildRouteValues>(literal, endpoint, this.ChildRouteValuesFunc, next);
			}

			IMultiLiteralContainer<TRouteValues> ILiteralContainer.BuildMulti(String literal)
			{
				return new MultiLiteralContainer<TRouteValues, TChildRouteValues>(literal, this.ChildRouteValuesFunc, this.ChildBuilder.Build());
			}
		}
	}
}
