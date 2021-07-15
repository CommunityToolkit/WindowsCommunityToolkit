using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    /// <summary>
    /// This class provides a file-save picker dialog for UWP apps that is optimized for gaze input
    /// </summary>
    public sealed class GazeFileSavePicker : GazeFilePicker
    {
        private bool _newFolderMode;
        private Button _newFolderButton;
        private Button _enterFilenameButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="GazeFileSavePicker"/> class.
        /// </summary>
        public GazeFileSavePicker()
        {
            Title = GetString("FileSave/Title");
            FilePickerInitialized += OnGazeFileSavePickerInitialized;
        }

        private void ShowCommandspaceButtons(bool show)
        {
            var visibility = show ? Visibility.Visible : Visibility.Collapsed;
            _newFolderButton.Visibility = visibility;
            _enterFilenameButton.Visibility = visibility;

            _newFolderButton.Content = GetString("NewFolder"); ;
            _enterFilenameButton.Content = GetString("EnterFilename");
            SelectButton.Content = GetString("Save");
            Button3.Content = GetString("Cancel");
        }

        private void OnEnterFilenameClicked(object sender, RoutedEventArgs e)
        {
            FilenameTextbox.Text = string.Empty;
            FilenameTextbox.TextChanged += OnFilenameChanged;
            ShowCommandspaceButtons(false);
            SetFilePickerView(FilePickerView.FilenameEntry);
        }

        private void OnFilenameChanged(object sender, TextChangedEventArgs e)
        {
            SelectButton.IsEnabled = FilenameTextbox.Text.Length > 0;
        }

        private void OnNewFolderClicked(object sender, RoutedEventArgs e)
        {
            _newFolderMode = true;
            FilenameTextbox.Text = string.Empty;
            FilenameTextbox.TextChanged += OnFilenameChanged;
            ShowCommandspaceButtons(false);
            SetFilePickerView(FilePickerView.FilenameEntry);
        }

        private void OnGazeFileSavePickerInitialized(object sender, EventArgs e)
        {
            _newFolderButton = Button0;
            _newFolderButton.Click += OnNewFolderClicked;

            _enterFilenameButton = Button1;
            _enterFilenameButton.Click += OnEnterFilenameClicked;

            SelectButton = Button2;
            SelectButton.Click += OnSaveButtonClicked;

            Button3.Click += OnCloseButtonClicked;
            ShowCommandspaceButtons(true);
        }

        private async void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_newFolderMode)
            {
                _newFolderMode = false;
                await CreateFolderAsync();
                SetFilePickerView(FilePickerView.FileListing);
                ShowCommandspaceButtons(true);
            }
            else
            {
                SelectedItem = await CreateFileAsync();
                Hide();
            }

            FilenameTextbox.TextChanged -= OnFilenameChanged;
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            if (CurrentView == FilePickerView.FileListing)
            {
                Hide();
            }
            else
            {
                SetFilePickerView(FilePickerView.FileListing);
                ShowCommandspaceButtons(true);
            }
        }
    }
}