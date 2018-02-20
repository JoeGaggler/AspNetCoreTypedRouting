using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.WebTest
{
    internal class TestUrlFactory
    {
		private readonly ITypedRoutePathFactory<RootRouteValues> factory;

		public TestUrlFactory(ITypedRoutePathFactory<RootRouteValues> pathFactory)
		{
			this.factory = pathFactory;
		}

		public PathString Client(ClientRouteValues values) => factory.GetPath(values);
		public PathString ClientUser(ClientUserRouteValues values) => factory.GetPath(values);
		public PathString ClientUserTask(ClientUserTaskRouteValues values) => factory.GetPath(values);
		public PathString ClientUserSettings(ClientUserSettingsRouteValues values) => factory.GetPath(values);
	}
}
