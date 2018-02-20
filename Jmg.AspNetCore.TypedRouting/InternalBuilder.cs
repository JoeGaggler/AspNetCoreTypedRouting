using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting
{
	/// <summary>
	/// Builds and handles routes
	/// </summary>
	/// <typeparam name="TRouteValues">Route values that represent the current path</typeparam>
	internal partial class InternalBuilder<TRootRouteValues, TRouteValues> : ITypedRouteBuilder<TRouteValues>
	{
		private readonly TypedRoutePathFactory<TRootRouteValues> pathBuilder;
		private readonly Func<TRouteValues, PathString> pathFunc;
		private readonly TypedRouteOptions options;

		// Child routes
		private Dictionary<String, ILiteralContainer> pathEntries = new Dictionary<String, ILiteralContainer>();
		private IGuidContainer guidContainer;
		private INumberContainer numberContainer;

		private ITypedRoutingEndpoint<TRouteValues> endpoint;

		public InternalBuilder(TypedRoutePathFactory<TRootRouteValues> pathBuilder, Func<TRouteValues, PathString> pathFunc, TypedRouteOptions options)
		{
			this.pathBuilder = pathBuilder;
			this.pathFunc = pathFunc;
			this.options = options;
		}

		ITypedRoutingEndpoint<TRouteValues> ITypedRouteBuilder<TRouteValues>.Endpoint
		{
			get => this.endpoint;
			set
			{
				if (this.options.HasFlag(TypedRouteOptions.IntermediateRoute))
				{
					throw new InvalidOperationException("Intermediate routes cannot have endpoints.");
				}

				this.endpoint = value;
			}
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddLiteral<TChildRouteValues>(String literal, Func<TRouteValues, TChildRouteValues> func, Func<TChildRouteValues, TRouteValues> reverseFunc, TypedRouteOptions options)
		{
			AssertNewSegment(literal);

			Func<TChildRouteValues, PathString> childPathFunc;
			if (this.pathFunc != null)
			{
				childPathFunc = (cv) => pathFunc(reverseFunc(cv)) + new PathString("/" + literal);
			}
			else
			{
				childPathFunc = (cv) => new PathString("/" + literal);
			}

			if (!options.HasFlag(TypedRouteOptions.IntermediateRoute))
			{
				this.pathBuilder.AddPath(childPathFunc);
			}

			var nextRouteHandler = new InternalBuilder<TRootRouteValues, TChildRouteValues>(this.pathBuilder, childPathFunc, options);
			this.pathEntries[literal] = new LiteralContainer<TChildRouteValues>(literal, func, nextRouteHandler);
			return nextRouteHandler;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddInt32<TChildRouteValues>(Func<TRouteValues, Int32, TChildRouteValues> func, Func<TChildRouteValues, TRouteValues> reverseFunc, Func<TChildRouteValues, Int32> split, TypedRouteOptions options)
		{
			Func<TChildRouteValues, PathString> childPathFunc;
			if (this.pathFunc != null)
			{
				childPathFunc = (cv) => pathFunc(reverseFunc(cv)) + new PathString("/" + split(cv));
			}
			else
			{
				childPathFunc = (cv) => new PathString("/" + split(cv));
			}

			if (!options.HasFlag(TypedRouteOptions.IntermediateRoute))
			{
				this.pathBuilder.AddPath(childPathFunc);
			}

			var nextRouteHandler = new InternalBuilder<TRootRouteValues, TChildRouteValues>(this.pathBuilder, childPathFunc, options);
			this.numberContainer = new NumberContainer<TChildRouteValues>(func, nextRouteHandler);
			return nextRouteHandler;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddGuid<TChildRouteValues>(Func<TRouteValues, Guid, TChildRouteValues> func, Func<TChildRouteValues, TRouteValues> reverseFunc, Func<TChildRouteValues, Guid> split, TypedRouteOptions options)
		{
			Func<TChildRouteValues, PathString> childPathFunc;
			if (this.pathFunc != null)
			{
				childPathFunc = (cv) => pathFunc(reverseFunc(cv)) + new PathString("/" + split(cv));
			}
			else
			{
				childPathFunc = (cv) => new PathString("/" + split(cv));
			}

			if (!options.HasFlag(TypedRouteOptions.IntermediateRoute))
			{
				this.pathBuilder.AddPath(childPathFunc);
			}

			var nextRouteHandler = new InternalBuilder<TRootRouteValues, TChildRouteValues>(this.pathBuilder, childPathFunc, options);
			this.guidContainer = new GuidContainer<TChildRouteValues>(func, nextRouteHandler);
			return nextRouteHandler;
		}

		private void AssertNewSegment(String segment)
		{
			if (IsSegmentReserved(segment))
			{
				throw new InvalidOperationException($"Segment has already been reserved: {segment}");
			}
		}

		private Boolean IsSegmentReserved(String segment) => this.pathEntries.ContainsKey(segment);

		ITypedRouteHandler<TRouteValues> ITypedRouteBuilder<TRouteValues>.Build()
		{
			var pathCount = this.pathEntries.Count;
			if (pathCount > 0)
			{
				if (pathCount == 1)
				{
					var first = this.pathEntries.First();
					var literal = first.Key;
					var entry = first.Value;
					return entry.BuildSingle(literal, endpoint);
				}
				else
				{
					IEnumerable<RouteHandlers.IMultiLiteralContainer<TRouteValues>> items = this.pathEntries.Select(i => i.Value.BuildMulti(i.Key));
					return new RouteHandlers.MultiLiteral<TRouteValues>(this.endpoint, items);
				}
			}

			if (this.numberContainer != null)
			{
				return this.numberContainer.Build(this.endpoint);
			}

			if (this.guidContainer != null)
			{
				return this.guidContainer.Build(this.endpoint);
			}

			return new RouteHandlers.Leaf<TRouteValues>(this.endpoint);
		}
	}
}
