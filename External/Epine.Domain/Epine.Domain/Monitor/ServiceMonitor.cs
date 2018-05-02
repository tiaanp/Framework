using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epine.Domain.Monitor
{
    public class ServiceMonitor
    {
        public string ServiceName { get; set; }

        public string FromIP { get; set; }

        public string ToIP { get; set; }

        public bool IsRunning { get; set; }

        public bool PassPing { get; set; }

        public string Exception { get; set; }
    }
}
