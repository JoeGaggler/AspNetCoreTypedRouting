using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	public class TypedRoutingMiddleware : IMiddleware
	{
		public TypedRoutingMiddleware()
		{

		}

		async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var router = context.RequestServices.GetService<TypedRouter>();
			if (!await router.TryInvokeAsync(context))
			{
				await next(context);
			}
		}
	}
}
