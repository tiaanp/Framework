using Epine.Infrastructure.Extensions;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WsdlNS = System.Web.Services.Description;

namespace Epine.Infrastructure.Wcf {
	internal class DynamicProxyFactory {

		#region Constants

		private const string DefaultNamespace = "http://tempuri.org/";

		#endregion

		#region Instance Properties

		internal IEnumerable<MetadataSection> Metadata {
			get {
				return this.metadataCollection;
			}
		}

		internal IEnumerable<Binding> Bindings {
			get {
				return this.bindings;
			}
		}

		internal IEnumerable<ContractDescription> Contracts {
			get {
				return this.contracts;
			}
		}

		internal IEnumerable<ServiceEndpoint> Endpoints {
			get {
				return this.endpoints;
			}
		}

		internal Assembly ProxyAssembly {
			get {
				return this.proxyAssembly;
			}
		}

		internal string ProxyCode {
			get {
				return this.proxyCode;
			}
		}

		internal IEnumerable<MetadataConversionError> MetadataImportWarnings {
			get {
				return this.importWarnings;
			}
		}

		internal IEnumerable<MetadataConversionError> CodeGenerationWarnings {
			get {
				return this.codegenWarnings;
			}
		}

		internal IEnumerable<CompilerError> CompilationWarnings {
			get {
				return this.compilerWarnings;
			}
		}

		#endregion

		#region Constructors

		internal DynamicProxyFactory(string wsdlUri, DynamicProxyFactoryOptions options) {
			"wsdlUri".IsNotNullArgument(wsdlUri);
			"options".IsNotNullArgument(options);

			this.wsdlUri = wsdlUri;
			this.options = options;

			this.DownloadMetadata();
			this.ImportMetadata();
			this.CreateProxy();
			this.WriteCode();
			this.CompileProxy();
		}

		internal DynamicProxyFactory(string wsdlUri)
			: this(wsdlUri, new DynamicProxyFactoryOptions()) {
			
		}

		#endregion

		#region Instance Methods

		internal ServiceEndpoint GetEndpoint(string contractName) {
			return 
				this.GetEndpoint(contractName, null);
		}
		internal ServiceEndpoint GetEndpoint(string contractName, string contractNamespace) {
			ServiceEndpoint matchingEndpoint = null;

			foreach (ServiceEndpoint endpoint in Endpoints) {
				if (this.ContractNameMatch(endpoint.Contract, contractName) &&
					this.ContractNamespaceMatch(endpoint.Contract, contractNamespace)) {
					matchingEndpoint = endpoint;
					break;
				}
			}

			if (matchingEndpoint == null)
				throw new ArgumentException(string.Format(
					Constants.ErrorMessages.ENDPOINT_NOTFOUND,
					contractName, contractNamespace));

			return matchingEndpoint;
		}
		private bool ContractNameMatch(ContractDescription cDesc, string name) {
			return (string.Compare(cDesc.Name, name, true) == 0);
		}
		private bool ContractNamespaceMatch(ContractDescription cDesc, string ns) {
			return ((ns == null) ||
					(string.Compare(cDesc.Namespace, ns, true) == 0));
		}


		internal DynamicProxy CreateProxy(string contractName) {
			return this.CreateProxy(contractName, null);
		}

		internal DynamicProxy CreateProxy(string contractName, string contractNamespace) {
			ServiceEndpoint endpoint = 
				this.GetEndpoint(
					contractName,
					contractNamespace);

			return 
				this.CreateProxy(endpoint);
		}

