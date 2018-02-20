﻿using System;
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

		public ClientUserTaskRouteValues(Int32 clientId, Int32 userId, Guid taskId)
		{
			this.ClientId = clientId;
			this.UserId = userId;
			this.TaskId = taskId;
		}
	}
}
