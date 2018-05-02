using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="JObject"/>-based objects.
	/// </summary>
	public static class JsonExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="property"></param>
		public static void AddUpdate(this JObject jObject, JProperty property) {
			jObject.AddUpdate(property, ValueStrategies.None, default(JProperty));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="property"></param>
		/// <param name="valueStrategy"></param>
		/// <param name="defaultValue"></param>
		public static void AddUpdate(this JObject jObject, JProperty property, ValueStrategies valueStrategy, JProperty defaultValue) {

			if (jObject.SelectToken(property.Name) != null) {
				if (!valueStrategy.HasFlag(ValueStrategies.PreserveOriginal)) {
					var value = jObject.SelectToken(property.Name).Parent.As<JProperty>();
					value.Value =
						JsonExtensions.ResolverValue(valueStrategy, property, value).Value;
				}

			} else {
				jObject.Add(JsonExtensions.ResolverValue(valueStrategy, property, defaultValue));
			}
		}
		private static JProperty ResolverValue(ValueStrategies valueStrategy, JProperty value, JProperty otherValue) {
			var response = value;


			if (valueStrategy.HasFlag(ValueStrategies.Force)) {
				response.Value = otherValue.Value;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.ForceOnEmpty)) {
				response.Value =
					((string)response.Value).IsNullOrEmpty()
						? otherValue.Value
						: value.Value;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Sum)) {
				response.Value =
					Convert.ToString(
						Convert.ToDouble(
							(response.Value.IsNotNullOrEmpty() ? response.Value : "0"), System.Globalization.CultureInfo.InvariantCulture) +
						Convert.ToDouble(
							(otherValue.Value.IsNotNullOrEmpty() ? otherValue.Value : "0"), System.Globalization.CultureInfo.InvariantCulture), System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Merge)) {
				if (value.Value.Type == JTokenType.Array && otherValue.Value.Type == JTokenType.Array) {
					((JArray)otherValue.Value).Children().ForEachElement(child => ((JArray)response.Value).Add(child));
				}
				else {
					response.Value = String.Concat(response.Value, otherValue.Value);
				}
			}

			return response;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void AddUpdate(this JObject jObject, string name, JToken value) {
			jObject.AddUpdate(name, value, ValueStrategies.None, default(JToken));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="valueStrategy"></param>
		/// <param name="defaultValue"></param>
		public static void AddUpdate(this JObject jObject, string name, JToken value, ValueStrategies valueStrategy, JToken defaultValue) {

			if (jObject.SelectToken(name) != null)
			{
				if (!valueStrategy.HasFlag(ValueStrategies.PreserveOriginal)) {
					jObject.SelectToken(name)
						.Parent
						.As<JProperty>()
						.Value =
						JsonExtensions
							.ResolverValue(
								valueStrategy,
								value,
								jObject.SelectToken(name).Parent.As<JProperty>().Value);
				}

			} else {
				jObject.Add(name, JsonExtensions.ResolverValue(valueStrategy, value, defaultValue));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="valueStrategy"></param>
		/// <param name="value"></param>
		/// <param name="otherValue"></param>
		/// <returns></returns>
		private static JToken ResolverValue(ValueStrategies valueStrategy, JToken value, JToken otherValue)
		{
			var response = value;

			if (valueStrategy.HasFlag(ValueStrategies.Force)) {
				response = otherValue;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.ForceOnEmpty)) {
				response =
					((string)response).IsNullOrEmpty()
						? otherValue
						: value;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Sum)) {
				response = 
					Convert.ToString(
						Convert.ToDouble(
							(response.IsNotNullOrEmpty() 
								? response 
								: "0"), 
							System.Globalization.CultureInfo.InvariantCulture) + 
						
						Convert.ToDouble(
							(otherValue.IsNotNullOrEmpty() 
								? otherValue 
								: "0"), 
							System.Globalization.CultureInfo.InvariantCulture), System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Merge)) {
				if (response.Type == JTokenType.Array) {
					if (otherValue.Type == JTokenType.Array) {
						((JArray)otherValue).Children().ForEachElement(child => ((JArray)response).Add(child));
					}
					else if (otherValue.HasValues) {
						((JArray)response).Add(otherValue);
					}
				}
				else {
					response = String.Concat(value, otherValue);
				}
			}

			return response;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void AddUpdate(this JObject jObject, string name, string value) {
			jObject.AddUpdate(name, value, ValueStrategies.None, String.Empty);
		}

		/// <summary>
		///		Adds/Updates a <see cref="JProperty"/> contained in 
		///		the parameter <paramref name="jObject"/>.
		/// </summary>
		/// <param name="jObject">
		///		A <see cref="JObject"/> instance to add/update.
		/// </param>
		/// <param name="name">
		///		The name of the <see cref="JProperty"/> to add/update 
		///		to parameter <paramref name="jObject"/>.
		/// </param>
		/// <param name="value">
		///		The value of the <see cref="JProperty"/> to add/update 
		///		to parameter <paramref name="jObject"/>.
		/// </param>
		/// <param name="valueStrategy">
		///		The <see cref="ValueStrategies"/> to apply when adding/updating 
		///		a <see cref="JProperty"/> on parameter <paramref name="jObject"/>.
		/// </param>
		/// <param name="defaultValue">
		///		A default value to be used in conjunction with the given <see cref="ValueStrategies"/> 
		///		when adding/updating parameter <paramref name="jObject"/>.
		/// </param>
		public static void AddUpdate(this JObject jObject, string name, string value, ValueStrategies valueStrategy, string defaultValue) {

			if (jObject.SelectToken(name) != null) {
				if (!valueStrategy.HasFlag(ValueStrategies.PreserveOriginal)) {
					var innerValue = (string)jObject.SelectToken(name).Parent.As<JProperty>().Value;
					jObject.SelectToken(name)
						.Parent
						.As<JProperty>()
						.Value =
							JsonExtensions
								.ResolverValue(
									valueStrategy,
									value,
									(valueStrategy == ValueStrategies.Force ? value : innerValue));
				}
			} else {
				jObject.Add(
					name,
					JsonExtensions
						.ResolverValue(
							valueStrategy,
							value,
							defaultValue));
			}
		}
		private static string ResolverValue(ValueStrategies valueStrategy, string value, string otherValue) {
			var response = value;

			if (valueStrategy.HasFlag(ValueStrategies.Force)) {
				response = otherValue;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.ForceOnEmpty)) {
				response =
					((string)response).IsNullOrEmpty()
						? otherValue
						: value;
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Sum))
			{

                response = 
					Convert.ToString(
						Convert.ToDouble(
							(response.IsNotNullOrEmpty() ? response : "0"), 
							System.Globalization.CultureInfo.InvariantCulture) + 
							
						Convert.ToDouble(
							(otherValue.IsNotNullOrEmpty() ? otherValue : "0"), 
							System.Globalization.CultureInfo.InvariantCulture), System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (valueStrategy.HasFlag(ValueStrategies.Merge)) {
				response = String.Concat(value, otherValue);
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jObject"></param>
		/// <param name="properties"></param>
		public static void AddUpdate(this JObject jObject, IEnumerable<JProperty> properties) {
			properties
				.ForEachElement(
					property => {
						if (property.Children().IsNotNullOrEmpty() && property.Children().First().HasValues) {
							if (jObject.SelectToken(property.Name) == null) {
								jObject.Add(property);
							} else {
								jObject
									.SelectToken(property.Name)
									.As<JObject>()
									.AddUpdate(
										property
											.Value
											.Children()
											.Cast<JProperty>());
							}
						} else {
							jObject.AddUpdate(property);
						}
					});
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="paths"></param>
		/// <param name="removeEmpty"></param>
		/// <returns></returns>
		public static string SelectPathData(this JObject data, IEnumerable<string> paths, bool removeEmpty) {
			return
				new JObject(
					paths
						.Where(
							property =>
								(property.Contains("[*]")
									? data.SelectTokens(property).IsNotNullOrEmpty()
									: data.SelectToken(property) != null) &&
								(removeEmpty
									? data.SelectToken(property).ToString().IsNotNullOrEmpty()
									: true))
						.Select(
							property => {
								var response = default(JProperty);

								if (property.Contains("[*]")) {
									if (property.LastIndexOf("]") == property.Length - 1) {
										response =
											data.SelectTokens(property)
												.First()
												.Parent
												.Parent
												.As<JProperty>();
									} else {
										response =
											data.SelectTokens(property)
												.First()
												.Parent
												.As<JProperty>();
									}

								} else {
									response =
										data.SelectToken(property)
											.Parent
											.As<JProperty>();
								}

								return response;
							}))
						.ToStringNone();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="paths"></param>
		/// <param name="delimiter"></param>
		/// <param name="removeEmpty"></param>
		/// <returns></returns>
		public static string JoinPathData(this JObject data, IEnumerable<string> paths, string delimiter, bool removeEmpty) {
			return
				paths
					.Where(
						property =>
							(property.Contains("[*]")
								? data.SelectTokens(property).IsNotNullOrEmpty()
								: data.SelectToken(property) != null) &&
							(removeEmpty
								? data.SelectToken(property).ToString().IsNotNullOrEmpty()
								: true))
					.Select(
						property => {
							var response = default(JProperty);

							if (property.Contains("[*]")) {
								if (property.LastIndexOf("]") == property.Length - 1) {
									response =
										data.SelectTokens(property)
											.First()
											.Parent
											.Parent
											.As<JProperty>();
								} else {
									response =
										data.SelectTokens(property)
											.First()
											.Parent
											.As<JProperty>();
								}

							} else {
								response =
									data.SelectToken(property)
										.Parent
										.As<JProperty>();
							}

							return response.Value.ToString();
						})
					.ToDelimited(delimiter);
		}
	}
}
