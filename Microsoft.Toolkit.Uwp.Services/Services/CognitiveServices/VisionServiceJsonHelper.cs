using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Congnative Json Service Parser.
    /// </summary>
    public static class VisionServiceJsonHelper
    {
        /// <summary>
        /// Deserialize json data to generic object T
        /// </summary>
        /// <typeparam name="T">Target Generic Object</typeparam>
        /// <param name="data">Data to be serialized</param>
        /// <returns>Object of type T</returns>
        public static T JsonDesrialize<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Serialize object data into json string
        /// </summary>
        /// <param name="data">Object to be serialized</param>
        /// <returns>Serialized string</returns>
        public static string JsonSerialize(object data)
        {
            if (data == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(data);
        }
    }
}
