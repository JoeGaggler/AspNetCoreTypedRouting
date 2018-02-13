using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jmg.AspNetCore.TypedRouting.Builder;
using System;
using System.Collections.Generic;

namespace Jmg.AspNetCore.TypedRouting.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var applicationBuilder = new MyApplicationBuilder();

			//var next = applicationBuilder.UseTypedRouting(root =>
			//{
			//	var clientRouter = root.Add("Client", (_, clientId) => new ClientRouteValues(clientId));
			//	var userRouter = clientRouter.Add("User", (c, userId) => new ClientUserRouteValues(c, userId));
			//});
		}

		internal class ClientRouteValues
		{
			public readonly Int32 ClientId;

			public ClientRouteValues(Int32 clientId)
			{
				this.ClientId = clientId;
			}
		}

		internal class ClientUserRouteValues
		{
			public readonly Int32 ClientId;
			public readonly Int32 UserId;

			public ClientUserRouteValues(ClientRouteValues clientRouteValues, Int32 userId)
			{
				this.ClientId = clientRouteValues.ClientId;
				this.UserId = userId;
			}
		}
	}
}
