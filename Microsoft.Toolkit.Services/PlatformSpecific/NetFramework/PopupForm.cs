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
        public Uri actualUrl;
        private string callbackHost;
        public PopupForm(Uri callbackUrl)
        {
            InitializeComponent();
            webBrowser1.AllowNavigation = true;
            webBrowser1.Navigating += webBrowserNavigating;
            callbackHost = GetTopLevelDomain(callbackUrl);
        }

        private void webBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (initialHost != GetTopLevelDomain(e.Url))
            {
                if (GetTopLevelDomain(e.Url) == callbackHost)
                {
                    actualUrl = e.Url;
                }

                this.Close();
            }
        }

        public void navigateTo(string url)
        {
            initialHost = GetTopLevelDomain(url);
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

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