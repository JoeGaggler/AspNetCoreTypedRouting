using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
    public class TypedRouteBuilder : ITypedRouteBuilder
    {
		void ITypedRouteBuilder.Configure(IRouteBuilder<RootRouteValues> builder)
		{
			var clientRoute = builder.Add("Client", (root, clientId) => new ClientRouteValues(clientId));
			var clientUserRoute = clientRoute.Add("User", (client, userId) => new ClientUserRouteValues(client, userId));

			clientRoute.SetEndpoint(new ClientEndpoint());
		}
	}
}
