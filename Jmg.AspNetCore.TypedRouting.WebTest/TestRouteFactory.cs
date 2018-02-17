using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
    public class TestRouteFactory : ITypedRouteFactory<RootRouteValues>
    {
		public void Configure(ITypedRouteBuilder<RootRouteValues> root)
		{
			var clientRoute = root.Add("Client", (_, clientId) => new ClientRouteValues(clientId));
			var clientUserRoute = clientRoute.Add("User", (client, userId) => new ClientUserRouteValues(client, userId));
			var clientUserTaskRoute = clientUserRoute.Add("Task", (user, taskId) => new ClientUserTaskRouteValues(user, taskId));
			var clientUserPassword = clientUserRoute.Add("Settings", (user) => user);

			clientRoute.Endpoint = new ClientEndpoint();
			clientUserTaskRoute.Endpoint = new ClientEndpoint();
		}
	}
}
