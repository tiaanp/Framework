using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epine.Infrastructure.Compression;
using Newtonsoft.Json.Linq;

namespace Epine.Infrastructure.Resolvers
{
    internal class ListJObjectResolver : ObjectResolver<List<JObject>>
    {
        protected override void Resolve(object data)
        {
            base._Result = ((List<string>)data).Select(e => JObject.Parse(e.ToString())).ToList();
            //List<JObject> result = new List<JObject>();
            //foreach (object o in (IList)data)
            //{
            //    result.Add(JObject.Parse(o.ToString()));
            //}
          //  base._Result = result;
        }
    }
}
