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

using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    internal class SampleAppMarkdownRenderer : MarkdownRenderer
    {
        public SampleAppMarkdownRenderer(MarkdownDocument document, ILinkRegister linkRegister, IImageResolver imageResolver, ICodeBlockResolver codeBlockResolver)
            : base(document, linkRegister, imageResolver, codeBlockResolver)
        {
        }

        protected override void RenderCode(CodeBlock element, IRenderContext context)
        {
            base.RenderCode(element, context);
            if (string.IsNullOrWhiteSpace(element.CodeLanguage))
            {
                return;
            }

            var language = element.CodeLanguage.ToUpper();
            switch (language)
            {
                case "CSHARP":
                case "CS":
                    language = "C#";
                    break;
            }

            var localContext = context as UIElementCollectionRenderContext;
            var collection = localContext?.BlockUIElementCollection;

            if (localContext == null || collection?.Any() != true)
            {
                return;
            }

            var lastIndex = collection.Count() - 1;

            // Removes the current Code Block UI from the UI Collection, and wraps it in additional UI.
            if (collection[lastIndex] is ScrollViewer viewer)
            {
                collection.RemoveAt(lastIndex);

                var headerGrid = new Grid
                {
                    Background = ColorCode.UWP.Common.ExtensionMethods.GetSolidColorBrush("#11000000")
                };
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var languageBlock = new TextBlock
                {
                    Text = language,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                headerGrid.Children.Add(languageBlock);

                var copyButton = new Button
                {
                    Content = "Copy",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                copyButton.Click += (s, e) =>
                {
                    var content = new DataPackage();
                    content.SetText(element.Text);
                    Clipboard.SetContent(content);
                };

                headerGrid.Children.Add(copyButton);
                Grid.SetColumn(copyButton, 1);

                var panel = new StackPanel();
                panel.Children.Add(headerGrid);
                panel.Children.Add(viewer);
                panel.Background = viewer.Background;
                panel.Margin = viewer.Margin;

                collection.Add(panel);
            }
        }
    }
}