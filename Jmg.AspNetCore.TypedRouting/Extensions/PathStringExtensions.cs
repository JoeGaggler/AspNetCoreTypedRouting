using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting.Extensions
{
	internal static class PathStringExtensions
	{
		public static Boolean TryGetStartingSegment(this PathString pathString, out String root, out PathString remainder)
		{
			if (pathString == PathString.Empty)
			{
				root = null;
				remainder = null;
				return false;
			}

			var path = pathString.ToString();
			var i = path.IndexOf('/', 1);
			if (i == -1)
			{
				root = path.ToString().Substring(1);
				remainder = null;
			}
			else
			{
				root = path.Substring(1, i - 1);
				var nextIndex = i;
				remainder = path.Substring(nextIndex);
			}

			return true;
		}
	}
}
