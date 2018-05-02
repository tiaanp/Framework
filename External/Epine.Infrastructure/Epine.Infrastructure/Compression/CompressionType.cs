using System.ComponentModel;

namespace Epine.Infrastructure.Compression
{
	/// <summary>
	/// 
	/// </summary>
    public enum CompressionType : int
    {
		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("none")
		]
        None = 0,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("rar")
		]
        Rar = 1,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("zip")
		]
        Zip = 2,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description("gz")
		]
        GZip = 4
    }
}
