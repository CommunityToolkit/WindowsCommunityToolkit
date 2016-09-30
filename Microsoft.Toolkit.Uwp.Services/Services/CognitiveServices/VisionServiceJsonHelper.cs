using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Congnative Service Parser.
    /// </summary>
    public static class VisionServiceJsonHelper
    {
        public static T Parse<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string Stringify(object data)
        {
            if (data == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(data);
        }
    }
}
