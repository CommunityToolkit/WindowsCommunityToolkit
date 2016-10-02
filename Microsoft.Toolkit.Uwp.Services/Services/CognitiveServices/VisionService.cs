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
        private const string AnalyzeImageUrl = "analyze";
        private const string DescribeImageUrl = "describe";
        private const string AutoDetect = "unk";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        private readonly string _subscriptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionService"/> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key optained from MS cogitative services</param>
        public VisionService(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        /// <summary>
        /// Vision Service Get Tags
        /// </summary>
        /// <param name="imageStream">Image Stream</param>
        /// <returns>Tags</returns>
        public async Task<ImageTags> GetTagsAsync(IRandomAccessStream imageStream)
        {
            string requestUri = GetTagUrl();
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.JsonDesrialize<ImageTags>(result);
        }

        /// <summary>
        /// Vision Service Get Tags
        /// </summary>
        /// <param name="imageUrl">Image Url</param>
        /// <returns>Tags</returns>
        public async Task<ImageTags> GetTagsAsync(string imageUrl)
        {
            var requestUri = GetTagUrl();
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<ImageTags>(result);
        }

        /// <summary>
        /// Vision Service OCR
        /// </summary>
        /// <param name="imageStream">Image Stream</param>
        /// <param name="language">Language</param>
        /// <param name="detectOrientation">Detect Orientation</param>
        /// <returns>OCR Data</returns>
        public async Task<ImageOCR> OcrAsync(IRandomAccessStream imageStream, string language = AutoDetect, bool detectOrientation = false)
        {
            string requestUri = GetOcrUrl(language, detectOrientation);
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.JsonDesrialize<ImageOCR>(result);
        }

        /// <summary>
        /// Vision Service OCR
        /// </summary>
        /// <param name="imageUrl">Image Url</param>
        /// <param name="language">Language</param>
        /// <param name="detectOrientation">Detect Orientation</param>
        /// <returns>OCR Data</returns>
        public async Task<ImageOCR> OcrAsync(string imageUrl, string language = AutoDetect, bool detectOrientation = false)
        {
            var requestUri = GetOcrUrl(language, detectOrientation);
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<ImageOCR>(result);
        }

        /// <summary>
        /// Vision Service Analyze Image
        /// </summary>
        /// <param name="imageStream">Image stream</param>
        /// <param name="visualFeatures">Visual Feature</param>
        /// <param name="details">Details</param>
        /// <returns>Analyze Image result</returns>
        public async Task<ImageAnalysis> AnalyzeImageAsync(IRandomAccessStream imageStream, string visualFeatures, string details)
        {
            var requestUri = GetAnalyzeImageUrl(visualFeatures, details);
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.JsonDesrialize<ImageAnalysis>(result);
        }

        /// <summary>
        /// Vision Service Analyze Image
        /// </summary>
        /// <param name="imageUrl">Image Url</param>
        /// <param name="visualFeatures">Visual Feature</param>
        /// <param name="details">Details</param>
        /// <returns>Analyze Image result</returns>
        public async Task<ImageAnalysis> AnalyzeImageAsync(string imageUrl, string visualFeatures, string details)
        {
            var requestUri = GetAnalyzeImageUrl(visualFeatures, details);
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<ImageAnalysis>(result);
        }

        /// <summary>
        /// Vision Service Describe Image
        /// </summary>
        /// <param name="imageStream">Image Stream</param>
        /// <param name="maxCandidates">Max number of candidates</param>
        /// <returns>Image Description</returns>
        public async Task<ImageDescription> DescribeImageAsync(IRandomAccessStream imageStream, int maxCandidates = 1)
        {
            var requestUri = GetDescribeImageUrl(maxCandidates);
            var client = GetHttpClient();
            var multipartFormDataContent = await GetMultiPartFormData(imageStream);
            var result = await Post(requestUri, client, multipartFormDataContent);
            return VisionServiceJsonHelper.JsonDesrialize<ImageDescription>(result);
        }

        /// <summary>
        /// Vision Service Describe Image
        /// </summary>
        /// <param name="imageUrl">Image Url</param>
        /// <param name="maxCandidates">Max number of candidates</param>
        /// <returns>Image Description</returns>
        public async Task<ImageDescription> DescribeImageAsync(string imageUrl, int maxCandidates = 1)
        {
            var requestUri = GetDescribeImageUrl(maxCandidates);
            var client = GetHttpClient();
            var content = GetStringContent(imageUrl);
            string result = await Post(requestUri, client, content);
            return VisionServiceJsonHelper.JsonDesrialize<ImageDescription>(result);
        }

        private static string GetOcrUrl(string language, bool detectOrientation)
        {
            return $"{ServiceUrl}{OcrServiceUrl}?language={language}&detectOrientation={detectOrientation}";
        }

        private static string GetTagUrl()
        {
            return $"{ServiceUrl}{TagServiceUrl}";
        }

        private static string GetAnalyzeImageUrl(string visualFeatures, string details)
        {
            return $"{ServiceUrl}{AnalyzeImageUrl}?visualFeatures={visualFeatures}&details={details}";
        }

        private static string GetDescribeImageUrl(int maxCandidates)
        {
            return $"{ServiceUrl}{DescribeImageUrl}?maxCandidates={maxCandidates}";
        }
    }
}
