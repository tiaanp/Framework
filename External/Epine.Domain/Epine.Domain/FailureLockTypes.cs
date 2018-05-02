using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epine.Domain
{
    public enum FailureLockTypes : int
    {
        None = 0,
        /// <summary>
        ///		SucessLock : actual value 1
        /// </summary>
        FailureLock = 1,

        /// <summary>
        ///		SucessLock : actual value 2
        /// </summary>
        SucessLock = 1 << 1,

    }
}
