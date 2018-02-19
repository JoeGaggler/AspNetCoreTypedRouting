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
		private readonly TRootRouteValues rootRouteValues;

		// TODO: Make private
		internal Dictionary<Type, Object> pathFuncMap = new Dictionary<Type, Object>(); // Object is Func<TRouteValues, PathString>

		public TypedRouter(ITypedRouteHandler<TRootRouteValues> handler, TRootRouteValues rootRouteValues)
		{
			this.rootRouteHandler = handler;
			this.rootRouteValues = rootRouteValues;
		}

		public PathString GetPath<TRouteValues>(TRouteValues routeValues)
		{
			if (!this.pathFuncMap.TryGetValue(typeof(TRouteValues), out var func) || !(func is Func<TRouteValues, PathString> pathFunc))
			{
				throw new InvalidOperationException("Route does not exist");
			}

			return pathFunc(routeValues);
		}

		public async Task<Boolean> TryInvokeAsync(HttpContext httpContext)
		{
			var path = httpContext.Request.Path;
			return await this.rootRouteHandler.TryInvokeAsync(httpContext, this.rootRouteValues, path);
		}
	}
}
