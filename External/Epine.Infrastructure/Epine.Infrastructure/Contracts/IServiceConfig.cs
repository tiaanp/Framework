using Epine.Infrastructure.Wcf;

namespace Epine.Infrastructure.Contracts {

	/// <summary>
	/// 
	/// </summary>
	public interface IServiceConfig {

		/// <summary>
		/// 
		/// </summary>
		string WsdlUri { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string Contract { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string EndPoint { get; set; }

		/// <summary>
		/// 
		/// </summary>
		BindingType BindingType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string Method { get; set; }
       
		/// <summary>
		/// 
		/// </summary>
        int Timeout { get; set; }
    }
}