		internal DynamicProxy CreateProxy(ServiceEndpoint endpoint) {
			Type contractType = 
				this.GetContractType(
					endpoint.Contract.Name,
					endpoint.Contract.Namespace);

			Type proxyType = this.GetProxyType(contractType);
           
			return 
				new DynamicProxy(
					proxyType, 
					endpoint.Binding,
					endpoint.Address);
		}
		private Type GetContractType(string contractName, string contractNamespace) {

			Type[] allTypes = proxyAssembly.GetTypes();

			ServiceContractAttribute scAttr = null;
			Type contractType = null;
			XmlQualifiedName cName;
			foreach (Type type in allTypes) {
				// Is it an interface?
				if (!type.IsInterface) continue;

				// Is it marked with ServiceContract attribute?
				object[] attrs = type.GetCustomAttributes(
					typeof(ServiceContractAttribute), false);
				if ((attrs == null) || (attrs.Length == 0)) continue;

				// is it the required service contract?
				scAttr = (ServiceContractAttribute)attrs[0];
				cName = GetContractName(type, scAttr.Name, scAttr.Namespace);

				if (string.Compare(cName.Name, contractName, true) != 0)
					continue;

				if (string.Compare(cName.Namespace, contractNamespace,
							true) != 0)
					continue;

				contractType = type;
				break;
			}

			if (contractType == null)
				throw new ArgumentException(
					Constants.ErrorMessages.UNKNOWN_CONTRACT);

			return contractType;
		}
		private Type GetProxyType(Type contractType) {
			Type clientBaseType = typeof(ClientBase<>).MakeGenericType(
					contractType);

			Type[] allTypes = ProxyAssembly.GetTypes();
			Type proxyType = null;

			foreach (Type type in allTypes) {
				// Look for a proxy class that implements the service 
				// contract and is derived from ClientBase<service contract>
				if (type.IsClass && contractType.IsAssignableFrom(type)
					&& type.IsSubclassOf(clientBaseType)) {
					proxyType = type;
					break;
				}
			}

			if (proxyType == null)
				throw new DynamicProxyException(string.Format(
							Constants.ErrorMessages.PROXYTYPE_NOTFOUND,
							contractType.FullName));

			return proxyType;
		}


		private void DownloadMetadata() {
			var epr = new EndpointAddress(this.wsdlUri);

			var disco = new DiscoveryClientProtocol();
			disco.AllowAutoRedirect = true;
			disco.UseDefaultCredentials = true;
			disco.DiscoverAny(this.wsdlUri);
			disco.ResolveAll();

			var results = new Collection<MetadataSection>();
			foreach (object document in disco.Documents.Values) {
				this.AddDocumentToResults(document, results);
			}
			this.metadataCollection = results;
		}
		private void AddDocumentToResults(object document, Collection<MetadataSection> results) {
			var wsdl = document as WsdlNS.ServiceDescription;
			XmlSchema schema = document as XmlSchema;
			XmlElement xmlDoc = document as XmlElement;

			if (wsdl != null) {
				results.Add(MetadataSection.CreateFromServiceDescription(wsdl));
			} else if (schema != null) {
				results.Add(MetadataSection.CreateFromSchema(schema));
			} else if (xmlDoc != null && xmlDoc.LocalName == "Policy") {
				results.Add(MetadataSection.CreateFromPolicy(xmlDoc, null));
			} else {
				MetadataSection mexDoc = new MetadataSection();
				mexDoc.Metadata = document;
				results.Add(mexDoc);
			}
		}


