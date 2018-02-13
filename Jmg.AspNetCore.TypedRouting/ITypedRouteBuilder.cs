using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	

    public interface ITypedRouteBuilder
    {
		void Configure(IRouteBuilder<RootRouteValues> builder);
	}
}
