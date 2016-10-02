// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
