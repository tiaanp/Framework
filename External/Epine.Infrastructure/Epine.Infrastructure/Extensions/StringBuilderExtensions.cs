using System.Collections.Generic;
using System.Text;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	/// 
	/// </summary>
	public static class StringBuilderExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lines"></param>
		public static void AppenLineRange(this StringBuilder builder, IEnumerable<string> lines) {
			lines
				.ForEachElement(
					line => builder.AppendLine(line));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lines"></param>
		/// <param name="args"></param>
		public static void AppenLineFormatRange(this StringBuilder builder, IEnumerable<string> lines, params object[] args) {
			lines
				.ForEachElement(
					line => {
						builder.AppendFormat(line, args);
						builder.AppendLine();
					});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lines"></param>
		public static void AppenRange(this StringBuilder builder, IEnumerable<string> lines) {
			lines
				.ForEachElement(
					line => builder.Append(line));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lines"></param>
		/// <param name="args"></param>
		public static void AppenFormatRange(this StringBuilder builder, IEnumerable<string> lines, params object[] args) {
			lines
				.ForEachElement(
					line => builder.AppendFormat(line, args));
		}
	}
}
