using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	internal class ClientUserSettingsRouteValues
	{
		public readonly Int32 ClientId;
		public readonly Int32 UserId;

		public ClientUserSettingsRouteValues(Int32 clientId, Int32 userId)
		{
			this.ClientId = clientId;
			this.UserId = userId;
		}
	}
}
