using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	public class ClientEndpoint : 
		IEndpoint<ClientRouteValues>,
		IEndpoint<ClientUserTaskRouteValues>
	{
		async Task IEndpoint<ClientRouteValues>.Run(HttpContext httpContext, ClientRouteValues routeValues)
		{
			var contextResponse = httpContext.Response;
			var stringResponse = $"Hello, world. {routeValues.ClientId}";
			var stringBytes = Encoding.ASCII.GetBytes(stringResponse);
			contextResponse.Body.Write(stringBytes, 0, stringBytes.Length);
		}

		async Task IEndpoint<ClientUserTaskRouteValues>.Run(HttpContext httpContext, ClientUserTaskRouteValues routeValues)
		{
			var contextResponse = httpContext.Response;
			var stringResponse = $"Hello, world. GUID: {routeValues.TaskId}";
			var stringBytes = Encoding.ASCII.GetBytes(stringResponse);
			contextResponse.Body.Write(stringBytes, 0, stringBytes.Length);
		}
	}
}
