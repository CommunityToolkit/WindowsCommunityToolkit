// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using Microsoft.Toolkit.Uwp.UI;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    internal enum FilePickerView
    {
        FileListing,
        FilenameEntry,
        FileOverwriteConfirmation
    }

    /// <summary>
    /// Provides file picker dialogs optimized for gaze input
    /// </summary>
    public partial class GazeFilePicker : ContentDialog
    {
        private bool _dialogInitialized;
        private bool _refreshNeeded;
        private ResourceLoader _resourceLoader;

        internal FilePickerView CurrentView { get; private set; }

        internal Button SelectButton { get; set; }

        /// <summary>
        /// Gets or sets the event handler that is called when the file picker has fully initialized
        /// </summary>
        public EventHandler FilePickerInitialized { get; protected set; }

        /// <summary>
        /// Gets or sets the currently selected file in the dialog as a StorageFile
        /// </summary>
        public StorageFile SelectedItem { get; protected set; }

        internal Button Button0 { get; private set; }

        internal Button Button1 { get; private set; }

        internal Button Button2 { get; private set; }

        internal Button Button3 { get; private set; }

        internal TextBox FilenameTextbox { get; private set; }

        private StorageFolder _currentFolder;

        internal static readonly DependencyProperty CurrentSelectedItemProperty =
            DependencyProperty.Register(
                "CurrentSelectedItem",
                typeof(StorageItem),
                typeof(GazeFilePicker),
                null);

        internal StorageItem CurrentSelectedItem
        {
            get { return (StorageItem)GetValue(CurrentSelectedItemProperty); }
            set { SetValue(CurrentSelectedItemProperty, value); }
        }

        internal static readonly DependencyProperty CurrentFolderItemsProperty =
            DependencyProperty.Register(
                "CurrentFolderItems",
                typeof(ObservableCollection<StorageItem>),
                typeof(GazeFilePicker),
                null);

        internal ObservableCollection<StorageItem> CurrentFolderItems
        {
            get { return (ObservableCollection<StorageItem>)GetValue(CurrentFolderItemsProperty); }
            set { SetValue(CurrentFolderItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current folder for the file picker dialog
        /// </summary>
        public StorageFolder CurrentFolder
        {
            get
            {
                return _currentFolder;
            }

            set
            {
                RefreshContents(value);
            }
        }

        /// <summary>
        /// Gets or sets the list of storage folders that appear as shortcuts
        /// on top of the folder view
        /// </summary>
        public List<StorageFolder> Favorites { get; set; }

        /// <summary>
        /// Gets or sets the collection of file types that the file open picker displays.
        /// </summary>
        public List<string> FileTypeFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GazeFilePicker"/> class.
        /// </summary>
        public GazeFilePicker()
        {
            this.InitializeComponent();

            this.Loaded += this.OnGazeFilePickerLoaded;

            FileTypeFilter = new List<string>();

            var resourcePath = "Microsoft.Toolkit.Uwp.Input.GazeControls/Resources";
            _resourceLoader = ResourceLoader.GetForViewIndependentUse(resourcePath);
        }

        private void OnGazeFilePickerLoaded(object sender, RoutedEventArgs e)
        {
            Button0 = Col0Button;
            Button1 = Col1Button;
            Button2 = Col2Button;
            Button3 = Col3Button;
            FilenameTextbox = FilenameEditBox;

            GazeInput.SetMaxDwellRepeatCount(this, 2);

            var uri = new Uri($"ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/FilenameEntry.xaml");
            GazeKeyboard.LayoutUri = uri;
            GazeKeyboard.Target = FilenameTextbox;

            var dialogSpace = this.FindDescendant("DialogSpace") as Grid;
            var commandSpace = this.FindDescendant("CommandSpace") as Grid;
            dialogSpace.Children.Remove(commandSpace);

            SetFilePickerView(FilePickerView.FileListing);
            CreateFavoritesButtons();
            SetFileTypeFilterText();

            _dialogInitialized = true;

            if (_refreshNeeded)
            {
                RefreshContents(_currentFolder);
            }

            if (FilePickerInitialized != null)
            {
                FilePickerInitialized(this, null);
            }
        }

        /// <summary>
        /// Returns the localized string from resources
        /// </summary>
        /// <param name="resource">Resource id of the string to get</param>
        /// <returns>Localized string</returns>
        protected string GetString(string resource)
        {
            return _resourceLoader.GetString(resource);
        }

        private void SetFileTypeFilterText()
        {
            var filters = FileTypeFilter.Select(item => Regex.Replace(item, "^\u002E", "*."));
            var allFilters = string.Join(", ", filters);
            FileTypeFilterTextBlock.Text = allFilters;
        }

        private async void CreateFavoritesButtons()
        {
            var favoritesPanel = this.FindDescendant("FavoritesPanel") as StackPanel;
            Debug.Assert(favoritesPanel != null, "KnownFoldersPanel not found");

            var style = (Style)this.Resources["PickerButtonStyles"];

            favoritesPanel.Children.Clear();

            List<StorageFolder> favorites;
            if ((Favorites == null) || (Favorites.Count == 0))
            {
                KnownFolderId[] knownFolderIds =
                {
                    KnownFolderId.DocumentsLibrary,
                    KnownFolderId.PicturesLibrary,
                    KnownFolderId.VideosLibrary,
                    KnownFolderId.MusicLibrary
                };

                favorites = new List<StorageFolder>();
                foreach (var folderId in knownFolderIds)
                {
                    try
                    {
                        var knownFolder = await KnownFolders.GetFolderAsync(folderId);
                        favorites.Add(knownFolder);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            else
            {
                favorites = Favorites;
            }

            foreach (var folder in favorites)
            {
                var button = new Button();
                button.Style = style;
                button.Tag = folder;
                button.Click += OnFavoritesClick;
                button.Content = folder.Name;

                favoritesPanel.Children.Add(button);
            }
        }

        /// <summary>
        /// Helper to create a folder in _currentFolder
        /// </summary>
        /// <returns>Task</returns>
        protected async Task CreateFolderAsync()
        {
            await _currentFolder.CreateFolderAsync(FilenameTextbox.Text);
            RefreshContents(_currentFolder);
        }

        /// <summary>
        /// Helper to create a file in the current folder
        /// </summary>
        /// <returns>IAsyncOperation[StorageFile]</returns>
        protected IAsyncOperation<StorageFile> CreateFileAsync()
        {
            return _currentFolder.CreateFileAsync(FilenameTextbox.Text);
        }

        internal void SetFilePickerView(FilePickerView filePickerView)
        {
            Grid[] viewGrids = { FileListingGrid, FilenameEntryGrid, FileOverwriteConfirmationGrid };
            foreach (var grid in viewGrids)
            {
                grid.Visibility = Visibility.Collapsed;
            }

            viewGrids[(int)filePickerView].Visibility = Visibility.Visible;
            CurrentView = filePickerView;
            FixFileListingButtons();
        }

        private async void FixFileListingButtons()
        {
            var folder = await _currentFolder.GetParentAsync();
            ParentFolderButton.IsEnabled = folder != null;

            SelectButton.IsEnabled = (CurrentSelectedItem != null) && (!CurrentSelectedItem.IsFolder);
        }

        private Task[] GetThumbnailsAsync(ObservableCollection<StorageItem> storageItems)
        {
            Task[] tasks = new Task[storageItems.Count];
            for (int i = 0; i < storageItems.Count; i++)
            {
                tasks[i] = storageItems[i].GetThumbnailAsync();
            }

            return tasks;
        }

        private async void RefreshContents(StorageFolder folder)
        {
            if (folder == null)
            {
                return;
            }

            _currentFolder = folder;

            if (!_dialogInitialized)
            {
                _refreshNeeded = true;
                return;
            }

            _refreshNeeded = false;

            var allItems = await folder.GetItemsAsync();
            var items = allItems.Where(item => item.IsOfType(StorageItemTypes.Folder) ||
                                          FileTypeFilter.Contains((item as StorageFile).FileType));
            var currentFolderItems = new ObservableCollection<StorageItem>(items.Select(item => new StorageItem(item)));

            var tasks = GetThumbnailsAsync(currentFolderItems);
            await Task.WhenAll(tasks);

            SelectedItemScrollViewer.ChangeView(SelectedItemScrollViewer.ExtentWidth, 0.0, 1.0f);

            CurrentSelectedItem = new StorageItem(_currentFolder);
            CurrentFolderItems = currentFolderItems;

            FixFileListingButtons();
        }

        private void OnCurrentFolderContentsItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem as StorageItem;
            var selectedItem = CurrentFolderContents.SelectedItem as StorageItem;

            if (clickedItem.IsFolder)
            {
                RefreshContents(clickedItem.Item as StorageFolder);
                SelectButton.IsEnabled = false;
            }
            else
            {
                SelectButton.IsEnabled = true;
            }

            CurrentSelectedItem = clickedItem;
        }

        private async void OnParentFolderClick(object sender, RoutedEventArgs e)
        {
            var folder = await _currentFolder.GetParentAsync();
            RefreshContents(folder);
        }

        private void OnFavoritesClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var folder = button.Tag as StorageFolder;
            RefreshContents(folder);
        }
    }
}
