using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jmg.AspNetCore.TypedRouting.Builder;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.Tests
{
	[TestClass]
	public class RouteTests
	{
		private class Invoker
		{
			private ITypedRouteHandler<RootRouteValues> handler;
			private RootRouteValues rootRouteValues;

			public Invoker(ITypedRouteBuilder<RootRouteValues> builder, RootRouteValues instance)
			{
				this.handler = builder.Build();
				this.rootRouteValues = instance;
			}

			public Task<Boolean> VisitAsync(ref int counter, PathString path)
			{
				counter++;
				return this.handler.TryInvokeAsync(null, this.rootRouteValues, path);
			}
		}

		[TestMethod]
		public async Task RouteTests_RootRoute()
		{
			var visitsRoot = 0;

			void AssertVisits()
			{
				Assert.AreEqual(0, visitsRoot);
			}

			var pathFactory = new Internal.PathFactory<RootRouteValues>();

			ITypedRouteBuilder<RootRouteValues> builder = Internal.RouteBuilder.Create(pathFactory);
			builder.AttachAction(i => --visitsRoot);

			var invoker = new Invoker(builder, RootRouteValues.Instance);

			AssertVisits();
			Assert.IsTrue(await invoker.VisitAsync(ref visitsRoot, "/"));
			AssertVisits();
		}

		[TestMethod]
		public async Task RouteTests_LiteralRoute()
		{
			var visitsRoot = 0;
			var visitsHome = 0;

			void AssertVisits()
			{
				Assert.AreEqual(0, visitsRoot);
				Assert.AreEqual(0, visitsHome);
			}

			var pathFactory = new Internal.PathFactory<RootRouteValues>();

			ITypedRouteBuilder<RootRouteValues> builder = Internal.RouteBuilder.Create(pathFactory);
			builder.AttachAction(i => --visitsRoot);

			var routeHome = builder.AddLiteral("Home", p => new HomeRouteValues(), c => RootRouteValues.Instance);
			routeHome.AttachAction(i => --visitsHome);


			var invoker = new Invoker(builder, RootRouteValues.Instance);

			AssertVisits();
			Assert.IsTrue(await invoker.VisitAsync(ref visitsRoot, "/"));
			AssertVisits();
			Assert.IsTrue(await invoker.VisitAsync(ref visitsHome, "/Home"));
			AssertVisits();
		}

		class HomeRouteValues
		{
		}
	}
}
