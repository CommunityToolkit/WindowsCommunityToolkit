
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
using WebViewControlNavigationStartingEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    /// <summary>
    /// Interaction logic for PopupWPF.xaml
    /// </summary>
    public partial class PopupWPF : Window
    {

        private string initialHost;
        public Uri actualUrl;
        private string callbackHost;

        public Func<object, object, object> FormClosed { get; internal set; }

        public PopupWPF(Uri callbackUrl)
        {
            InitializeComponent();
         
            WebView1.NavigationStarting += WebViewNavigationStarting;
            callbackHost = GetTopLevelDomain(callbackUrl);
        }

        private void WebViewNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            if (initialHost != GetTopLevelDomain(e.Uri))
            {
                if (GetTopLevelDomain(e.Uri) == callbackHost)
                {
                    actualUrl = e.Uri;
                }

                this.Close();
            }
        }

        public void navigateTo(string url)
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

