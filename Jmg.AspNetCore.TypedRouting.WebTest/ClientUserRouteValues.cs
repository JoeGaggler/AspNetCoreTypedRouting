using System;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	internal class ClientUserRouteValues
	{
		public readonly Int32 ClientId;
		public readonly Int32 UserId;

		public ClientUserRouteValues(ClientRouteValues client, System.Int32 userId)
		{
			this.ClientId = client.ClientId;
			this.UserId = userId;
		}
	}
}