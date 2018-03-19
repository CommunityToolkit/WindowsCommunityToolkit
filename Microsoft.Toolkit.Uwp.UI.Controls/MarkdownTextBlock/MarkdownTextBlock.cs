// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownTextBlock"/> class.
        /// </summary>
        public MarkdownTextBlock()
        {
            // Set our style.
            DefaultStyleKey = typeof(MarkdownTextBlock);

            // Listens for theme changes and updates the rendering.
            themeListener = new Helpers.ThemeListener();
            themeListener.ThemeChanged += ThemeListener_ThemeChanged;

            // Register for property callbacks that are owned by our parent class.
            RegisterPropertyChangedCallback(FontSizeProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BackgroundProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BorderBrushProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BorderThicknessProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(CharacterSpacingProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontFamilyProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontSizeProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontStretchProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontStyleProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontWeightProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(ForegroundProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(PaddingProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(RequestedThemeProperty, OnPropertyChanged);
        }

        private void ThemeListener_ThemeChanged(Helpers.ThemeListener sender)
        {
            RenderMarkdown();
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