using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterLoginExample
{
    public partial class PopupForm : Form
    {
        private Uri navigateToUrl;
        public Uri actualUrl;
        public PopupForm()
        {
            InitializeComponent();
            webBrowser1.AllowNavigation = true;
            webBrowser1.Navigating += webBrowserNavigating;

        }

        private void webBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (navigateToUrl.Host != e.Url.Host)
            {
                actualUrl = e.Url;
                this.Close();
            }
        }

        public void navigateTo(string url)
        {
            navigateToUrl = new Uri(url);
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
