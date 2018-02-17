using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	internal class ClientUserTaskRouteValues
	{
		public readonly Int32 ClientId;
		public readonly Int32 UserId;
		public readonly Guid TaskId;

		public ClientUserTaskRouteValues(ClientUserRouteValues user, Guid taskId)
		{
			this.ClientId = user.ClientId;
			this.UserId = user.UserId;
			this.TaskId = taskId;
		}
	}
}
