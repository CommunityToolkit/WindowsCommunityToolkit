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
using System.Xml.Linq;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
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
        private const string BaseUrl = "http://api.microsofttranslator.com/v2/Http.svc/";
        private const string LanguagesUri = "GetLanguagesForTranslate";
        private const string TranslateUri = "Translate?text={0}&to={1}&contentType=text/plain";
        private const string TranslateWithFromUri = "Translate?text={0}&from={1}&to={2}&contentType=text/plain";
        private const string DetectUri = "Detect?text={0}";
        private const string LanguageNamesUri = "GetLanguageNames?locale={0}";
        private const string AuthorizationUri = "Authorization";

        private const string ArrayNamespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";
        private const string ArrayOfStringXmlElement = "ArrayOfstring";
        private const string StringXmlElement = "string";
        private const string XmlContentType = "text/xml";

        private const int _maxTextLength = 1000;
        private const int _MaxTextLengthForAutoDetection = 100;

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
        /// <param name="text">A string represeting the text whose language must be detected.</param>
        /// <returns>A string containing a two-character Language code for the given text.</returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <term>The <see cref="SubscriptionKey"/> property hasn't been set.</term>
        /// <term>The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</term>
        /// </list>
        /// </exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language detection.</para>
        /// <para>For more information, go to https://docs.microsofttranslator.com/text-translate.html#!/default/get_Detect.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        public async Task<string> DetectLanguageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            text = text.Substring(0, Math.Min(text.Length, _MaxTextLengthForAutoDetection));

            // Checks if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            var uriString = string.Format(DetectUri, Uri.EscapeDataString(text));

            using (var request = CreateHttpRequest($"{BaseUrl}{uriString}"))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var doc = XDocument.Parse(content);
                var detectedLanguage = doc.Root.Value;

                return detectedLanguage;
            }
        }

        /// <summary>
        /// Retrieves the languages available for translation.
        /// </summary>
        /// <returns>A string array containing the language codes supported for translation by <strong>Microsoft Translator Service</strong>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language codes.</para>
        /// <para>For more information, go to https://docs.microsofttranslator.com/text-translate.html#!/default/get_GetLanguagesForTranslate.
        /// </para>
        /// </remarks>
        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            // Check if it is necessary to obtain/update access token.
            await CheckUpdateTokenAsync().ConfigureAwait(false);

            using (var request = CreateHttpRequest($"{BaseUrl}{LanguagesUri}"))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                XNamespace ns = ArrayNamespace;
                var xmlContent = XDocument.Parse(content);

                var languages = xmlContent.Root.Elements(ns + "string").Select(s => s.Value);
                return languages;
            }
        }

        /// <summary>
        /// Retrieves friendly names for the languages available for text translation.
        /// </summary>
        /// <param name="language">The language used to localize the language names. If the parameter is set to <strong>null</strong>, the language specified in the <seealso cref="Language"/> property will be used.</param>
        /// <returns>An array of <see cref="ServiceLanguage"/> containing the language codes and names supported for translation by <strong>Microsoft Translator Service</strong>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks><para>This method performs a non-blocking request for language name.</para>
        /// <para>For more information, go to https://docs.microsofttranslator.com/text-translate.html#!/default/post_GetLanguageNames.
        /// </para>
        /// </remarks>
        /// <see cref="GetLanguagesAsync"/>
        public async Task<IEnumerable<ServiceLanguage>> GetLanguageNamesAsync(string language = null)
        {
            var languageCodes = await GetLanguagesAsync();

            if (string.IsNullOrWhiteSpace(language))
            {
                language = Language ?? "en";
            }

            using (var request = CreateHttpRequest(string.Format($"{BaseUrl}{LanguageNamesUri}", language), HttpMethod.Post))
            {
                XNamespace ns = ArrayNamespace;
                var xmlRequest = new XDocument(new XElement(ns + ArrayOfStringXmlElement, from lang in languageCodes select new XElement(ns + StringXmlElement, lang)));

                request.Content = new StringContent(xmlRequest.ToString());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(XmlContentType);

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var xmlContent = XDocument.Parse(content);

                var languageNames = xmlContent.Root.Elements(ns + StringXmlElement).Select(s => s.Value);

                // Creates the response object.
                var languages = new ServiceLanguage[languageCodes.Count()];
                for (int i = 0; i < languages.Length; i++)
                {
                    languages[i] = new ServiceLanguage(languageCodes.ElementAt(i), languageNames.ElementAt(i));
                }

                return languages;
            }
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
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
        /// <para>For more information, go to https://docs.microsofttranslator.com/text-translate.html#!/default/get_Translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public async Task<string> TranslateAsync(string text, string from, string to)
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

            string uriString = null;
            if (string.IsNullOrWhiteSpace(from))
            {
                uriString = string.Format(TranslateUri, Uri.EscapeDataString(text), to);
            }
            else
            {
                uriString = string.Format(TranslateWithFromUri, Uri.EscapeDataString(text), from, to);
            }

            using (var request = CreateHttpRequest($"{BaseUrl}{uriString}"))
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var doc = XDocument.Parse(content);
                var translatedText = doc.Root.Value;

                return translatedText;
            }
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
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
        /// <para>For more information, go to https://docs.microsofttranslator.com/text-translate.html#!/default/get_Translate.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public Task<string> TranslateAsync(string text, string to = null) => TranslateAsync(text, null, to);

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

        private HttpRequestMessage CreateHttpRequest(string uriString, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, new Uri(uriString));
            request.Headers.Add(AuthorizationUri, _authorizationHeaderValue);

            return request;
        }
    }
}