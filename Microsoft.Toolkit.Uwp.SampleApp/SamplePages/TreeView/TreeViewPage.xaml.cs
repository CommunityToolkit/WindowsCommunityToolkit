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
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TreeViewPage
    {
        public TreeViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<FileSystemViewModel> TreeItems { get; } = new ObservableCollection<FileSystemViewModel>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void TreeViewControl_SelectedItemChanged(object sender, Microsoft.Toolkit.Uwp.UI.Controls.RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TreeItems.Add(new FileSystemViewModel
            {
                StorageItem = KnownFolders.PicturesLibrary
            });
        }
    }
}
