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
			var router = new InternalRouter<RootRouteValues>();
			ITypedRouteBuilder<RootRouteValues> builder = router;
			ITypedRouteHandler<RootRouteValues> handler = router;

			var folder1Route = builder.Add("Folder1", (_) => ("Folder1"));

			Boolean didRun = false;
			folder1Route.AttachAction(rv => didRun = true);
			var found = await handler.TryInvokeAsync(null, RootRouteValues.Instance, "/Folder1");
			Assert.IsTrue(found, "Endpoint not found");
			Assert.IsTrue(didRun, "Endpoint did not run");
		}
	}
}
