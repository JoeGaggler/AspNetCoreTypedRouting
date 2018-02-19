using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
    public class TestRouteFactory : ITypedRouteFactory<RootRouteValues>
    {
		RootRouteValues ITypedRouteFactory<RootRouteValues>.RootRouteValues => RootRouteValues.Instance;

		void ITypedRouteFactory<RootRouteValues>.Configure(ITypedRouteBuilder<RootRouteValues> root)
		{
			var clientRoute = root.AddNamedNumber("Client", (_, clientId) => new ClientRouteValues(clientId), (c) => RootRouteValues.Instance, (c) => c.ClientId);
			var clientUserRoute = clientRoute.AddNamedNumber("User", (client, userId) => new ClientUserRouteValues(client, userId), (c) => new ClientRouteValues(c.ClientId), (c) => c.UserId);
			var clientUserTaskRoute = clientUserRoute.AddNamedGuid("Task", (user, taskId) => new ClientUserTaskRouteValues(user, taskId), (c) => new ClientUserRouteValues(c.ClientId, c.UserId), (c) => c.TaskId);
			var clientUserPassword = clientUserRoute.AddLiteral("Settings", p => (p, 1), c => c.Item1);

			// TODO: Get links to work again
			clientRoute.Endpoint = new StringEndpoint<ClientRouteValues>(c => $"Client: {c.ClientId} - {"typedRouter.GetPath(c)"}");
			clientUserRoute.Endpoint = new StringEndpoint<ClientUserRouteValues>(c => $"Client User: {c.ClientId} {c.UserId} - {"typedRouter.GetPath(c)"}");
			clientUserTaskRoute.Endpoint = new StringEndpoint<ClientUserTaskRouteValues>(c => $"Client User Task: {c.ClientId} {c.UserId} {c.TaskId} - {"typedRouter.GetPath(c)"}");
		}
	}
}
