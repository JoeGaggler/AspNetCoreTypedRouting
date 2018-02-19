using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	public class StringEndpoint<TRouteValues> : ITypedRoutingEndpoint<TRouteValues>
	{
		private readonly Func<TRouteValues, String> func;

		public StringEndpoint(Func<TRouteValues, String> func)
		{
			this.func = func;
		}

		public async Task Run(HttpContext httpContext, TRouteValues routeValues)
		{
			var response = httpContext.Response;
			var body = response.Body;

			var result = this.func(routeValues);
			if (result == null)
			{
				response.StatusCode = StatusCodes.Status404NotFound;
				return;
			}

			response.StatusCode = StatusCodes.Status200OK;
			using (var textWriter = new StreamWriter(body, Encoding.UTF8, 4096, true))
			{
				await textWriter.WriteAsync(result);
			}
		}

		public interface IEndpoint
		{
			String Get(TRouteValues routeValues);
		}
	}
}
