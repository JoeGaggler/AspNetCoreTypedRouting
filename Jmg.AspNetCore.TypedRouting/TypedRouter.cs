using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Typed Router
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values from the current Typed Routing middleware</typeparam>
	public class TypedRouter<TRootRouteValues>
	{
		private readonly ITypedRouteHandler<TRootRouteValues> rootRouteHandler;
		private readonly ITypedRoutePathFactory<TRootRouteValues> pathFactory;
		private readonly TRootRouteValues rootRouteValues;

		/// <summary>
		/// Constructs a Typed Router
		/// </summary>
		/// <param name="handler">Configured route handler</param>
		/// <param name="pathFactory">Configured path factory</param>
		/// <param name="rootRouteValues">Root route values</param>
		public TypedRouter(ITypedRouteHandler<TRootRouteValues> handler, ITypedRoutePathFactory<TRootRouteValues> pathFactory, TRootRouteValues rootRouteValues)
		{
			this.rootRouteHandler = handler;
			this.pathFactory = pathFactory;
			this.rootRouteValues = rootRouteValues;
		}

		/// <summary>
		/// Maps route values to <see cref="PathString"/>
		/// </summary>
		/// <typeparam name="TRouteValues">Route values</typeparam>
		/// <param name="routeValues">Route values</param>
		/// <returns>Path mapped from route values</returns>
		public PathString GetPath<TRouteValues>(TRouteValues routeValues) => this.pathFactory.GetPath(routeValues);

		/// <summary>
		/// Attempts to apply Typed Routes to the current request
		/// </summary>
		/// <param name="httpContext">Current reuest</param>
		/// <returns>Task that indicates whether the request was successfully routed</returns>
		public async Task<Boolean> TryInvokeAsync(HttpContext httpContext)
		{
			var path = httpContext.Request.Path;
			return await this.rootRouteHandler.TryInvokeAsync(httpContext, this.rootRouteValues, path);
		}
	}
}
