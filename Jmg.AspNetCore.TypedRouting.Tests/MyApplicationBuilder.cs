using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Tests
{
	class MyApplicationBuilder : IApplicationBuilder
	{
		private IServiceProvider serviceProvider;

		public MyApplicationBuilder()
		{
			this.serviceProvider = new MyServiceProvider();
		}

		public IServiceProvider ApplicationServices { get => this.serviceProvider; set => this.serviceProvider = value; }

		public IFeatureCollection ServerFeatures => throw new NotImplementedException();

		public IDictionary<String, Object> Properties => throw new NotImplementedException();

		public RequestDelegate Build()
		{
			return null;
		}

		public IApplicationBuilder New()
		{
			return this;
		}

		public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
		{
			return this;
		}
	}

	class MyServiceProvider : IServiceProvider
	{
		public Object GetService(Type serviceType)
		{
			return null;
		}
	}
}
