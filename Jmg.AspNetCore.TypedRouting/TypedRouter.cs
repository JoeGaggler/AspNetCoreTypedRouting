using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
    public class TypedRouter<TRootRouteValues>
	{
		// TODO: Separate building from handling
		//public InternalRouter<RootRouteValues> RootRoute { get; private set; }

		private readonly ITypedRouteHandler<TRootRouteValues> rootRouteHandler;
		private readonly ITypedRoutePathFactory<TRootRouteValues> pathFactory;
		private readonly TRootRouteValues rootRouteValues;

		public TypedRouter(ITypedRouteHandler<TRootRouteValues> handler, ITypedRoutePathFactory<TRootRouteValues> pathFactory, TRootRouteValues rootRouteValues)
		{
			this.rootRouteHandler = handler;
			this.pathFactory = pathFactory;
			this.rootRouteValues = rootRouteValues;
		}

		public PathString GetPath<TRouteValues>(TRouteValues routeValues) => this.pathFactory.GetPath(routeValues);

		public async Task<Boolean> TryInvokeAsync(HttpContext httpContext)
		{
			var path = httpContext.Request.Path;
			return await this.rootRouteHandler.TryInvokeAsync(httpContext, this.rootRouteValues, path);
		}
	}
}
