using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Class for connecting to Vision Service
    /// </summary>
    public partial class VisionService
    {
        private const string ServiceUrl = "https://api.projectoxford.ai/vision/v1.0/";
        private const string TagServiceUrl = "tag";
        private const string OcrServiceUrl = "ocr";
        private const string AutoDetect = "unk";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        private readonly string _subscriptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionService"/> class.
        /// </summary>
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
            return VisionServiceJsonHelper.JsonDesrialize<ImageTags>(result);
        }

        public async Task<ImageTags> GetTagsAsync(string imageUrl)
        {
            var requestUri = GetTagUrl();
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<ImageTags>(result);
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
            return VisionServiceJsonHelper.JsonDesrialize<OcrData>(result);
        }

        public async Task<OcrData> OcrAsync(string imageUrl, string language = AutoDetect, bool detectOrientation = false)
        {
            var requestUri = GetOcrUrl(language, detectOrientation);
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<OcrData>(result);
        }

        private static string GetOcrUrl(string language, bool detectOrientation)
        {
            return $"{ServiceUrl}{OcrServiceUrl}?language={language}&detectOrientation={detectOrientation}";
        }
    }
}
