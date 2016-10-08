// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Windows.UI.Core;
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

            // IncrementalLoadingCollection can be bound to a GridView or a ListView. In this case it is a ListView called PeopleListView.
            var collection = new IncrementalLoadingCollection<PeopleSource, Person>();

            PeopleListView.ItemsSource = collection;

            // Binds the collection to the page DataContext in order to use its IsLoading and HasMoreItems properties.
            DataContext = collection;
        }
    }
}