		private void ImportMetadata() {
			this.codeCompileUnit = new CodeCompileUnit();
			this.CreateCodeDomProvider();

			WsdlImporter importer = new WsdlImporter(new MetadataSet(metadataCollection));
			this.AddStateForDataContractSerializerImport(importer);
			this.AddStateForXmlSerializerImport(importer);

			this.bindings = importer.ImportAllBindings();
			this.contracts = importer.ImportAllContracts();
			this.endpoints = importer.ImportAllEndpoints();
			this.importWarnings = importer.Errors;

			bool success = true;
			if (this.importWarnings != null) {
				foreach (MetadataConversionError error in this.importWarnings) {
					if (!error.IsWarning) {
						success = false;
						break;
					}
				}
			}

			if (!success) {
				DynamicProxyException exception = new DynamicProxyException(
					Constants.ErrorMessages.IMPORT_ERROR);
				exception.MetadataImportErrors = this.importWarnings;
				throw exception;
			}
		}
		private void CreateCodeDomProvider() {
			this.codeDomProvider = CodeDomProvider.CreateProvider(options.Language.ToString());
		}
		private void AddStateForDataContractSerializerImport(WsdlImporter importer) {
			XsdDataContractImporter xsdDataContractImporter =
				new XsdDataContractImporter(this.codeCompileUnit);
			xsdDataContractImporter.Options = new ImportOptions();
			xsdDataContractImporter.Options.ImportXmlType =
				(this.options.FormatMode ==
					DynamicProxyFactoryOptions.FormatModeOptions.DataContractSerializer);

			xsdDataContractImporter.Options.CodeProvider = this.codeDomProvider;
			importer.State.Add(typeof(XsdDataContractImporter),
					xsdDataContractImporter);

			foreach (IWsdlImportExtension importExtension in importer.WsdlImportExtensions) {
				DataContractSerializerMessageContractImporter dcConverter =
					importExtension as DataContractSerializerMessageContractImporter;

				if (dcConverter != null) {
					if (this.options.FormatMode ==
						DynamicProxyFactoryOptions.FormatModeOptions.XmlSerializer)
						dcConverter.Enabled = false;
					else
						dcConverter.Enabled = true;
				}

			}
		}
		private void AddStateForXmlSerializerImport(WsdlImporter importer) {
			XmlSerializerImportOptions importOptions =
				new XmlSerializerImportOptions(this.codeCompileUnit);
			importOptions.CodeProvider = this.codeDomProvider;

			importOptions.WebReferenceOptions = new WsdlNS.WebReferenceOptions();
			importOptions.WebReferenceOptions.CodeGenerationOptions =
				CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateOrder;

			importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(
				typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName);
			importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(
				typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);

			importer.State.Add(typeof(XmlSerializerImportOptions), importOptions);
		}


		private void CreateProxy() {
			this.CreateServiceContractGenerator();

			foreach (ContractDescription contract in this.contracts) {
				this.contractGenerator.GenerateServiceContractType(contract);
			}

			bool success = true;
			this.codegenWarnings = this.contractGenerator.Errors;
			if (this.codegenWarnings != null) {
				foreach (MetadataConversionError error in this.codegenWarnings) {
					if (!error.IsWarning) {
						success = false;
						break;
					}
				}
			}

			if (!success) {
				DynamicProxyException exception = new DynamicProxyException(
				 Constants.ErrorMessages.CODE_GENERATION_ERROR);
				exception.CodeGenerationErrors = this.codegenWarnings;
				throw exception;
			}
		}
		private void CreateServiceContractGenerator() {
			this.contractGenerator = new ServiceContractGenerator(
				this.codeCompileUnit);
			this.contractGenerator.Options |= ServiceContractGenerationOptions.ClientClass;
		}


		private void WriteCode() {
			using (var writer = new StringWriter()) {
				CodeGeneratorOptions codeGenOptions = new CodeGeneratorOptions();
				codeGenOptions.BracingStyle = "C";
				this.codeDomProvider.GenerateCodeFromCompileUnit(
						this.codeCompileUnit, writer, codeGenOptions);
				writer.Flush();
				this.proxyCode = writer.ToString();
			}

			// use the modified proxy code, if code modifier is set.
			if (this.options.CodeModifier != null)
				this.proxyCode = this.options.CodeModifier(this.proxyCode);
		}

