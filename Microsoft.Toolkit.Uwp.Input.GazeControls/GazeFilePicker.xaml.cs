// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    /// <summary>
    /// Provides file picker dialogs optimized for gaze input
    /// </summary>
    public sealed partial class GazeFilePicker : ContentDialog, INotifyPropertyChanged
    {
        private Grid _commandSpaceGrid;
        private Button _newFolderButton;
        private Button _enterFilenameButton;
        private Button _selectButton;
        private Button _cancelButton;
        private DispatcherTimer _initializationTimer;
        private bool _dialogInitialized;
        private bool _refreshNeeded;
        private bool _newFolderMode;

        private ObservableCollection<StorageItem> _currentFolderItems;

        private StorageItem _curSelectedItem;

        /// <summary>
        /// Gets or sets a value indicating whether this is FileSave dialog or a FileOpen dialog
        /// </summary>
        public bool SaveMode { get; set; }

        /// <summary>
        /// Gets the currently selected file in the dialog as a StorageFile
        /// </summary>
        public StorageFile SelectedItem { get; private set; }

        private StorageFolder _currentFolder;

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

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GazeFilePicker"/> class.
        /// </summary>
        public GazeFilePicker()
        {
            this.InitializeComponent();

            this.Opened += OnGazeFilePickerOpened;

            FileTypeFilter = new List<string>();

            _initializationTimer = new DispatcherTimer();
            _initializationTimer.Interval = TimeSpan.FromMilliseconds(125);
            _initializationTimer.Tick += OnInitializationTimerTick;
        }

        private void OnInitializationTimerTick(object sender, object e)
        {
            _initializationTimer.Stop();
            CreateFavoritesButtons();
            CreateCommandSpaceButtons();
            GazeKeyboard.Target = FilenameTextbox;
            SetFileListingsLayout();
            SetFileTypeFilterText();
            _dialogInitialized = true;
            if (_refreshNeeded)
            {
                RefreshContents(_currentFolder);
            }
        }

        private void SetFileTypeFilterText()
        {
            var filters = FileTypeFilter.Select(item => Regex.Replace(item, "^\u002E", "*."));
            var allFilters = string.Join(", ", filters);
            FileTypeFilterTextBlock.Text = allFilters;
        }

        private async void CreateFavoritesButtons()
        {
            var favoritesPanel = this.FindControl<StackPanel>("FavoritesPanel");
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

        private void CreateCommandSpaceButtons()
        {
            _commandSpaceGrid = this.FindControl<Grid>("CommandSpace");
            Debug.Assert(_commandSpaceGrid != null, "CommandSpaceGrid not found");

            _commandSpaceGrid.Children.Clear();
            _commandSpaceGrid.RowDefinitions.Clear();
            _commandSpaceGrid.ColumnDefinitions.Clear();

            _commandSpaceGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 4; i++)
            {
                _commandSpaceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var style = (Style)this.Resources["CommandSpaceButtonStyles"];
            this.PrimaryButtonStyle = style;
            this.SecondaryButtonStyle = style;
            this.CloseButtonStyle = style;

            _newFolderButton = new Button();
            _newFolderButton.Content = "New Folder...";
            _newFolderButton.Style = style;
            _newFolderButton.Click += OnNewFolderClick;

            _enterFilenameButton = new Button();
            _enterFilenameButton.Content = "Enter file name...";
            _enterFilenameButton.Style = style;
            _enterFilenameButton.Click += OnNewFolderClick;

            _selectButton = _commandSpaceGrid.FindName("PrimaryButton") as Button;
            _selectButton.Click += OnSelectButtonClick;
            _selectButton.Content = "Select";

            _cancelButton = _commandSpaceGrid.FindName("CloseButton") as Button;
            _cancelButton.Click += OnCancelButtonClick;
            _cancelButton.Content = "Cancel";

            _commandSpaceGrid.Children.Add(_newFolderButton);
            _commandSpaceGrid.Children.Add(_enterFilenameButton);
            _commandSpaceGrid.Children.Add(_selectButton);
            _commandSpaceGrid.Children.Add(_cancelButton);

            _enterFilenameButton.Content = "Enter Filename...";
            _enterFilenameButton.Click += OnEnterFilenameButtonClick;

            Grid.SetRow(_newFolderButton, 0);
            Grid.SetRow(_enterFilenameButton, 0);
            Grid.SetRow(_selectButton, 0);
            Grid.SetRow(_cancelButton, 0);

            Grid.SetColumnSpan(_newFolderButton, 1);
            Grid.SetColumnSpan(_enterFilenameButton, 1);
            Grid.SetColumnSpan(_selectButton, 1);
            Grid.SetColumnSpan(_cancelButton, 1);

            Grid.SetColumn(_newFolderButton, 0);
            Grid.SetColumn(_enterFilenameButton, 1);
            Grid.SetColumn(_selectButton, 2);
            Grid.SetColumn(_cancelButton, 3);

            SetFileListingsLayout();
        }

        private async void OnSelectButtonClick(object sender, RoutedEventArgs e)
        {
            if (_newFolderMode)
            {
                _newFolderMode = false;
                await _currentFolder.CreateFolderAsync(FilenameTextbox.Text);
                RefreshContents(_currentFolder);
                SetFileListingsLayout();
            }
            else if (SaveMode)
            {
                SelectedItem = await _currentFolder.CreateFileAsync(FilenameTextbox.Text);
            }
            else if (!_curSelectedItem.IsFolder)
            {
                SelectedItem = _curSelectedItem?.Item as StorageFile;
            }

            SelectedItemScrollViewer.ChangeView(SelectedItemScrollViewer.ExtentWidth, 0.0, 1.0f);
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SetFileListingsLayout();
        }

        private void SetFileListingsLayout()
        {
            FileListingGrid.Visibility = Visibility.Visible;
            FilenameEntryGrid.Visibility = Visibility.Collapsed;

            var vis = SaveMode ? Visibility.Visible : Visibility.Collapsed;
            _newFolderButton.Visibility = vis;
            _enterFilenameButton.Visibility = vis;
        }

        private void SetKeyboardInputLayout()
        {
            FileListingGrid.Visibility = Visibility.Collapsed;
            FilenameEntryGrid.Visibility = Visibility.Visible;

            _newFolderButton.Visibility = Visibility.Collapsed;
            _enterFilenameButton.Visibility = Visibility.Collapsed;
        }

        private void OnEnterFilenameButtonClick(object sender, RoutedEventArgs e)
        {
            _newFolderMode = false;
            SetKeyboardInputLayout();
            var uri = new Uri($"ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/FilenameEntry.xaml");
            GazeKeyboard.LayoutUri = uri;
        }

        private void OnNewFolderClick(object sender, RoutedEventArgs e)
        {
            _newFolderMode = true;
            SetKeyboardInputLayout();
            var uri = new Uri($"ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/FilenameEntry.xaml");
            GazeKeyboard.LayoutUri = uri;
        }

        private void OnGazeFilePickerOpened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            _initializationTimer.Start();
            GazeInput.SetMaxDwellRepeatCount(this, 2);
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
            _curSelectedItem = new StorageItem(_currentFolder);

            if (!_dialogInitialized)
            {
                _refreshNeeded = true;
                return;
            }

            _refreshNeeded = false;

            var allItems = await folder.GetItemsAsync();
            var items = allItems.Where(item => item.IsOfType(StorageItemTypes.Folder) ||
                                          FileTypeFilter.Contains((item as StorageFile).FileType));
            _currentFolderItems = new ObservableCollection<StorageItem>(items.Select(item => new StorageItem(item)));

            var tasks = GetThumbnailsAsync(_currentFolderItems);
            await Task.WhenAll(tasks);
            foreach (var item in _currentFolderItems)
            {
                item.OnPropertyChanged("Thumbnail");
            }

            _selectButton.IsEnabled = !_curSelectedItem.IsFolder;
            SelectedItemScrollViewer.ChangeView(SelectedItemScrollViewer.ExtentWidth, 0.0, 1.0f);
            OnPropertyChanged("_curSelectedItem");
            OnPropertyChanged("_currentFolderItems");

            folder = await _currentFolder.GetParentAsync();
            ParentFolderButton.IsEnabled = folder != null;
        }

        private void OnCurrentFolderContentsItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem as StorageItem;
            var selectedItem = CurrentFolderContents.SelectedItem as StorageItem;

            if (clickedItem.IsFolder)
            {
                RefreshContents(clickedItem.Item as StorageFolder);
                _selectButton.IsEnabled = false;
            }
            else
            {
                _selectButton.IsEnabled = true;
            }

            _curSelectedItem = clickedItem;

            OnPropertyChanged("_curSelectedItem");
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

        private void OnFilePickerClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if ((FileListingGrid.Visibility == Visibility.Collapsed) &&
                (args.Result == ContentDialogResult.None || _newFolderMode))
            {
                args.Cancel = true;
                return;
            }
        }
    }
}
