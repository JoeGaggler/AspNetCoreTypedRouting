using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Represents route values for the root path
	/// </summary>
	public class RootRouteValues
	{
		public static RootRouteValues Instance = new RootRouteValues();

		private RootRouteValues()
		{

		}
	}
}
