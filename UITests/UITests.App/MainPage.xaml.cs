// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UITests.App
{
    public sealed partial class MainPage
    {
        private readonly Dictionary<string, Type> pageMap;

        public MainPage()
        {
            InitializeComponent();
            pageMap = ((App)Application.Current).TestPages;
        }

        private void PageName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && sender is TextBox s)
            {
                OpenPage(s.Text);
            }
        }

        private void OpenPage(string pageName)
        {
            try
            {
                navigationFrame.Navigate(pageMap[pageName]);
            }
            catch (KeyNotFoundException ex)
            {
                throw new Exception("Cannot find page.", ex);
            }
        }
    }
}
