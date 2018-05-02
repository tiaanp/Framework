
namespace Epine.Infrastructure.Wcf {
	internal class Constants {
		internal class ErrorMessages {
			internal const string IMPORT_ERROR =
				"There was an error in importing the metadata.";

			internal const string CODE_GENERATION_ERROR =
				"There was an error in generating the proxy code.";

			internal const string COMPILATION_ERROR =
				"There was an error in compiling the proxy code.";

			internal const string UNKNOWN_CONTRACT =
				"The specified contract is not found in the proxy assembly.";

			internal const string ENDPOINT_NOTFOUND =
				"The endpoint associated with contract {1}:{0} is not found.";

			internal const string PROXYTYPE_NOTFOUND =
				"The proxy that implements the service contract {0} is not found.";

			internal const string PROXY_CTOR_NOTFOUND =
				"The constructor matching the specified parameter types is not found.";

			internal const string PARAMETER_VALUE_MISTMATCH =
				"The type for each parameter values must be specified.";

			internal const string METHOD_NOTFOUND =
				"The method {0} is not found.";
		}
	}
}
