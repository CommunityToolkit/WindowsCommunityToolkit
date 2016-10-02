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

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using HttpClient = System.Net.Http.HttpClient;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Class for connecting to Vision Service
    /// </summary>
    public partial class VisionService
    {
        private static async Task<string> Post(string requestUri, HttpClient client, HttpContent content)
        {
            var response = await client.PostAsync(requestUri, content);
            var result = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return result;
                default:
                    var details = VisionServiceJsonHelper.JsonDesrialize<RequestExceptionDetails>(result);
                    throw new VisionServiceException(details.Message, details);
            }
        }

        private static StringContent GetStringContent(string imageUrl)
        {
            var result = VisionServiceJsonHelper.JsonSerialize(new { url = imageUrl });
            var content = new StringContent(result);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        private static async Task<MultipartFormDataContent> GetMultiPartFormData(IRandomAccessStream imageStream)
        {
            var fileBytes = new byte[imageStream.Size];
            using (var reader = new DataReader(imageStream))
            {
                await reader.LoadAsync((uint)imageStream.Size);
                reader.ReadBytes(fileBytes);
            }

            string boundary = DateTime.Now.Ticks.ToString("x");
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent(boundary);
            HttpContent byteContent = new ByteArrayContent(fileBytes);
            multipartFormDataContent.Add(byteContent, "media");
            return multipartFormDataContent;
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(SubscriptionKeyHeader, _subscriptionKey);
            return client;
        }
    }
}
