using System.ComponentModel;

namespace Epine.Infrastructure.Wcf {

	/// <summary>
	///		
	/// </summary>
	public enum BindingType : int {

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.BasicHttpBinding")
		]
		BasicHttpBinding				= 1,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.BasicHttpsBinding")
		]
		BasicHttpsBinding				= 2,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetHttpBinding")
		]
		NetHttpBinding					= 3,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetHttpsBinding")
		]
		NetHttpsBinding					= 4,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetMsmqBinding")
		]
		NetMsmqBinding					= 5,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetNamedPipeBinding")
		]
		NetNamedPipeBinding				= 6,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetPeerTcpBinding")
		]
		NetPeerTcpBinding				= 7,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.NetTcpBinding")
		]
		NetTcpBinding					= 8,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.WS2007FederationHttpBinding")
		]
		WS2007FederationHttpBinding		= 9,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.WS2007HttpBinding")
		]
		WS2007HttpBinding				= 10,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.WSDualHttpBinding")
		]
		WSDualHttpBinding				= 11,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.WSFederationHttpBinding")
		]
		WSFederationHttpBinding			= 12,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("System.ServiceModel.WSHttpBinding")
		]
		WSHttpBinding					= 13
	}
}
