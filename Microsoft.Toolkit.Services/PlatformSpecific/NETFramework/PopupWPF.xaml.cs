// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    /// <summary>
    /// Interaction logic for PopupWPF.xaml
    /// </summary>
    public partial class PopupWPF : Window
    {
        private string initialHost;
        private string callbackHost;

        /// <summary>
        /// Gets or sets the current URL before closing the form
        /// </summary>
        public Uri ActualUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWPF"/> class.
        /// </summary>
        /// <param name="callbackUrl">Uri callback url</param>
        public PopupWPF(Uri callbackUrl)
        {
            InitializeComponent();

            WebView1.NavigationStarting += (s, e) => WebViewNavigationStartingHandler(e.Uri);
            callbackHost = GetTopLevelDomain(callbackUrl);
        }

        private void WebViewNavigationStartingHandler(Uri uri)
        {
            var topLevelDomain = GetTopLevelDomain(uri);
            if (initialHost != topLevelDomain && topLevelDomain == callbackHost)
            {
                ActualUrl = uri;
                this.Close();
            }
        }

        /// <summary>
        /// Loads a given url in the WebView
        /// </summary>
        /// <param name="url">Url string to navigate to.</param>
        public void NavigateTo(string url)
        {
            initialHost = GetTopLevelDomain(url);
            WebView1.Navigate(url);
        }

        private string GetTopLevelDomain(string url)
        {
            return GetTopLevelDomain(new Uri(url));
        }

        private string GetTopLevelDomain(Uri url)
        {
            var hostParts = url.Host.Split('.').Select(x => x.ToString());
            if (hostParts.Count() > 1)
            {
                return hostParts.ElementAt(1);
            }

            return hostParts.ElementAt(0);
        }
    }
}
