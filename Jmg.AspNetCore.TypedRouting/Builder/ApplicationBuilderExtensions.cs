using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Builder
{
    public static class ApplicationBuilderExtensions
    {
		public static IApplicationBuilder UseTypedRouting(this IApplicationBuilder app)
		{
			return app.UseMiddleware<TypedRoutingMiddleware>();
		}
    }
}