		private void CompileProxy() {
			// reference the required assemblies with the correct path.
			CompilerParameters compilerParams = new CompilerParameters();

			this.AddAssemblyReference(
				typeof(System.ServiceModel.ServiceContractAttribute).Assembly,
				compilerParams.ReferencedAssemblies);

			this.AddAssemblyReference(
				typeof(System.Web.Services.Description.ServiceDescription).Assembly,
				compilerParams.ReferencedAssemblies);

			this.AddAssemblyReference(
				typeof(System.Runtime.Serialization.DataContractAttribute).Assembly,
				compilerParams.ReferencedAssemblies);

			this.AddAssemblyReference(
				typeof(System.Xml.XmlElement).Assembly,
				compilerParams.ReferencedAssemblies);

			this.AddAssemblyReference(
				typeof(System.Uri).Assembly,
				compilerParams.ReferencedAssemblies);

			this.AddAssemblyReference(
				typeof(System.Data.DataSet).Assembly,
				compilerParams.ReferencedAssemblies);

			CompilerResults results =
				this.codeDomProvider.CompileAssemblyFromSource(
					compilerParams,
					this.proxyCode);

			if ((results.Errors != null) && (results.Errors.HasErrors)) {
				DynamicProxyException exception = new DynamicProxyException(
					Constants.ErrorMessages.COMPILATION_ERROR);
				exception.CompilationErrors = ToEnumerable(results.Errors);

				throw exception;
			}

			this.compilerWarnings = ToEnumerable(results.Errors);
			this.proxyAssembly = Assembly.LoadFile(results.PathToAssembly);
		}
		private void AddAssemblyReference(Assembly referencedAssembly, StringCollection refAssemblies) {
			string path = Path.GetFullPath(referencedAssembly.Location);
			string name = Path.GetFileName(path);
			if (!(refAssemblies.Contains(name) || refAssemblies.Contains(path))) {
				refAssemblies.Add(path);
			}
		}

		#endregion

		#region Class Methods

		internal static string ToString(IEnumerable<MetadataConversionError> importErrors) {
			var response = default(string);

			if (importErrors.IsNotNullOrEmpty()) {
				var builder = new StringBuilder();

				builder
					.AppenLineRange(
						importErrors
							.Select(
								error =>
									(error.IsWarning
										? "Warning : "
										: "Error : ")
										.Append(error.Message)));

				response = builder.ToString();
			}

			return response;
		}

		internal static string ToString(IEnumerable<CompilerError> compilerErrors) {

			var response = default(string);

			if (compilerErrors.IsNotNullOrEmpty()) {
				var builder = new StringBuilder();

				builder
					.AppenLineRange(
						compilerErrors
							.Select(
								error =>
									error.ToString()));

				response = builder.ToString();
			}

			return response;
		}

		private static IEnumerable<CompilerError> ToEnumerable(
				CompilerErrorCollection collection) {
			if (collection == null) return null;

			List<CompilerError> errorList = new List<CompilerError>();
			foreach (CompilerError error in collection)
				errorList.Add(error);

			return errorList;
		}

		private static XmlQualifiedName GetContractName(Type contractType, string name, string ns) {
			if (String.IsNullOrEmpty(name)) {
				name = contractType.Name;
			}

			if (ns == null) {
				ns = DefaultNamespace;
			} else {
				ns = Uri.EscapeUriString(ns);
			}

			return new XmlQualifiedName(name, ns);
		}

		#endregion

		#region Instance Fields

		private string wsdlUri;
		private DynamicProxyFactoryOptions options;

		private CodeCompileUnit codeCompileUnit;
		private CodeDomProvider codeDomProvider;
		private ServiceContractGenerator contractGenerator;

		private Collection<MetadataSection> metadataCollection;
		private IEnumerable<Binding> bindings;
		private IEnumerable<ContractDescription> contracts;
		private ServiceEndpointCollection endpoints;
		private IEnumerable<MetadataConversionError> importWarnings;
		private IEnumerable<MetadataConversionError> codegenWarnings;
		private IEnumerable<CompilerError> compilerWarnings;

		private Assembly proxyAssembly;
		private string proxyCode;

		#endregion
	}
}
