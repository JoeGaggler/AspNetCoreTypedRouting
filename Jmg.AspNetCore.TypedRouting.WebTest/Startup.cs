using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jmg.AspNetCore.TypedRouting.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTypedRouting<RootRouteValues, TestRouteFactory>();
			services.AddSingleton<TestUrlFactory>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseTypedRouting();

			app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Invalid route");
			});
		}
	}
}
