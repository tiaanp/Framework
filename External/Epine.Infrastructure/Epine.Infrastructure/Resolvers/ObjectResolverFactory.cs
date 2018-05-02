using Epine.Infrastructure.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Epine.Infrastructure.Resolvers {
	internal static class ObjectResolverFactory {

		#region Constants

		private const string EXCEPTION_GETRESOLVER =
			"Epine.Infrastructure.Resolvers.ObjectResolverFactory:GetResolver [ An error occurred when retrieving a ResultResolver for type {0} ]";

		#endregion

		#region Class Methods

		internal static ObjectResolver<TResult> GetResolver<TResult>() {

			var response = default(ObjectResolver);
			
			try {
				response = ObjectResolverFactory._Resolvers[typeof(TResult)];
			}
			catch (Exception exception) {

				throw
					new InvalidOperationException(
							EXCEPTION_GETRESOLVER
								.FormatString(typeof(TResult).Name),
							exception);
			}

			return
				(ObjectResolver<TResult>)response;
		}

		#endregion

		#region Class Fields

		private static readonly IDictionary<Type, ObjectResolver> _Resolvers =
			new Dictionary<Type, ObjectResolver>() {				
				{ typeof(Missing),  new MissingObjectResolver() },
				{ typeof(String),  new StringObjectResolver() },
				{ typeof(JObject),  new JObjectObjectResolver() },
				{ typeof(JArray),  new JArrayObjectResolver() },
                { typeof(byte[]),  new ByteArrayObjectResolver() },
                  { typeof(List<JObject>),  new ListJObjectResolver() }
            };

		#endregion
	}
}
