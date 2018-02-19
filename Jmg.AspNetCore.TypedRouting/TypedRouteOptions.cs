using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	public enum TypedRouteOptions
	{
		None = 0,

		/// <summary>
		/// Indicates that the route will not serve endpoints.
		/// </summary>
		IntermediateRoute = 1
	}
}
