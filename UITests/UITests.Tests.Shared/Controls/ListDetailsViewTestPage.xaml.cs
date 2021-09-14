// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListDetailsViewTestPage : Page
    {
        public ListDetailsViewTestPage()
        {
            this.InitializeComponent();

            ListDetailsView.ItemsSource = new string[]
            {
                "First",
                "Second",
            };
        }
    }
}
