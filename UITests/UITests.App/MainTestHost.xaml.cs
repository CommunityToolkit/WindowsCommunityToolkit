// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UITests.App
{
    /// <summary>
    /// MainPage hosting all other test pages.
    /// </summary>
    public sealed partial class MainTestHost
    {
        private readonly Dictionary<string, Type> pageMap;

        private DispatcherQueue queue;

        public MainTestHost()
        {
            InitializeComponent();
            ((App)Application.Current).host = this;
            pageMap = ((App)Application.Current).TestPages;
            queue = DispatcherQueue.GetForCurrentThread();
        }

        internal bool OpenPage(string pageName)
        {
            try
            {
                ((App)Application.Current).Log("Trying to Load Page: " + pageName);

                // Ensure we're on the UI thread as we'll be called from the AppService now.
                queue.EnqueueAsync(() =>
                {
                    navigationFrame.Navigate(pageMap[pageName]);
                });
            }
            catch (Exception e)
            {
                ((App)Application.Current).Log(string.Format("Exception Loading Page {0}: {1} ", pageName, e.Message));
                return false;
            }

            return true;
        }
    }
}
