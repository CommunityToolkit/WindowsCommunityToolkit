using System.Collections.Generic;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Core
{
    public class JsonParser<T> : IParser<T> where T : SchemaBase
    {
        public IEnumerable<T> Parse(string data)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(data);
        }
    }
}
