using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    public class JsonParser<T> : IParser<T> where T : SchemaBase
    {
        public IEnumerable<T> Parse(string data)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(data);
        }
    }
}
