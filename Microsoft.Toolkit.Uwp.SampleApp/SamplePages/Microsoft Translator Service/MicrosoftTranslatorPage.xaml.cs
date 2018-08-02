// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator;
using Windows.System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A sample XAML page that shows how to use <see cref="TranslatorService"/> class.
    /// </summary>
    public sealed partial class MicrosoftTranslatorPage
    {
        private TranslatorService _translatorClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTranslatorPage"/> class.
        /// </summary>
        public MicrosoftTranslatorPage()
        {
            InitializeComponent();

            _translatorClient = TranslatorService.Instance;
        }

        private async void GetKey_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation"));
        }

        private async void GetLanguages_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TranslatorServiceKey.Text))
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;

            Languages.ItemsSource = null;

            _translatorClient.SubscriptionKey = TranslatorServiceKey.Text;
            var languages = await _translatorClient.GetLanguageNamesAsync();

            Languages.ItemsSource = languages.OrderBy(d => d.Name);
            Languages.SelectedIndex = 0;

            SampleController.Current.DisplayWaitRing = false;
        }

        private async void Translate_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TranslatorServiceKey.Text) || Languages.SelectedValue == null || string.IsNullOrWhiteSpace(Sentence.Text))
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;

            Translation.Text = string.Empty;

            // Translates the text to the selected language.
            _translatorClient.SubscriptionKey = TranslatorServiceKey.Text;
            var translatedText = await _translatorClient.TranslateAsync(Sentence.Text, Languages.SelectedValue.ToString());

            Translation.Text = translatedText;

            SampleController.Current.DisplayWaitRing = false;
        }
    }
}
