using Epine.Infrastructure.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Epine.Infrastructure.Resolvers {
	internal static class StreamReaderResolverFactory {

		#region Constants

		private const string EXCEPTION_GETRESOLVER =
			"Epine.Infrastructure.Resolvers.StreamReaderResolverFactory:GetResolver [ An error occurred when retrieving a ResultResolver for type {0} ]";

		#endregion

		#region Class Methods

		internal static StreamReaderResolver<TResult> GetResolver<TResult>() {

			var response = default(StreamReaderResolver);
			
			try {
				response = StreamReaderResolverFactory._Resolvers[typeof(TResult)];
			}
			catch (Exception exception) {

				throw
					new InvalidOperationException(
							EXCEPTION_GETRESOLVER
								.FormatString(typeof(TResult).Name),
							exception);
			}

			return
				(StreamReaderResolver<TResult>)response;
		}

		#endregion

		#region Class Fields

		private static readonly IDictionary<Type, StreamReaderResolver> _Resolvers =
			new Dictionary<Type, StreamReaderResolver>() {
				{ typeof(JObject),  new JObjectStreamReaderResolver() },
				{ typeof(Missing),  new MissingStreamReaderResolver() },
				{ typeof(String),  new StringStreamReaderResolver() },
				{ typeof(JArray),  new JArrayStreamReaderResolver() }
			};

		#endregion
	}
}
