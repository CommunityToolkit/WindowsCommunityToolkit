// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using UITests.App.Pages;
using Windows.UI.Xaml;

namespace UITests.App
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSimple_Click(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(typeof(SimpleTest));
        }

        private void btnTextBoxMask_Click(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(typeof(TextBoxMaskTestPage));
        }
    }
}
