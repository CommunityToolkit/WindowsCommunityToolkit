using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.SampleApp.Controls
{
    public partial class CodeRenderer : Control
    {
        private ProgressRing _progress;
        private WebView _webView;

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

            _progress = GetTemplateChild("progress") as ProgressRing;
            _webView = GetTemplateChild("webView") as WebView;


            if (_webView != null)
            {
                _webView.LoadCompleted += OnLoadCompleted;
            }

            _isInitialized = true;

            SetHtmlSource(HtmlSource);
            SetCSharpSource(CSharpSource);
            SetXamlSource(XamlSource);
            SetXmlSource(XmlSource);
            SetJsonSource(JsonSource);

            base.OnApplyTemplate();
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
