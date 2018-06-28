// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An efficient and extensible control that can parse and render markdown.
    /// </summary>
    public sealed partial class MarkdownTextBlock : Control, ILinkRegister, IImageResolver, ICodeBlockResolver
    {

        private long FontSizePropertyToken { get; set; }

        private long BackgroundPropertyToken { get; set; }

        private long BorderBrushPropertyToken { get; set; }

        private long BorderThicknessPropertyToken { get; set; }

        private long CharacterSpacingPropertyToken { get; set; }

        private long FontFamilyPropertyToken { get; set; }

        private long FontStretchPropertyToken { get; set; }

        private long FontStylePropertyToken { get; set; }

        private long FontWeightPropertyToken { get; set; }

        private long ForegroundPropertyToken { get; set; }

        private long PaddingPropertyToken { get; set; }

        private long RequestedThemePropertyToken { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownTextBlock"/> class.
        /// </summary>
        public MarkdownTextBlock()
        {
            // Set our style.
            DefaultStyleKey = typeof(MarkdownTextBlock);

            // Set our style.
            DefaultStyleKey = typeof(MarkdownTextBlock);
            themeListener = new Helpers.ThemeListener();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void ThemeListener_ThemeChanged(Helpers.ThemeListener sender)
        {
            RenderMarkdown();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Listens for theme changes and updates the rendering.
            themeListener.ThemeChanged += ThemeListener_ThemeChanged;

            // Register for property callbacks that are owned by our parent class.
            FontSizePropertyToken = RegisterPropertyChangedCallback(FontSizeProperty, OnPropertyChanged);
            BackgroundPropertyToken = RegisterPropertyChangedCallback(BackgroundProperty, OnPropertyChanged);
            BorderBrushPropertyToken = RegisterPropertyChangedCallback(BorderBrushProperty, OnPropertyChanged);
            BorderThicknessPropertyToken = RegisterPropertyChangedCallback(BorderThicknessProperty, OnPropertyChanged);
            CharacterSpacingPropertyToken = RegisterPropertyChangedCallback(CharacterSpacingProperty, OnPropertyChanged);
            FontFamilyPropertyToken = RegisterPropertyChangedCallback(FontFamilyProperty, OnPropertyChanged);
            FontStretchPropertyToken = RegisterPropertyChangedCallback(FontStretchProperty, OnPropertyChanged);
            FontStylePropertyToken = RegisterPropertyChangedCallback(FontStyleProperty, OnPropertyChanged);
            FontWeightPropertyToken = RegisterPropertyChangedCallback(FontWeightProperty, OnPropertyChanged);
            ForegroundPropertyToken = RegisterPropertyChangedCallback(ForegroundProperty, OnPropertyChanged);
            PaddingPropertyToken = RegisterPropertyChangedCallback(PaddingProperty, OnPropertyChanged);
            RequestedThemePropertyToken = RegisterPropertyChangedCallback(RequestedThemeProperty, OnPropertyChanged);

        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnhookListeners();
            themeListener.ThemeChanged -= ThemeListener_ThemeChanged;
            themeListener = null;

            // Register for property callbacks that are owned by our parent class.
            UnregisterPropertyChangedCallback(FontSizeProperty, FontSizePropertyToken);
            UnregisterPropertyChangedCallback(BackgroundProperty, BackgroundPropertyToken);
            UnregisterPropertyChangedCallback(BorderBrushProperty, BorderBrushPropertyToken);
            UnregisterPropertyChangedCallback(BorderThicknessProperty, BorderThicknessPropertyToken);
            UnregisterPropertyChangedCallback(CharacterSpacingProperty, CharacterSpacingPropertyToken);
            UnregisterPropertyChangedCallback(FontFamilyProperty, FontFamilyPropertyToken);
            UnregisterPropertyChangedCallback(FontStretchProperty, FontStylePropertyToken);
            UnregisterPropertyChangedCallback(FontWeightProperty, FontWeightPropertyToken);
            UnregisterPropertyChangedCallback(ForegroundProperty, ForegroundPropertyToken);
            UnregisterPropertyChangedCallback(PaddingProperty, PaddingPropertyToken);
            UnregisterPropertyChangedCallback(RequestedThemeProperty, RequestedThemePropertyToken);
            _rootElement = null;
        }


        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            // Grab our root
            _rootElement = GetTemplateChild("RootElement") as Border;

            // And make sure to render any markdown we have.
            RenderMarkdown();
        }
    }
}