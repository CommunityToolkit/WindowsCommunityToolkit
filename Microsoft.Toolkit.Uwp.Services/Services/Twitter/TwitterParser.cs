using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Parser.
    /// </summary>
    /// <typeparam name="T">Type to parse in to.</typeparam>
    public class TwitterParser<T> : Toolkit.Services.IParser<T>
        where T : Toolkit.Services.SchemaBase
    {
        /// <summary>
        /// Parse string data into strongly typed list.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>List of strongly typed objects.</returns>
        IEnumerable<T> Toolkit.Services.IParser<T>.Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<List<T>>(data);
            }
            catch (JsonSerializationException)
            {
                List<T> items = new List<T>();
                items.Add(JsonConvert.DeserializeObject<T>(data));
                return items;
            }
        }
    }
}
