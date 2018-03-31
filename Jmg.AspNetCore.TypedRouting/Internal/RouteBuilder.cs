using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jmg.AspNetCore.TypedRouting.Internal
{
	/// <summary>
	/// Factory for <see cref="RouteBuilder{TRootRouteValues, TRouteValues}"/>
	/// </summary>
	public static class RouteBuilder
	{
		/// <summary>
		/// Creates a <see cref="RouteBuilder{TRootRouteValues, TRouteValues}"/>
		/// </summary>
		/// <typeparam name="TRootRouteValues">Root route values</typeparam>
		/// <param name="pathFactory">Path factory</param>
		/// <returns>New typed route builder</returns>
		public static RouteBuilder<TRootRouteValues, TRootRouteValues> Create<TRootRouteValues>(ITypedRoutePathFactory<TRootRouteValues> pathFactory)
		{
			return new RouteBuilder<TRootRouteValues, TRootRouteValues>(pathFactory, null, TypedRouteOptions.None);
		}
	}

	/// <summary>
	/// Internal implementation of the route builder
	/// </summary>
	/// <typeparam name="TRootRouteValues">Root route values from the current Typed Routing middleware</typeparam>
	/// <typeparam name="TRouteValues">Route values that represent the current path</typeparam>
	public partial class RouteBuilder<TRootRouteValues, TRouteValues> : ITypedRouteBuilder<TRouteValues>
	{
		private readonly PathFactory<TRootRouteValues> pathFactory;
		private readonly Func<TRouteValues, PathString> pathFunc;
		private readonly TypedRouteOptions options;

		// Child routes
		private Dictionary<String, ILiteralContainer> pathEntries = new Dictionary<String, ILiteralContainer>();
		private IGuidContainer guidContainer;
		private INumberContainer numberContainer;

		internal RouteBuilder(ITypedRoutePathFactory<TRootRouteValues> pathFactory, Func<TRouteValues, PathString> pathFunc, TypedRouteOptions options)
		{
			this.pathFactory = (PathFactory<TRootRouteValues>)pathFactory;
			this.pathFunc = pathFunc;
			this.options = options;
		}

		private ITypedRoutingEndpoint<TRouteValues> endpoint;
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

		private ITypedRouteBuilder<TChildRouteValues> CreateChildBuilder<TChildRouteValues>(
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			Func<TChildRouteValues, String> suffixFunc,
			TypedRouteOptions options)
		{
			Func<TChildRouteValues, PathString> childPathFunc;
			if (this.pathFunc != null)
			{
				childPathFunc = (cv) => pathFunc(getParentRouteValues(cv)) + new PathString("/" + suffixFunc(cv));
			}
			else
			{
				childPathFunc = (cv) => new PathString("/" + suffixFunc(cv));
			}

			if (!options.HasFlag(TypedRouteOptions.IntermediateRoute))
			{
				this.pathFactory.AddPath(childPathFunc);
			}

			return new RouteBuilder<TRootRouteValues, TChildRouteValues>(this.pathFactory, childPathFunc, options);
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddLiteral<TChildRouteValues>(
			String literal,
			Func<TRouteValues, TChildRouteValues> getChildRouteValues,
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			TypedRouteOptions options)
		{
			if (this.pathEntries.ContainsKey(literal))
			{
				throw new InvalidOperationException($"A child route has already been created for this literal: {literal}");
			}

			var child = CreateChildBuilder(getParentRouteValues, cv => literal, options);
			this.pathEntries[literal] = new LiteralContainer<TChildRouteValues>(literal, getChildRouteValues, child);
			return child;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddInt32<TChildRouteValues>(
			Func<TRouteValues, Int32, TChildRouteValues> getChildRouteValues,
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			Func<TChildRouteValues, Int32> getLastValue,
			TypedRouteOptions options)
		{
			var child = CreateChildBuilder(getParentRouteValues, cv => getLastValue(cv).ToString(), options);
			this.numberContainer = new NumberContainer<TChildRouteValues>(getChildRouteValues, child);
			return child;
		}

		ITypedRouteBuilder<TChildRouteValues> ITypedRouteBuilder<TRouteValues>.AddGuid<TChildRouteValues>(
			Func<TRouteValues, Guid, TChildRouteValues> getChildRouteValues,
			Func<TChildRouteValues, TRouteValues> getParentRouteValues,
			Func<TChildRouteValues, Guid> getLastValue,
			TypedRouteOptions options)
		{
			var child = CreateChildBuilder(getParentRouteValues, cv => getLastValue(cv).ToString(), options);
			this.guidContainer = new GuidContainer<TChildRouteValues>(getChildRouteValues, child);
			return child;
		}

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
					var items = this.pathEntries.Select(i => i.Value.BuildMulti(i.Key));
					return new RouteHandlers.MultiLiteral<TRouteValues>(this.endpoint, items);
				}
			}
			else if (this.numberContainer != null)
			{
				return this.numberContainer.Build(this.endpoint);
			}
			else if (this.guidContainer != null)
			{
				return this.guidContainer.Build(this.endpoint);
			}
			else
			{
				return new RouteHandlers.Leaf<TRouteValues>(this.endpoint);
			}
		}
	}
}
