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
		}
	}
}
