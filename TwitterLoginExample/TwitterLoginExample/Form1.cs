using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace TwitterLoginExample
{
    public partial class Form1 : Form
    {

        PopupForm popupForm = new PopupForm();
        Uri newUrl;
        public Form1()
        {
            InitializeComponent();
            popupForm.FormClosed += popupIsClosed;
        }

        private void popupIsClosed(object sender, FormClosedEventArgs e)
        {
            newUrl = popupForm.actualUrl;
            if(newUrl != null)
            {
                var query = HttpUtility.ParseQueryString(newUrl.Query);
                var auth_token = query.Get("oauth_token");
                var auth_verifier = query.Get("oauth_verifier");
            }
        }

        // ref http://stackoverflow.com/a/3978040
        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private HttpListener StartHttpListener(string uri)
        {
            var http = new HttpListener();
            http.Prefixes.Add(uri);
            http.Start();
            return http;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            var http = StartHttpListener($"http://localhost:54501/");
            var oauthToken = "";
            string authorizationRequest = $"https://api.twitter.com/oauth/authorize?oauth_token={oauthToken}&force_login=true";

            popupForm.navigateTo(authorizationRequest);
            popupForm.Show();

        }
    }
}
