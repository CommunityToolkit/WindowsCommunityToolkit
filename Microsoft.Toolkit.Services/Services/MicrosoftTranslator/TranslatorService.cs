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

        private const int _maxTextLength = 5000;
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
        }

        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        public string SubscriptionKey
        {
            get { return _authToken.SubscriptionKey; }
            set { _authToken.SubscriptionKey = value; }
        }

        /// <summary>
        /// Gets or sets the string representing the supported language code to translate the text in.
        /// </summary>
        /// <value>The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        public string Language { get; set; }

        /// <summary>
        /// Detects the language of a text.
        /// </summary>
        /// <param name="text">A string representing the text whose language must be detected.</param>
        /// <returns>A <see cref="DetectLanguageResponse"/> object containing information about the detected language.</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-detect.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        public async Task<DetectLanguageResponse> DetectLanguageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            text = text.Substring(0, Math.Min(text.Length, _MaxTextLengthForDetection));

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}detect?{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, new object[] { new { Text = text } }))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<DetectLanguageResponse>>(content).FirstOrDefault();
                return responseContent;
            }
        }

        /// <summary>
        /// Retrieves the languages available for translation.
        /// </summary>
        /// <param name="requestLanguage">The language in which to obtain the name of the languages. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used. If this property is <strong>null</strong> too, the default English language wil be used.</param>
        /// <returns>A dictionary containing the languages supported for translation by <strong>Microsoft Translator Service</strong>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language codes.</para>
        /// <para>For more information, go to https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-languages.
        /// </para>
        /// </remarks>
        public async Task<Dictionary<string, Language>> GetLanguagesAsync(string requestLanguage = null)
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = $"{BaseUrl}languages?scope=translation&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString))
            {
                requestLanguage = requestLanguage ?? Language;
                if (!string.IsNullOrWhiteSpace(requestLanguage))
                {
                    // If necessary, adds the Accept-Language header in order to get localized language names.
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(requestLanguage));
                }

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var jsonContent = JToken.Parse(content)["translation"];
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, Language>>(jsonContent.ToString());

                return responseContent;
            }
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A <see cref="TranslateResponse"/> object containing translated text and information.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <param name="from">A string representing the language code of the original text. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">The <paramref name="text"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public async Task<TranslateResponse> TranslateAsync(string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length > _maxTextLength)
            {
                throw new ArgumentException($"{nameof(text)} parameter cannot be longer than {_maxTextLength} characters");
            }

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(to))
            {
                to = Language;
            }

            var uriString = (string.IsNullOrWhiteSpace(from) ? $"{BaseUrl}translate?to={to}" : $"{BaseUrl}translate?from={from}&to={to}") + $"&{ApiVersion}";
            using (var request = CreateHttpRequest(uriString, HttpMethod.Post, new object[] { new { Text = text } }))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseContent = JsonConvert.DeserializeObject<IEnumerable<TranslateResponse>>(content).FirstOrDefault();
                return responseContent;
            }
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A <see cref="TranslateResponse"/> object containing translated text and information.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">The <paramref name="text"/> parameter is longer than 1000 characters.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method perform a non-blocking request for text translation.</para>
        /// <para>For more information, go to https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public Task<TranslateResponse> TranslateAsync(string text, string to = null) => TranslateAsync(text, null, to);

        /// <summary>
        /// Initializes the <see cref="TranslatorService"/> class by getting an access token for the service.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>
        /// <para>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</para>
        /// <para>You must register Microsoft Translator on https://portal.azure.com to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        public Task InitializeAsync() => CheckUpdateTokenAsync();

        /// <summary>
        /// Initializes the <see cref="TranslatorService"/> class by getting an access token for the service.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key for the Microsoft Translator Service on Azure.</param>
        /// <param name="language">A string representing the supported language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>
        /// <para>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</para>
        /// <para>You must register Microsoft Translator on https://portal.azure.com to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        public Task InitializeAsync(string subscriptionKey, string language)
        {
            _authToken = new AzureAuthToken(subscriptionKey);

            Language = language;

            return InitializeAsync();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorService"/> class by getting an access token for the service.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key for the Microsoft Translator Service on Azure.</param>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>
        /// <para>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</para>
        /// <para>You must register Microsoft Translator on https://portal.azure.com to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        public Task InitializeAsync(string subscriptionKey) => InitializeAsync(subscriptionKey, CultureInfo.CurrentCulture.Name.ToLower());

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