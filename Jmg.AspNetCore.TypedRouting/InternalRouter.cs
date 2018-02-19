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
	public partial class InternalRouter<TRouteValues> : ITypedRouteBuilder<TRouteValues>
	{
		//private readonly TypedRouter typedRouter;
		private readonly Func<TRouteValues, PathString> pathFunc;
		private readonly TypedRouteOptions options;

		// Child routes
		private Dictionary<String, ILiteralContainer> pathEntries = new Dictionary<String, ILiteralContainer>();
		private IGuidContainer guidContainer;
		private INumberContainer numberContainer;

		private ITypedRoutingEndpoint<TRouteValues> endpoint;

		public InternalRouter(/*TypedRouter typedRouter,*/ Func<TRouteValues, PathString> pathFunc, TypedRouteOptions options)
		{
			//this.typedRouter = typedRouter;
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
				//this.typedRouter.pathFuncMap[typeof(TChildRouteValues)] = childPathFunc;
			}

			var nextRouteHandler = new InternalRouter<TChildRouteValues>(/*this.typedRouter,*/ childPathFunc, options);
			var container = new LiteralContainer<TChildRouteValues>(literal, func, nextRouteHandler);
			this.pathEntries[literal] = container;
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
				//this.typedRouter.pathFuncMap[typeof(TChildRouteValues)] = childPathFunc;
			}

			var nextRouteHandler = new InternalRouter<TChildRouteValues>(/*this.typedRouter,*/ childPathFunc, options);
			var container = new NumberContainer<TChildRouteValues>(func, nextRouteHandler);
			this.numberContainer = container;
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
				//this.typedRouter.pathFuncMap[typeof(TChildRouteValues)] = childPathFunc;
			}

			var nextRouteHandler = new InternalRouter<TChildRouteValues>(/*this.typedRouter,*/ childPathFunc, options);
			var container = new GuidContainer<TChildRouteValues>(func, nextRouteHandler);
			this.guidContainer = container;
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
			if (this.pathEntries.Count == 1 && this.numberContainer == null && this.guidContainer == null)
			{
				var first = this.pathEntries.First();
				var literal = first.Key;
				var entry = first.Value;
				return entry.Build(literal, endpoint);
			}

			if (this.numberContainer != null)
			{
				return this.numberContainer.Build(this.endpoint);
			}

			return new RouteHandlers.Leaf<TRouteValues>(this.endpoint);
		}
	}
}
