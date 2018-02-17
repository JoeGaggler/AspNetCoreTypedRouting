using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Builder
{
	/// <summary>
	/// Extensions to inserts Typed Routing into the pipeline
	/// </summary>
    public static class ApplicationBuilderExtensions
    {
		public static IApplicationBuilder UseTypedRouting<TRootRouteValues>(this IApplicationBuilder app)
		{
			return app.UseMiddleware<TypedRoutingMiddleware<TRootRouteValues>>();
		}
    }
}
