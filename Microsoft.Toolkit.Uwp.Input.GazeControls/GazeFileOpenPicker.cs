using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    /// <summary>
    /// Gaze optimized file picker to open files
    /// </summary>
    public sealed class GazeFileOpenPicker : GazeFilePicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GazeFileOpenPicker"/> class.
        /// </summary>
        public GazeFileOpenPicker()
        {
            Title = GetString("FileOpen/Title");
            this.FilePickerInitialized += OnGazeFileOpenPickerInitialized;
        }

        private void OnGazeFileOpenPickerInitialized(object sender, EventArgs e)
        {
            SelectButton = Button2;
            SelectButton.Click += OnOpenButtonClicked;
            SelectButton.Content = GetString("Open");

            Button3.Click += OnCancelClicked;
            Button3.Content = GetString("Cancel");
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OnOpenButtonClicked(object sender, RoutedEventArgs args)
        {
            Debug.Assert(!CurrentSelectedItem.IsFolder, "Selected item should not be a folder!");
            SelectedItem = CurrentSelectedItem?.Item as StorageFile;
            Hide();
        }
    }
}