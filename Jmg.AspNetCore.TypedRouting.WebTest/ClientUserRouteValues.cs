using System;
using Microsoft.AspNetCore.Http;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	internal class ClientUserRouteValues
	{
		public readonly Int32 ClientId;
		public readonly Int32 UserId;

		public ClientUserRouteValues(Int32 clientId, Int32 userId)
		{
			this.ClientId = clientId;
			this.UserId = userId;
		}

		public ClientUserRouteValues(ClientRouteValues client, Int32 userId)
		{
			this.ClientId = client.ClientId;
			this.UserId = userId;
		}
	}
}