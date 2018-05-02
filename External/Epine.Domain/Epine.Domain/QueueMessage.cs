using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epine.Domain
{
    public class QueueMessage
    {
        public string Identifier { get; set; }

        public byte[] Data { get; set; }

        public byte[] CorrelationId { get; set; }
    }
}
