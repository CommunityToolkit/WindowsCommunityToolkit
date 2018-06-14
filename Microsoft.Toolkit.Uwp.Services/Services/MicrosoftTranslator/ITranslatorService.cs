// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// The <strong>ITranslatorServiceClient</strong> interface specifies properties and methods to translate text in various supported languages.
    /// </summary>
    public interface ITranslatorService
    {
        /// <summary>
        /// Gets or sets the Subscription key that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Subscription Key.</value>
        /// <remarks>
        /// <para>You must register Microsoft Translator on https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation to obtain the Subscription key needed to use the service.</para>
        /// </remarks>
        string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the string representing the supported language code to translate the text in.
        /// </summary>
        /// <value>The string representing the supported language code to translate the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        string Language { get; set; }

        /// <summary>
        /// Initializes the <see cref="TranslatorService"/> class by getting an access token for the service.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the initialize operation.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="SubscriptionKey"/> property hasn't been set.</exception>
        /// <exception cref="TranslatorServiceException">The provided <see cref="SubscriptionKey"/> isn't valid or has expired.</exception>
        /// <remarks>Calling this method isn't mandatory, because the token is get/refreshed everytime is needed. However, it is called at startup, it can speed-up subsequest requests.</remarks>
        Task InitializeAsync();

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
        /// <seealso cref="Language"/>
        Task<string> DetectLanguageAsync(string text);

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
        Task<IEnumerable<string>> GetLanguagesAsync();

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
        Task<IEnumerable<ServiceLanguage>> GetLanguageNamesAsync(string language = null);

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
        Task<string> TranslateAsync(string text, string from, string to);

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
        Task<string> TranslateAsync(string text, string to = null);
    }
}