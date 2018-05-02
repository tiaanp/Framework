using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="String"/>-based objects.
	/// </summary>
	public static class StringExtensions {
		#region Class Methods

		/// <summary>
		///		Validates the parameter <paramref name="argument"/> that it is not null, 
		///		else throws an <see cref="ArgumentNullException"/>.
		/// </summary>
		/// <param name="name">
		///		the name of the actual parameter being validated.
		/// </param>
		/// <param name="argument">
		///		The parameter being validated.
		/// </param>
		public static void IsNotNullArgument(this string name, object argument) {
			if (argument == null) {
				throw new ArgumentNullException(name);
			}
		}

		/// <summary>
		///		Validates the parameter <paramref name="argument"/> that it is not null or empty or white space, 
		///		else throws an <see cref="ArgumentNullException"/>.
		/// </summary>
		/// <param name="argument">
		///		The parameter being validated.
		/// </param>
		public static void IsNotNullOrEmpytOrWhiteSpaceArgument(this string argument) {
			if (String.IsNullOrEmpty(argument) || String.IsNullOrWhiteSpace(argument)) {
				throw new ArgumentNullException(argument);
			}
		}

		/// <summary>
		///		Concatenates two specified instances of System.String.
		/// </summary>
		/// <param name="value">
		///		The first string to concatenate.
		/// </param>
		/// <param name="append">
		///		The second <see cref="object"/> to concatenate.
		/// </param>
		/// <returns>
		///		The concatenation of <paramref name="value"/> and <paramref name="append"/>.
		/// </returns>
		public static string Append(this string value, object append) {
			return
				String.Concat(
					value,
					append);
		}
		/// <summary>
		///		Concatenates the values provided in parameter <paramref name="appends"/> 
		///		to the given parameter <paramref name="value"/>.
		/// </summary>
		/// <param name="value">
		///		A <see cref="string"/> value to append to.
		/// </param>
		/// <param name="appends">
		///		A object collection to append to parameter <paramref name="value"/>.
		/// </param>
		/// <returns>
		///		The concatenation of <paramref name="value"/> and <paramref name="appends"/>.
		/// </returns>
		public static string Append(this string value, params object[] appends) {
			var response = value;
			appends
				.ForEachElement(
					append => {
						response = response.Append(append);
					});

			return response;
		}

		/// <summary>
		///		Replaces the format item in a specified string with the string 
		///		representation of a corresponding object in a specified array.
		/// </summary>
		/// <param name="format">
		///		A composite format string.
		/// </param>
		/// <param name="args">
		///		 An object array that contains zero or more objects to format.
		/// </param>
		/// <returns>
		///		A copy of format in which the format items have been replaced by the string 
		///		representation of the corresponding objects in args.
		/// </returns>
		public static string FormatString(this string format, params object[] args) {
			return
				String.Format(
					CultureInfo.InvariantCulture,
					format,
					args);
		}

		/// <summary>
		///		Indicates whether the specified string is not 
		///		null or an System.String.Empty string.
		/// </summary>
		/// <param name="value">
		///		The string to test.
		/// </param>
		/// <returns>
		///		true if the value parameter is not null or an empty string (""); otherwise, false.
		/// </returns>
		public static bool IsNotNullOrEmpty(this string value) {
			return !(String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string value) {
			return String.IsNullOrEmpty(value);
		}

		/// <summary>
		///		Encodes all the characters in parameter 
		///		<paramref name="value"/> into a sequence of bytes.
		/// </summary>
		/// <param name="value">
		///		A <see cref="string"/> value to convert into bytes.
		/// </param>
		/// <returns>
		///		A byte array.
		/// </returns>
		public static byte[] ToByteArray(this string value) {
			return System.Text.Encoding.Default.GetBytes(value);
		}

	    public static byte[] ToByteArrayUtf8(this string value)
	    {
	        return System.Text.Encoding.UTF8.GetBytes(value);
	    }
        /// <summary>
        ///		Encodes all the characters in parameter <paramref name="value"/>,
        ///		using the specified encoding passed by parameter <paramref name="encoding"/>,
        ///		into a sequence of bytes.
        /// </summary>
        /// <param name="value">
        ///		A <see cref="string"/> value to convert into bytes.
        /// </param>
        /// <param name="encoding">
        ///		Represents a character encoding.
        /// </param>
        /// <returns>
        ///		A byte array.
        /// </returns>
        public static byte[] ToByteArray(this string value, System.Text.Encoding encoding) {
			return encoding.GetBytes(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="separator"></param>
		/// <param name="ignoreSeparatorBetween"></param>
		/// <returns></returns>
		public static string[] SplitString(this string @this, char separator, char ignoreSeparatorBetween) {

			if (@this.Count(c => c == ignoreSeparatorBetween) % 2 == 0) {

				var response = new List<string>();
				var builder = new StringBuilder();
				var ignoreSeperator = false;

				foreach (var item in @this) {
					if ((item != separator || ignoreSeperator) && item != ignoreSeparatorBetween) {
						builder.Append(item);
					}
					if (item == ignoreSeparatorBetween && !ignoreSeperator) {
						ignoreSeperator = true;
					} else if (item == ignoreSeparatorBetween && ignoreSeperator) {
						ignoreSeperator = false;
					}

					if (item == separator && !ignoreSeperator) {
						response.Add(builder.ToString());
						builder.Clear();
					}
				}

				response.Add(builder.ToString());

				return response.ToArray();
			} else {
				return @this.Split(separator);
			}
		}

        public static string[] SplitStringFix(this string @this, char separator, char ignoreSeparatorBetween)
        {

            var response = new List<string>();
            var builder = new StringBuilder();
            var ignoreSeperator = false;
            for (int i = 0; i < @this.Length; i++)
            {
                var item = @this[i];


                if ((item != separator || ignoreSeperator) && item != ignoreSeparatorBetween)
                {
                    builder.Append(item);
                }
                if (item == ignoreSeparatorBetween && !ignoreSeperator)
                {
                    ignoreSeperator = true;
                }
                else if (item == ignoreSeparatorBetween && ignoreSeperator)
                {
                    if (@this.Length == i || @this[i + 1] == separator)
                    {
                        ignoreSeperator = false;

                    }
                }

                if (item == separator && !ignoreSeperator)
                {
                    response.Add(builder.ToString());
                    builder.Clear();
                }
            }

            response.Add(builder.ToString());

            return response.ToArray();
        }

        /// <summary>
        ///		Reverses the contents of parameter <paramref name="this"/>.
        /// </summary>
        /// <param name="this">
        ///		The <see cref="string"/> value to reverse.
        /// </param>
        /// <returns>
        ///		A reveres representation of parameter <paramref name="this"/>
        /// </returns>
        public static string ReverseString(this string @this) {
			if (@this == null) return null;
			var charArray = @this.ToCharArray();
			Array.Reverse(charArray);
			return new String(charArray);
		}

		/// <summary>
		///		Pluralizes parameter <paramref name="this"/>.
		/// </summary>
		/// <param name="this">
		///		A <see cref="string"/> value to pluralize.
		/// </param>
		/// <returns>
		///		A pluralized representation of parameter <paramref name="this"/>.
		/// </returns>
		public static string ToPlural(this string @this) {
			return
				System
					.Data
					.Entity
					.Design
					.PluralizationServices
					.PluralizationService
					.CreateService(
						System.Globalization.CultureInfo.CurrentUICulture)
					.Pluralize(@this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static String ToTitleCase(this string @this)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this);
		}

		/// <summary>
		///		In a specified input string, replaces all strings that match a specified 
		///		regular expression with a specified replacement string.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="pattern"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public static string ReplaceInsensitive(this string value, string pattern, string newValue) {

			return
				Regex.Replace(
					value,
					pattern,
					newValue,
					RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string ReturnContains(this string @this, IEnumerable<string> list) {
			return 
				@this.ReturnContains(list, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="list"></param>
		/// <param name="startsWith"></param>
		/// <returns></returns>
		public static string ReturnContains(this string @this, IEnumerable<string> list, bool startsWith) {
			return 
				list.FirstOrDefault(
					item => 
						@this
							.Substring(
								0, 
								(startsWith ? item.Length : @this.Length))
							.Contains(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool Contains(this string @this, IEnumerable<string> list) {
			return 
				@this.Contains(list, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="list"></param>
		/// <param name="startsWith"></param>
		/// <returns></returns>
		public static bool Contains(this string @this, IEnumerable<string> list, bool startsWith) {
			return 
				list.Any(
					item =>
						@this
							.Substring(
								0,
								(startsWith ? item.Length : @this.Length))
							.Contains(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static bool IsNumeric(this string @this) {
			return @this.All(Char.IsDigit);
		}

        /// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static JObject ToJObject(this string @this)
        {
            return JObject.Parse(@this);
        }

        #endregion
    }
}
