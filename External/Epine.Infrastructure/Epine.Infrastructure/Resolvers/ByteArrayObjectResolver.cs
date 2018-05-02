using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epine.Infrastructure.Resolvers
{
    internal class ByteArrayObjectResolver : ObjectResolver<byte[]>
    {

        protected override void Resolve(object data)
        {
            base._Result = (byte[]) data;
        }
    }
}
