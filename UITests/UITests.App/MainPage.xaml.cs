// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UITests.App
{
    public sealed partial class MainPage
    {
        private static readonly string _pageNS = typeof(MainPage).Namespace + ".Pages.";
        private NetworkStream clientStream;
        private StreamWriter writer;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(typeof(DefaultPage));

            Task.Run(ConnectToTestDriver);
        }

        private void ConnectToTestDriver()
        {
            var client = new TcpClient();
            client.ConnectAsync(IPAddress.Loopback, 13000);

            clientStream = client.GetStream();
            writer = new StreamWriter(clientStream)
            {
                AutoFlush = true
            };

            writer.WriteLine("Ready");

            var thread = new Thread(new ThreadStart(ListenForPageRequest));
            thread.Start();
        }

        private void ListenForPageRequest()
        {
            var reader = new StreamReader(clientStream);

            while (true)
            {
                try
                {
                    if (reader.ReadLine() is string request)
                    {
                        var type = Type.GetType(_pageNS + request);
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => navigationFrame.Navigate(type))
                            .AsTask()
                            .ContinueWith(_ => writer.WriteLine("Opened " + request));
                    }
                    else
                    {
                        // The server has disconnected
                        break;
                    }
                }
                catch
                {
                    // A socket error has occurred
                    break;
                }
            }
        }
    }
}
