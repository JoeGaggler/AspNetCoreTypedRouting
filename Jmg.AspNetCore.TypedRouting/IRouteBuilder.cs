using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
    public interface IRouteBuilder<TRouteValues>
    {
		IRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, TChildRouteValues> func);

		IRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, Int32, TChildRouteValues> func);

		void SetEndpoint(IEndpoint<TRouteValues> endpoint);
	}
}
