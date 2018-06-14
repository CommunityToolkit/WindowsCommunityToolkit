// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A sample XAML page that shows how to use <see cref="IIncrementalSource{TSource}"/> and <see cref="IncrementalLoadingCollection{TSource, IType}"/> classes.
    /// </summary>
    public sealed partial class IncrementalLoadingCollectionPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalLoadingCollectionPage"/> class.
        /// </summary>
        public IncrementalLoadingCollectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the <see cref="Windows.UI.Xaml.Controls.Page"/> is loaded and becomes the current source of a parent <see cref="Windows.UI.Xaml.Controls.Frame"/>.
        /// </summary>
        /// <param name="e">
        /// Event data that is representative of the pending navigation that will load the current <see cref="Windows.UI.Xaml.Controls.Page"/>. Usually the most relevant property to examine is <see cref="NavigationEventArgs.Parameter"/>.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Refresh Collection", RefreshCollection);

            // IncrementalLoadingCollection can be bound to a GridView or a ListView. In this case it is a ListView called PeopleListView.
            var collection = new IncrementalLoadingCollection<PeopleSource, Person>();
            PeopleListView.ItemsSource = collection;

            // Binds the collection to the page DataContext in order to use its IsLoading and HasMoreItems properties.
            DataContext = collection;
        }

        private async void RefreshCollection(object sender, RoutedEventArgs e)
        {
            var collection = (IncrementalLoadingCollection<PeopleSource, Person>)PeopleListView.ItemsSource;
            await collection.RefreshAsync();
        }
    }
}
