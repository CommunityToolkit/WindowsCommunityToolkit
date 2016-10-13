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
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public partial class CodeRenderer : Control
    {
        private ProgressRing _progress;
        private WebView _webView;
        private Button _printButton;
        private PrintHelper _printHelper;
        private Grid _container;

        private bool _isInitialized;

        public CodeRenderer()
        {
            DefaultStyleKey = typeof(CodeRenderer);
        }

        protected override void OnApplyTemplate()
        {
            if (_webView != null)
            {
                _webView.LoadCompleted -= OnLoadCompleted;
            }

            if (_printButton != null)
            {
                _printButton.Click -= PrintButton_Click;
            }

            _progress = GetTemplateChild("progress") as ProgressRing;
            _webView = GetTemplateChild("webView") as WebView;
            _printButton = GetTemplateChild("PrintButton") as Button;
            _container = GetTemplateChild("Container") as Grid;

            if (_webView != null)
            {
                _webView.LoadCompleted += OnLoadCompleted;
            }

            if (_printButton != null)
            {
                _printButton.Click += PrintButton_Click;
            }

            _isInitialized = true;

            SetHtmlSource(HtmlSource);
            SetCSharpSource(CSharpSource);
            SetXamlSource(XamlSource);
            SetXmlSource(XmlSource);
            SetJsonSource(JsonSource);

            base.OnApplyTemplate();
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            // await _webView.InvokeScriptAsync("eval", new[] { "window.print();" });

            if (_printHelper == null)
            {
                _printHelper = new PrintHelper(_container);

                double originalWidth = _webView.Width;
                var widthString = await _webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
                int contentWidth;

                if (!int.TryParse(widthString, out contentWidth))
                {
                    throw new Exception(string.Format("failure/width:{0}", widthString));
                }

                _webView.Width = contentWidth;

                // resize height to content
                double originalHeight = _webView.Height;
                var heightString = await _webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
                int contentHeight;

                if (!int.TryParse(heightString, out contentHeight))
                {
                    throw new Exception(string.Format("failure/height:{0}", heightString));
                }

                _webView.Height = contentHeight;

                WebViewBrush brush = new WebViewBrush();
                brush.SetSource(_webView);
                brush.Stretch = Stretch.Uniform;

                brush.Redraw();

                // reset, return
                _webView.Width = originalWidth;
                _webView.Height = originalHeight;

                // Send to print
                var rect = new Rectangle();
                rect.Fill = brush;

                _printHelper.ElementsToPrint.Add(rect);
            }

            await _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App");

            Shell.Current.DisplayWaitRing = false;
        }

        private async Task ShowDocument(string docText, string pattern)
        {
            if (_webView != null)
            {
                HideWebView();

                var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Html/{pattern}"));
                var patternText = await FileIO.ReadTextAsync(patternFile);

                docText = docText.Replace("<", "&lt;").Replace(">", "&gt;");

                string html = patternText.Replace("[CODE]", docText);

                _webView.NavigateToString(html);
            }
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            ShowWebView();
        }

        private string IndentXml(string xml)
        {
            try
            {
                var xdoc = XDocument.Parse(xml);

                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    xdoc.Save(writer);
                }

                return sb.ToString();
            }
            catch
            {
                return xml;
            }
        }

        private string IndentJson(string json)
        {
            try
            {
                var parsedJson = JsonConvert.DeserializeObject(json);
                string indented = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                return indented;
            }
            catch (Exception)
            {
                return json;
            }
        }

        private void ShowWebView()
        {
            _progress.IsActive = false;
            _webView.Visibility = Visibility.Visible;
        }

        private void HideWebView()
        {
            _progress.IsActive = true;
            _webView.Visibility = Visibility.Collapsed;
        }
    }
}
