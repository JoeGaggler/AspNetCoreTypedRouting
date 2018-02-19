using System;
using System.Collections.Generic;
using System.Text;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Interface for a type that can configure a Typed Route Builder
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values</typeparam>
    public interface ITypedRouteFactory<TRootRouteValues>
    {
		void Configure(TypedRouter typedRouter);
	}
}
