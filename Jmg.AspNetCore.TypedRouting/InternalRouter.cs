using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Builds and handles routes
	/// </summary>
	/// <typeparam name="TRouteValues">Route values that represent the current path</typeparam>
	public partial class InternalRouter<TRouteValues> :
		ITypedRouteBuilder<TRouteValues>,
		ITypedRouteHandler<TRouteValues>
	{
		// Child routes
		private Dictionary<String, IPathContainer> pathEntries = new Dictionary<String, IPathContainer>();
		private Dictionary<String, INumberContainer> numberEntries = new Dictionary<String, INumberContainer>();
		private Dictionary<String, IGuidContainer> guidEntries = new Dictionary<String, IGuidContainer>();

		private ITypedRoutingEndpoint<TRouteValues> endpoint;


		public InternalRouter()
		{

		}

		ITypedRoutingEndpoint<TRouteValues> ITypedRouteBuilder<TRouteValues>.Endpoint { get => this.endpoint; set => this.endpoint = value; }

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.Add<TChildRouteValues>(String segment, Func<TRouteValues, TChildRouteValues> func)
		{
			AssertNewSegment(segment);

			var nextRouteHandler = new InternalRouter<TChildRouteValues>();
			var container = new PathContainer<TChildRouteValues>(func, nextRouteHandler);
			this.pathEntries[segment] = container;
			return nextRouteHandler;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.Add<TChildRouteValues>(String segment, Func<TRouteValues, Int32, TChildRouteValues> func)
		{
			AssertNewSegment(segment);

			var nextRouteHandler = new InternalRouter<TChildRouteValues>();
			var container = new NumberContainer<TChildRouteValues>(func, nextRouteHandler);
			this.numberEntries[segment] = container;
			return nextRouteHandler;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.Add<TChildRouteValues>(String segment, Func<TRouteValues, Guid, TChildRouteValues> func)
		{
			AssertNewSegment(segment);

			var nextRouteHandler = new InternalRouter<TChildRouteValues>();
			var container = new GuidContainer<TChildRouteValues>(func, nextRouteHandler);
			this.guidEntries[segment] = container;
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

		async Task<Boolean> ITypedRouteHandler<TRouteValues>.TryInvokeAsync(HttpContext httpContext, TRouteValues values, PathString path)
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
				return await pathContainer.TryInvokeAsync(httpContext, values, suffix);
			}
			else if (numberEntries.TryGetValue(prefix, out var numberContainer))
			{
				if (!suffix.TryGetStartingSegment(out var keyPrefix, out var keySuffix) ||
					!Int32.TryParse(keyPrefix, out var key))
				{
					return false;
				}

				return await numberContainer.TryInvokeAsync(httpContext, values, key, keySuffix);
			}
			else if (guidEntries.TryGetValue(prefix, out var guidContainer))
			{
				if (!suffix.TryGetStartingSegment(out var keyPrefix, out var keySuffix) ||
					!Guid.TryParse(keyPrefix, out var key))
				{
					return false;
				}

				return await guidContainer.TryInvokeAsync(httpContext, values, key, keySuffix);
			}
			else
			{
				return false;
			}
		}
	}
}
