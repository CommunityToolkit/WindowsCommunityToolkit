// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    /// <summary>
    /// Redirect form for Twitter Service login.
    /// </summary>
    public partial class PopupForm : Form
    {
        private string initialHost;

        public Uri actualUrl;
        private string callbackHost;
        /// <summary>
        /// Gets or sets the actual url.
        /// </summary>
        public Uri ActualUrl { get => actualUrl; set => actualUrl = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupForm"/> class.
        /// </summary>
        /// <param name="callbackUrl">Uri callback url</param>
        public PopupForm(Uri callbackUrl)
        {
            InitializeComponent();
            webBrowser1.AllowNavigation = true;
            webBrowser1.Navigating += WebBrowserNavigating;
            callbackHost = GetTopLevelDomain(callbackUrl);
        }

        /// <summary>
        /// Checks if the actual page is the Callback url.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">WebBrowser navigating event arguments</param>
        private void WebBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (initialHost != GetTopLevelDomain(e.Url))
            {
                if (GetTopLevelDomain(e.Url) == callbackHost)
                {
                    ActualUrl = e.Url;
                }

                this.Close();
            }
        }

        /// <summary>
        /// Loads a given url in the WebBrowser
        /// </summary>
        /// <param name="url">Url string to navigate to.</param>
        public void navigateTo(string url)
        {
            initialHost = GetTopLevelDomain(url);
            webBrowser1.Navigate(url);
        }

        /// <summary>
        /// empty
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">WebBrowserDocumentCompleteEventArgs</param>
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        /// <summary>
        /// Invoke the GetTopLevelDomain method
        /// </summary>
        /// <param name="url">URL String</param>
        /// <returns>Url's top level domain</returns>
        private string GetTopLevelDomain(string url)
        {
            return GetTopLevelDomain(new Uri(url));
        }

        /// <summary>
        /// Gets the top level domain from a given url
        /// </summary>
        /// <param name="url">URI url</param>
        /// <returns>The top level domain</returns>
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
