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

using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An efficient and extensible control that can parse and render markdown.
    /// </summary>
    public partial class MarkdownTextBlock
    {
        /// <summary>
        /// Calls OnPropertyChanged.
        /// </summary>
        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as MarkdownTextBlock;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        /// <summary>
        /// Fired when the value of a DependencyProperty is changed.
        /// </summary>
        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop)
        {
            RenderMarkdown();
        }

        /// <summary>
        /// Fired when a user taps one of the link elements
        /// </summary>
        private void Hyperlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            LinkHandled((string)sender.GetValue(HyperlinkUrlProperty), LinkReturnType.Hyperlink);
        }

        /// <summary>
        /// Fired when a user taps one of the image elements
        /// </summary>
        private void NewImagelink_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            LinkHandled((string)(sender as Image).GetValue(HyperlinkUrlProperty), LinkReturnType.Image);
        }

        /// <summary>
        /// Fired when the text is done parsing and formatting. Fires each time the markdown is rendered.
        /// </summary>
        public event EventHandler<MarkdownRenderedEventArgs> MarkdownRendered;

        /// <summary>
        /// Fired when a link element in the markdown was tapped.
        /// </summary>
        public event EventHandler<LinkClickedEventArgs> LinkClicked;

        /// <summary>
        /// Fired when an image from the markdown document needs to be resolved.
        /// The default implementation is basically <code>new BitmapImage(new Uri(e.Url));</code>.
        /// <para/>You must set <see cref="ImageResolvingEventArgs.Handled"/> to true in order to process your changes.
        /// </summary>
        public event EventHandler<ImageResolvingEventArgs> ImageResolving;

        /// <summary>
        /// Fired when a Code Block is being Rendered.
        /// The default implementation is to output the CodeBlock as Plain Text.
        /// <para/>You must set <see cref="CodeBlockResolvingEventArgs.Handled"/> to true in order to process your changes.
        /// </summary>
        public event EventHandler<CodeBlockResolvingEventArgs> CodeBlockResolving;
    }
}