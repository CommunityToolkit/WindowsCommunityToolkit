// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the FadeHeaderBehavior
    /// </summary>
    public sealed partial class FadeHeaderBehaviorPage : Page, IXamlRenderListener
    {
        private ListView myListView;

        public FadeHeaderBehaviorPage()
        {
            InitializeComponent();

            // If you wanted to use C# instead of XAML to attach the behavior, you can do it like this
            // Interaction.GetBehaviors(MyListView).Add(new FadeHeaderBehavior());
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            myListView = control.FindChildByName("MyListView") as ListView;

            // Load the ListView with Sample Data
            if (myListView != null)
            {
                myListView.ItemsSource = GenerateItems();
            }
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Generates sample data for the ListView
        /// </summary>
        /// <returns>List of strings titles using the loop number that generated the item</returns>
        private static List<string> GenerateItems()
        {
            var list = new List<string>();

            for (var i = 1; i < 21; i++)
            {
                list.Add($"Item {i}");
            }

            return list;
        }
    }
}
