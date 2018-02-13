using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	public class TypedRouter
	{
		private IRouteHandler<RootRouteValues> routeHander;

		public TypedRouter(ITypedRouteBuilder builder)
		{
			var routeHandler = new InternalRouter<RootRouteValues>();
			builder.Configure(routeHandler);
			this.routeHander = routeHandler;
		}

		public async Task<Boolean> TryInvokeAsync(HttpContext httpContext)
		{
			var path = httpContext.Request.Path;
			return await this.routeHander.TryInvokeAsync(httpContext, path, RootRouteValues.Instance);
		}
	}
}
