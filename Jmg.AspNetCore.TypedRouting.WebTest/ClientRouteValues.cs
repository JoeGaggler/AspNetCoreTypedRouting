﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
	internal class ClientRouteValues
	{
		public readonly Int32 ClientId;

		public ClientRouteValues(Int32 clientId)
		{
			this.ClientId = clientId;
		}
	}
}
