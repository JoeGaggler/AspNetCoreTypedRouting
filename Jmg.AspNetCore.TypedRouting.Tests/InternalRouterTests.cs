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
	public class InternalRouterTests
	{
		[TestMethod]
		public async Task InternalRouter_SingleRoute_StringSegment_Test()
		{
			var router = new TypedRouter();
			var rootRoute = router.RootRoute;
			ITypedRouteBuilder<RootRouteValues> builder = rootRoute;
			ITypedRouteHandler<RootRouteValues> handler = rootRoute;

			var folder1Route = builder.AddLiteral("Folder1", (_) => new StringRouteValues("Folder1"), (s) => RootRouteValues.Instance);

			Boolean didRun = false;
			folder1Route.AttachAction(rv => didRun = true);
			var found = await handler.TryInvokeAsync(null, RootRouteValues.Instance, "/Folder1");
			Assert.IsTrue(found, "Endpoint not found");
			Assert.IsTrue(didRun, "Endpoint did not run");
		}

		public class StringRouteValues
		{
			public String Segment { get; }

			public StringRouteValues(String segment)
			{
				this.Segment = segment;
			}
		}
	}
}
