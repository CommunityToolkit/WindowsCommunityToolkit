using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    public class TwitterSearchParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.statuses.Select(r => r.Parse()).ToList();
        }
    }
}
