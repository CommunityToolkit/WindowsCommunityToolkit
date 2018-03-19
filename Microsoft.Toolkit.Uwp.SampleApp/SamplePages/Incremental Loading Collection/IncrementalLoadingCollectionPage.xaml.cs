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

using Windows.UI.Xaml;

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
            Load();
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Refresh Collection", RefreshCollection);

            // IncrementalLoadingCollection can be bound to a GridView or a ListView. In this case it is a ListView called PeopleListView.
            var collection = new IncrementalLoadingCollection<PeopleSource, Person>();
            PeopleListView.ItemsSource = collection;

            // Binds the collection to the page DataContext in order to use its IsLoading and HasMoreItems properties.
            DataContext = collection;
        }

        private void RefreshCollection(object sender, RoutedEventArgs e)
        {
            var collection = (IncrementalLoadingCollection<PeopleSource, Person>)PeopleListView.ItemsSource;
            collection.Refresh();
        }
    }
}