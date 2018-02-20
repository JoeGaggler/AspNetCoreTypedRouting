using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jmg.AspNetCore.TypedRouting.Extensions;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
    internal class TestRouteFactory : ITypedRouteFactory<RootRouteValues>
    {
		private readonly ITypedRoutePathFactory<RootRouteValues> pathFactory;
		private readonly TestUrlFactory urlFactory;

		RootRouteValues ITypedRouteFactory<RootRouteValues>.RootRouteValues => RootRouteValues.Instance;

		public TestRouteFactory(ITypedRoutePathFactory<RootRouteValues> pathFactory, TestUrlFactory urlFactory)
		{
			this.pathFactory = pathFactory;
			this.urlFactory = urlFactory;
		}

		void ITypedRouteFactory<RootRouteValues>.Configure(ITypedRouteBuilder<RootRouteValues> root)
		{
			var clientRoute = root.AddNamedNumber("Client", (_, clientId) => new ClientRouteValues(clientId), (c) => RootRouteValues.Instance, (c) => c.ClientId);
			var clientUserRoute = clientRoute.AddNamedNumber("User", (client, userId) => new ClientUserRouteValues(client.ClientId, userId), (c) => new ClientRouteValues(c.ClientId), (c) => c.UserId);
			var clientUserTaskRoute = clientUserRoute.AddNamedGuid("Task", (user, taskId) => new ClientUserTaskRouteValues(user.ClientId, user.UserId, taskId), (c) => new ClientUserRouteValues(c.ClientId, c.UserId), (c) => c.TaskId);
			var clientUserSettingsRoute = clientUserRoute.AddLiteral("Settings", (user) => new ClientUserSettingsRouteValues(user.ClientId, user.UserId), (c) => new ClientUserRouteValues(c.ClientId, c.UserId));
			
			clientRoute.Endpoint = new StringEndpoint<ClientRouteValues>(c => $"Client: {c.ClientId} - {urlFactory.Client(c)}");
			clientUserRoute.Endpoint = new StringEndpoint<ClientUserRouteValues>(c => $"Client User: {c.ClientId} {c.UserId} - {pathFactory.GetPath(c)}");
			clientUserTaskRoute.Endpoint = new StringEndpoint<ClientUserTaskRouteValues>(c => $"Client User Task: {c.ClientId} {c.UserId} {c.TaskId} - {pathFactory.GetPath(c)}");
			clientUserSettingsRoute.Endpoint = new StringEndpoint<ClientUserSettingsRouteValues>(c => $"Client User Settings: {c.ClientId} {c.UserId} - {pathFactory.GetPath(c)}");
		}
	}
}
