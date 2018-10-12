using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    public partial class PopupForm : Form
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
        public PopupForm(Uri callbackUrl)
        {
            InitializeComponent();

            webView1.NavigationStarting += (s, e) => WebViewNavigationStartingHandler(e.Uri);
            callbackHost = GetTopLevelDomain(callbackUrl);
        }

        private void WebViewNavigationStartingHandler(Uri uri)
        {
            if (initialHost != GetTopLevelDomain(uri))
            {
                if (GetTopLevelDomain(uri) == callbackHost)
                {
                    ActualUrl = uri;
                }

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
            webView1.Navigate(url);
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
