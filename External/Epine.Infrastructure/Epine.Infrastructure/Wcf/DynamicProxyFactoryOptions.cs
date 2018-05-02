using System.Text;

namespace Epine.Infrastructure.Wcf {

	internal class DynamicProxyFactoryOptions {

		#region Instance Properties

		internal LanguageOptions Language {
			get {
				return this.lang;
			}

			set {
				this.lang = value;
			}
		}

		internal FormatModeOptions FormatMode {
			get {
				return this.mode;
			}

			set {
				this.mode = value;
			}
		}

		/// <summary>
		///		Gets/Sets
		/// </summary>
		/// <remarks>
		///		The code modifier allows the user of the dynamic proxy factory to modify 
		///		the generated proxy code before it is compiled and used. This is useful in 
		///		situations where the generated proxy has to be modified manually for inter-op reasons.
		/// </remarks>
		internal ProxyCodeModifier CodeModifier {
			get {
				return this.codeModifier;
			}

			set {
				this.codeModifier = value;
			}
		}

		#endregion

		#region Constructor

		public DynamicProxyFactoryOptions() {
			this.lang = LanguageOptions.CS;
			this.mode = FormatModeOptions.Auto;
			this.codeModifier = null;
		}

		#endregion

		#region Object Implementation

		public override string ToString() {
			return
				new StringBuilder()
					.Append("DynamicProxyFactoryOptions[")
					.Append("Language=" + Language)
					.Append(",FormatMode=" + FormatMode)
					.Append(",CodeModifier=" + CodeModifier)
					.Append("]")
				.ToString();
		}

		#endregion

		#region Instance Events

		internal delegate string ProxyCodeModifier(string proxyCode);

		#endregion

		#region Instance Fields

		private LanguageOptions lang;
		private FormatModeOptions mode;
		private ProxyCodeModifier codeModifier;

		#endregion

		#region Nested Types

		internal enum LanguageOptions { CS, VB }

		internal enum FormatModeOptions { Auto, XmlSerializer, DataContractSerializer }

		#endregion
	}
}
