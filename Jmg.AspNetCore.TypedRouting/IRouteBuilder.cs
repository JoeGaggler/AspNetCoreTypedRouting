using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can construct a routing table by adding new route values
	/// </summary>
	/// <typeparam name="TRouteValues">Route values that describes the base route</typeparam>
    public interface IRouteBuilder<TRouteValues>
    {
		IRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, TChildRouteValues> func);

		IRouteBuilder<TChildRouteValues> Add<TChildRouteValues>(String segment, Func<TRouteValues, Int32, TChildRouteValues> func);

		void SetEndpoint(IEndpoint<TRouteValues> endpoint);
	}
}
