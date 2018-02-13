using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
    public interface IEndpoint<TRouteValues>
    {
		Task Run(HttpContext httpContext, TRouteValues routeValues);
    }
}
