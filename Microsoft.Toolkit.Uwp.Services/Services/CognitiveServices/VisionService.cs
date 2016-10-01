using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using HttpClient = System.Net.Http.HttpClient;
using HttpMethod = System.Net.Http.HttpMethod;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    public partial class VisionService
    {
        private const string ServiceUrl = "https://api.projectoxford.ai/vision/v1.0/";
        private const string TagServiceUrl = "tag";
        private const string OcrServiceUrl = "ocr";
        private const string AutoDetect = "unk";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        private readonly string _subscriptionKey;

        public VisionService(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<ImageTags> GetTagsAsync(IRandomAccessStream imageStream)
        {
            string requestUri = GetTagUrl();
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.Parse<ImageTags>(result);
        }

        public async Task<ImageTags> GetTagsAsync(string imageUrl)
        {
            var requestUri = GetTagUrl();
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.Parse<ImageTags>(result);
        }

        private static string GetTagUrl()
        {
            return $"{ServiceUrl}{TagServiceUrl}";
        }

        public async Task<OcrData> OcrAsync(IRandomAccessStream imageStream, string language = AutoDetect, bool detectOrientation = false)
        {
            string requestUri = GetOcrUrl(language, detectOrientation);
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.Parse<OcrData>(result);
        }

        public async Task<OcrData> OcrAsync(string imageUrl, string language = AutoDetect, bool detectOrientation = false)
        {
            var requestUri = GetOcrUrl(language, detectOrientation);
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.Parse<OcrData>(result);
        }

        private static string GetOcrUrl(string language, bool detectOrientation)
        {
            return $"{ServiceUrl}{OcrServiceUrl}?language={language}&detectOrientation={detectOrientation}";
        }
    }
}
