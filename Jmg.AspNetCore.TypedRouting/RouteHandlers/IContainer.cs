using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.RouteHandlers
{
	internal interface IContainer<TRouteValues>
	{
		Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, PathString suffix);
	}

	internal interface IContainer<TRouteValues, TLastRouteValue>
	{
		Task<Boolean> TryInvokeAsync(HttpContext httpContext, TRouteValues prefix, TLastRouteValue value, PathString suffix);
	}
}
