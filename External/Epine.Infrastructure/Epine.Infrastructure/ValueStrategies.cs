using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epine.Infrastructure
{
    public enum ValueStrategies : int
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

		/// <summary>
		///		Force : actual value 1 was (1)
		/// </summary>
		Force = 1,

		/// <summary>
		///		ForceOnEmpty : actual value 2 was (2)
		/// </summary>
		ForceOnEmpty = 1 << 1,

		/// <summary>
		///		Sum : actual value 4 was (3)
		/// </summary>
		Sum = 1 << 2,

		/// <summary>
		///		Output : actual value 8 was (4)
		/// </summary>
		Merge = 1 << 3,

		/// <summary>
		///		PreserveOriginal : actual value 16 was (5)
		/// </summary>
		PreserveOriginal = 1 << 4
    }
}
