using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;

namespace Epine.Infrastructure.Wcf {

	/// <summary>
	/// 
	/// </summary>
	public class DynamicProxyException : ApplicationException {

		#region Instance Properties

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<MetadataConversionError> MetadataImportErrors {
			get;
			internal set;
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<MetadataConversionError> CodeGenerationErrors {
			get;
			internal set;
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<CompilerError> CompilationErrors {
			get;
			internal set;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		internal DynamicProxyException(string message)
			: base(message) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		internal DynamicProxyException(string message, Exception innerException)
			: base(message, innerException) {
		}

		#endregion

		#region Object Implementation

		/// <inheritdoc />
		public override string ToString() {
			var builder = new StringBuilder();
			builder.AppendLine(base.ToString());

			if (this.MetadataImportErrors != null) {
				builder
					.AppendLine("Metadata Import Errors:")
					.AppendLine(
						DynamicProxyFactory
							.ToString(
								this.MetadataImportErrors));
			}

			if (this.CodeGenerationErrors != null) {
				builder
					.AppendLine("Code Generation Errors:")
					.AppendLine(
						DynamicProxyFactory
							.ToString(
								this.CodeGenerationErrors));
			}

			if (this.CompilationErrors != null) {
				builder
					.AppendLine("Compilation Errors:")
					.AppendLine(
						DynamicProxyFactory
							.ToString(
								this.CompilationErrors));
			}

			return builder.ToString();
		}

		#endregion
	}
}
