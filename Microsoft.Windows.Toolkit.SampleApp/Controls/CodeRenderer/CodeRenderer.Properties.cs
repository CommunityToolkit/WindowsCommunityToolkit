// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.SampleApp.Controls
{
    public partial class CodeRenderer
    {
        public static readonly DependencyProperty HtmlSourceProperty = DependencyProperty.Register("HtmlSource", typeof(string), typeof(CodeRenderer), new PropertyMetadata(null, HtmlSourceChanged));
        public static readonly DependencyProperty CSharpSourceProperty = DependencyProperty.Register("CSharpSource", typeof(string), typeof(CodeRenderer), new PropertyMetadata(null, CSharpSourceChanged));
        public static readonly DependencyProperty JsonSourceProperty = DependencyProperty.Register("JsonSource", typeof(string), typeof(CodeRenderer), new PropertyMetadata(null, JsonSourceChanged));
        public static readonly DependencyProperty XamlSourceProperty = DependencyProperty.Register("XamlSource", typeof(string), typeof(CodeRenderer), new PropertyMetadata(null, XamlSourceChanged));
        public static readonly DependencyProperty XmlSourceProperty = DependencyProperty.Register("XmlSource", typeof(string), typeof(CodeRenderer), new PropertyMetadata(null, XmlSourceChanged));

        public string HtmlSource
        {
            get { return (string)GetValue(HtmlSourceProperty); }
            set { SetValue(HtmlSourceProperty, value); }
        }

        private static void HtmlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeRenderer;
            control?.SetHtmlSource(e.NewValue as string);
        }

        private void SetHtmlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                _webView.NavigateToString(str);
            }
        }

        public string CSharpSource
        {
            get { return (string)GetValue(CSharpSourceProperty); }
            set { SetValue(CSharpSourceProperty, value); }
        }

        private static void CSharpSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeRenderer;
            control?.SetCSharpSource(e.NewValue as string);
        }

        private async void SetCSharpSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(str, "CSharp.html");
            }
        }

        public string XamlSource
        {
            get { return (string)GetValue(XamlSourceProperty); }
            set { SetValue(XamlSourceProperty, value); }
        }

        private static void XamlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeRenderer;
            control?.SetXamlSource(e.NewValue as string);
        }

        private async void SetXamlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(str, "Xaml.html");
            }
        }

        public string XmlSource
        {
            get { return (string)GetValue(XmlSourceProperty); }
            set { SetValue(XmlSourceProperty, value); }
        }

        private static void XmlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeRenderer;
            control?.SetXmlSource(e.NewValue as string);
        }

        private async void SetXmlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(IndentXml(str), "Xml.html");
            }
        }

        public string JsonSource
        {
            get { return (string)GetValue(JsonSourceProperty); }
            set { SetValue(JsonSourceProperty, value); }
        }

        private static void JsonSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeRenderer;
            control?.SetJsonSource(e.NewValue as string);
        }

        private async void SetJsonSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(IndentJson(str), "Json.html");
            }
        }

    }
}
