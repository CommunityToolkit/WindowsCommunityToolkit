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