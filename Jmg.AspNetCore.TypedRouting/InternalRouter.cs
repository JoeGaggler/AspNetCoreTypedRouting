using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	internal interface IRouteHandler<TRouteValues>
	{
		Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues);
	}

	internal class InternalRouter<TRouteValues> :
		IRouteBuilder<TRouteValues>,
		IRouteHandler<TRouteValues>
	{
		// Child routes
		private Dictionary<String, IPathContainer> pathEntries = new Dictionary<String, IPathContainer>();
		private Dictionary<String, INumberContainer> numberEntries = new Dictionary<String, INumberContainer>();

		private IEndpoint<TRouteValues> endpoint;

		public InternalRouter()
		{

		}

		void IRouteBuilder<TRouteValues>.SetEndpoint(IEndpoint<TRouteValues> endpoint)
		{
			this.endpoint = endpoint;
		}

		IRouteBuilder<TChildRouteValues> IRouteBuilder<TRouteValues>.Add<TChildRouteValues>(String segment, Func<TRouteValues, TChildRouteValues> routeValuesFunc)
		{
			AssertNewSegment(segment);

			var nextRouteHandler = new InternalRouter<TChildRouteValues>();
			var container = new PathContainer<TChildRouteValues>(routeValuesFunc, nextRouteHandler);
			this.pathEntries[segment] = container;
			return nextRouteHandler;
		}

		IRouteBuilder<TChildRouteValues> IRouteBuilder<TRouteValues>.Add<TChildRouteValues>(String segment, Func<TRouteValues, Int32, TChildRouteValues> routeValuesFunc)
		{
			AssertNewSegment(segment);

			var nextRouteHandler = new InternalRouter<TChildRouteValues>();
			var container = new NumberContainer<TChildRouteValues>(routeValuesFunc, nextRouteHandler);
			this.numberEntries[segment] = container;
			return nextRouteHandler;
		}

		private void AssertNewSegment(String segment)
		{
			if (IsSegmentReserved(segment))
			{
				throw new InvalidOperationException($"Segment has already been reserved: {segment}");
			}
		}

		private Boolean IsSegmentReserved(String segment) =>
			this.pathEntries.ContainsKey(segment) ||
			this.numberEntries.ContainsKey(segment);

		async Task<Boolean> IRouteHandler<TRouteValues>.TryInvokeAsync(HttpContext httpContext, PathString path, TRouteValues values)
		{
			if (!path.HasValue || path == "/")
			{
				if (this.endpoint == null)
				{
					return false;
				}
				else
				{
					await this.endpoint.Run(httpContext, values);
					return true;
				}
			}

			if (!path.TryGetStartingSegment(out var prefix, out var suffix))
			{
				return false;
			}

			if (pathEntries.TryGetValue(prefix, out var pathContainer))
			{
				return await pathContainer.TryInvokeAsync(httpContext, suffix, values);
			}
			else if (numberEntries.TryGetValue(prefix, out var numberContainer))
			{
				if (!suffix.TryGetStartingSegment(out var numberPrefix, out var numberSuffix) ||
					!Int32.TryParse(numberPrefix, out var number))
				{
					return false;
				}

				return await numberContainer.TryInvokeAsync(httpContext, numberSuffix, values, number);
			}
			else
			{
				return false;
			}
		}

		private interface IPathContainer
		{
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues);
		}

		private class PathContainer<TChildRouteValues> : IPathContainer
		{
			private readonly IRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, TChildRouteValues> ChildRouteValuesFunc;

			public PathContainer(Func<TRouteValues, TChildRouteValues> childRouteValuesFunc, InternalRouter<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> IPathContainer.TryInvokeAsync(HttpContext httpContext, PathString right, TRouteValues leftValues)
			{
				var childValues = this.ChildRouteValuesFunc(leftValues);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, right, childValues);
			}
		}

		private interface INumberContainer
		{
			Task<Boolean> TryInvokeAsync(HttpContext httpContext, PathString pathString, TRouteValues routeValues, Int32 number);
		}

		private class NumberContainer<TChildRouteValues> : INumberContainer
		{
			private readonly IRouteHandler<TChildRouteValues> ChildRouteHandler;
			private readonly Func<TRouteValues, Int32, TChildRouteValues> ChildRouteValuesFunc;

			public NumberContainer(Func<TRouteValues, Int32, TChildRouteValues> childRouteValuesFunc, IRouteHandler<TChildRouteValues> childRouteHandler)
			{
				this.ChildRouteHandler = childRouteHandler;
				this.ChildRouteValuesFunc = childRouteValuesFunc;
			}

			Task<Boolean> INumberContainer.TryInvokeAsync(HttpContext httpContext, PathString right, TRouteValues leftValues, Int32 number)
			{
				var childValues = this.ChildRouteValuesFunc(leftValues, number);
				return this.ChildRouteHandler.TryInvokeAsync(httpContext, right, childValues);
			}
		}
	}
}
