using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using HttpClient = System.Net.Http.HttpClient;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
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
                    var details = VisionServiceJsonHelper.Parse<RequestExceptionDetails>(result);
                    throw new VisionServiceException(details.Message, details);
            }
        }

        private static StringContent GetStringContent(string imageUrl)
        {
            var result = VisionServiceJsonHelper.Stringify(new { url = imageUrl });
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
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(SubscriptionKeyHeader, _subscriptionKey);
            return client;
        }
    }
}
