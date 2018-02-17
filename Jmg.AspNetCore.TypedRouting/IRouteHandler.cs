using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can locate an endpoint for a route based on its path
	/// </summary>
	/// <typeparam name="TRouteValues"></typeparam>
	internal interface IRouteHandler<TRouteValues>
	{
		Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues);
	}
}
