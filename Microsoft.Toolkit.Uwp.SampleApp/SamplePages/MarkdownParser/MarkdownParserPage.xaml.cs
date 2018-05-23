// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MarkdownParserPage : Page
    {
        public MarkdownParserPage()
        {
            this.InitializeComponent();
            this.Loaded += MarkdownParserPage_Loaded;
        }

        private void MarkdownParserPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateMDResult();
        }

        private void RawMarkdown_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMDResult();
        }

        private void UpdateMDResult()
        {
            var document = new MarkdownDocument();
            document.Parse(RawMarkdown.Text);

            var json = JsonConvert.SerializeObject(document, Formatting.Indented, new StringEnumConverter());
            MarkdownResult.Text = json;
        }
    }
}