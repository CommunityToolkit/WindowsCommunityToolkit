// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// The <strong>TranslatorService</strong> class provides methods to translate text in various supported languages.
    /// </summary>
    /// <remarks>
    /// <para>To use this library, you must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key.
    /// </para>
    /// </remarks>
    public class TranslatorService : ITranslatorService
    {
        private const string BaseUrl = "https://api.cognitive.microsofttranslator.com/";
        private const string ApiVersion = "api-version=3.0";
        private const string AuthorizationUri = "Authorization";
        private const string JsonMediaType = "application/json";

        private const int _MaxArrayLengthForTranslation = 25;
        private const int _MaxTextLengthForTranslation = 5000;
        private const int _MaxArrayLengthForDetection = 100;
        private const int _MaxTextLengthForDetection = 10000;

        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static TranslatorService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static TranslatorService Instance => instance ?? (instance = new TranslatorService());

        private AzureAuthToken _authToken;
        private string _authorizationHeaderValue = string.Empty;

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public object Provider
        {
            get { throw new NotImplementedException(); }
        }

        private TranslatorService()
        {
            _authToken = new AzureAuthToken(string.Empty);
            Language = CultureInfo.CurrentCulture.Name.ToLower();
        }

        /// <inheritdoc/>
        public string SubscriptionKey
        {
            get { return _authToken.SubscriptionKey; }
            set { _authToken.SubscriptionKey = value; }
        }

        /// <inheritdoc/>
        public string Language { get; set; }

        /// <inheritdoc/>
        public async Task<string> DetectLanguageAsync(string input)
        {
            var response = await DetectLanguageWithResponseAsync(input).ConfigureAwait(false);
            return response?.Language;
        }

        /// <inheritdoc/>
        public async Task<DetectedLanguageResponse> DetectLanguageWithResponseAsync(string input)
        {
            var response = await DetectLanguagesWithResponseAsync(new string[] { input }).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DetectedLanguageResponse>> DetectLanguagesWithResponseAsync(IEnumerable<string> input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!input.Any())
            {
                throw new ArgumentException($"{nameof(input)} array must contain at least 1 element");
            }

            if (input.Count() > _MaxArrayLengthForDetection)
            {
                throw new ArgumentException($"{nameof(input)} array can have at most {_MaxArrayLengthForDetection} elements");
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}detect?{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new { Text = t.Substring(0, Math.Min(t.Length, _MaxTextLengthForDetection)) })))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<DetectedLanguageResponse>>(content);
                return responseContent;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var languages = await GetLanguageNamesAsync();
            return languages.OrderBy(l => l.Code).Select(l => l.Code).ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ServiceLanguage>> GetLanguageNamesAsync(string language = null)
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}languages?scope=translation&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString))
            {
                language = language ?? Language;
                if (!string.IsNullOrWhiteSpace(language))
                {
                    // If necessary, adds the Accept-Language header in order to get localized language names.
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
                }

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var jsonContent = JToken.Parse(content)["translation"];
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, ServiceLanguage>>(jsonContent.ToString()).ToList();
                responseContent.ForEach(r => r.Value.Code = r.Key);

                return responseContent.Select(r => r.Value).OrderBy(r => r.Name).ToList();
            }
        }

        /// <inheritdoc/>
        public Task<string> TranslateAsync(string input, string to = null) => TranslateAsync(input, null, to ?? Language);

        /// <inheritdoc/>
        public async Task<string> TranslateAsync(string input, string from, string to)
        {
            var response = await TranslateWithResponseAsync(new string[] { input }, from, new string[] { to }).ConfigureAwait(false);
            return response.FirstOrDefault()?.Translation.Text;
        }

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateWithResponseAsync(string input, string to = null) => TranslateWithResponseAsync(input, null, to ?? Language);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateWithResponseAsync(string input, string from, string to)
        {
            var response = await TranslateWithResponseAsync(new string[] { input }, from, new string[] { to }).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateWithResponseAsync(IEnumerable<string> input, string from, string to) => TranslateWithResponseAsync(input, from, new string[] { to });

        /// <inheritdoc/>
        public Task<TranslationResponse> TranslateWithResponseAsync(string input, IEnumerable<string> to) => TranslateWithResponseAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<TranslationResponse> TranslateWithResponseAsync(string input, string from, IEnumerable<string> to)
        {
            var response = await TranslateWithResponseAsync(new string[] { input }, from, to).ConfigureAwait(false);
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TranslationResponse>> TranslateWithResponseAsync(IEnumerable<string> input, IEnumerable<string> to = null) => TranslateWithResponseAsync(input, null, to);

        /// <inheritdoc/>
        public async Task<IEnumerable<TranslationResponse>> TranslateWithResponseAsync(IEnumerable<string> input, string from, IEnumerable<string> to)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Count() > _MaxArrayLengthForTranslation)
            {
                throw new ArgumentException($"{nameof(input)} array can have at most {_MaxArrayLengthForTranslation} elements");
            }

            if (input.Any(str => string.IsNullOrWhiteSpace(str) || str.Length > _MaxTextLengthForTranslation))
            {
                throw new ArgumentException($"Each sentence cannot be null and longer than {_MaxTextLengthForTranslation} characters");
            }

            if (to == null || !to.Any())
            {
                to = new string[] { Language };
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var toQueryString = string.Join("&", to.Select(t => $"to={t}"));
            var uriString = (string.IsNullOrWhiteSpace(from) ? $"{BaseUrl}translate?{toQueryString}" : $"{BaseUrl}translate?from={from}&{toQueryString}") + $"&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, input.Select(t => new { Text = t })))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<TranslationResponse>>(content);
                return responseContent;
            }
        }

        /// <inheritdoc/>
        public Task InitializeAsync() => CheckUpdateTokenAsync();

        /// <inheritdoc/>
        public Task InitializeAsync(string subscriptionKey, string language = null)
        {
            _authToken = new AzureAuthToken(subscriptionKey);
            Language = language ?? CultureInfo.CurrentCulture.Name.ToLower();

            return InitializeAsync();
        }

        private async Task CheckUpdateTokenAsync()
        {
            // If necessary, updates the access token.
            _authorizationHeaderValue = await _authToken.GetAccessTokenAsync().ConfigureAwait(false);
        }

        private HttpRequestMessage CreateHttpRequest(string uriString)
            => CreateHttpRequest(uriString, HttpMethod.Get);

        private HttpRequestMessage CreateHttpRequest(string uriString, HttpMethod method, object content = null)
        {
            var request = new HttpRequestMessage(method, new Uri(uriString));
            request.Headers.Add(AuthorizationUri, _authorizationHeaderValue);

            if (content != null)
            {
                var jsonRequest = JsonConvert.SerializeObject(content);
                var requestContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, JsonMediaType);
                request.Content = requestContent;
            }

            return request;
        }
    }
}