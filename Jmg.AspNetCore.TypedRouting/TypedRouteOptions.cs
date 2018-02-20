using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Options that apply to Typed Routes
	/// </summary>
	public enum TypedRouteOptions
	{
		/// <summary>
		/// No options
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates that the route will not serve endpoints, and so cannot be mapped into <see cref="PathString"/>
		/// </summary>
		IntermediateRoute = 1
	}
}
