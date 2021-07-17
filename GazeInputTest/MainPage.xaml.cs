// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Input.GazeControls;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using Windows.ApplicationModel.Core;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GazeInputTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StorageFolder _layoutsFolder;
        private MediaElement _mediaElement;
        private SpeechSynthesizer _speechSynthesizer;

        public MainPage()
        {
            this.InitializeComponent();

            ShowCursor.IsChecked = GazeInput.GetIsCursorVisible(this);

            _mediaElement = new MediaElement();
            _speechSynthesizer = new SpeechSynthesizer();

            GazeInput.IsDeviceAvailableChanged += GazeInput_IsDeviceAvailableChanged;
            GazeInput_IsDeviceAvailableChanged(null, null);
            Loaded += this.OnMainPageLoaded;
        }

        private async void OnMainPageLoaded(object sender, RoutedEventArgs e)
        {
            var uri = new Uri($"ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/MinAAC.xaml");
            var layoutFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            _layoutsFolder = await layoutFile.GetParentAsync();
            await GazeKeyboard.TryLoadLayoutAsync(layoutFile);
            GazeKeyboard.Target = TheTextBox;
            GazeKeyboard.PredictionTargets = new Button[] { Prediction0, Prediction1, Prediction2 };
        }

        private void GazeInput_IsDeviceAvailableChanged(object sender, object e)
        {
            DeviceAvailable.Text = GazeInput.IsDeviceAvailable ? "Eye tracker device available" : "No eye tracker device available";
        }

        private void OnStateChanged(object sender, StateChangedEventArgs ea)
        {
            Dwell.Content = ea.PointerState.ToString();
        }

        private void Dwell_Click(object sender, RoutedEventArgs e)
        {
            Dwell.Content = "Clicked";
        }

        private void ShowCursor_Toggle(object sender, RoutedEventArgs e)
        {
            if (ShowCursor.IsChecked.HasValue)
            {
                GazeInput.SetIsCursorVisible(this, ShowCursor.IsChecked.Value);
            }
        }

        private int _clickCount;

        private void OnLegacyInvoked(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            HowButton.Content = string.Format("{0}: Legacy click", _clickCount);
        }

        private void OnGazeInvoked(object sender, DwellInvokedRoutedEventArgs e)
        {
            _clickCount++;
            HowButton.Content = string.Format("{0}: Accessible click", _clickCount);
            e.Handled = true;
        }

        private void OnInvokeProgress(object sender, DwellProgressEventArgs e)
        {
            if (e.State == DwellProgressState.Progressing)
            {
                ProgressShow.Value = 100.0 * e.Progress;
            }

            ProgressShow.IsIndeterminate = e.State == DwellProgressState.Complete;
            e.Handled = true;
        }

        private async void OnSpawnClicked(object sender, RoutedEventArgs e)
        {
            var newView = CoreApplication.CreateNewView();
            var newViewId = 0;

            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(MainPage), newViewId);
                Window.Current.Content = frame;

                // In Windows 10 UWP we need to activate our view first.
                // Let's do it now so that we can use TryShow...() and SwitchAsync().
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void OnChangeLayout(object sender, RoutedEventArgs e)
        {
            var picker = new GazeFileOpenPicker();
            var library = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
            picker.Favorites = new List<StorageFolder>();
            picker.Favorites.Add(_layoutsFolder);
            picker.Favorites.Add(library.SaveFolder);
            picker.FileTypeFilter.Add(".xaml");
            picker.CurrentFolder = library.SaveFolder;
            await picker.ShowAsync();
            var file = picker.SelectedItem;
            if (file != null)
            {
                await GazeKeyboard.TryLoadLayoutAsync(file);
            }
        }

        private async void OnOpenFile(object sender, RoutedEventArgs e)
        {
            var picker = new GazeFileOpenPicker();
            var library = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
            picker.FileTypeFilter.Add(".txt");
            picker.CurrentFolder = library.SaveFolder;
            await picker.ShowAsync();
            var file = picker.SelectedItem;
            if (file != null)
            {
                TheTextBox.Text = await FileIO.ReadTextAsync(file);
            }
        }

        private async void OnSaveFile(object sender, RoutedEventArgs e)
        {
            var picker = new GazeFileSavePicker();
            var library = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
            picker.FileTypeFilter.Add(".txt");
            picker.CurrentFolder = library.SaveFolder;
            await picker.ShowAsync();
            var file = picker.SelectedItem;
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, TheTextBox.Text);
            }
        }

        private async void OnPlay(object sender, RoutedEventArgs e)
        {
            var text = TheTextBox.Text.ToString();
            var stream = await _speechSynthesizer.SynthesizeTextToStreamAsync(text);
            _mediaElement.SetSource(stream, stream.ContentType);
            _mediaElement.AutoPlay = true;
            _mediaElement.Play();
        }

        private void OnPauseResume(object sender, RoutedEventArgs e)
        {
            GazeKeyboard.IsEnabled = !GazeKeyboard.IsEnabled;
        }
    }
}