// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Input.GazeControls;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using Microsoft.Toolkit.Uwp.UI;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazeControlsPage : IXamlRenderListener
    {
        private const int NUM_PREDICTIONS = 3;
        private MediaElement _mediaElement;
        private SpeechSynthesizer _speechSynthesizer;
        private Button[] _predictions;
        private TextBox _textControl;
        private GazeKeyboard _gazeKeyboard;
        private StorageFolder _layoutsFolder;

        public GazeControlsPage()
        {
            this.InitializeComponent();
            _predictions = new Button[NUM_PREDICTIONS];
            _mediaElement = new MediaElement();
            _speechSynthesizer = new SpeechSynthesizer();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            GazeInput.IsDeviceAvailableChanged += GazeInput_IsDeviceAvailableChanged;

            var predictions = new Button[3];
            predictions[0] = control.FindChild("Prediction0") as Button;
            predictions[1] = control.FindChild("Prediction1") as Button;
            predictions[2] = control.FindChild("Prediction2") as Button;
            _textControl = control.FindChild("TextControl") as TextBox;
            _gazeKeyboard = control.FindChild("GazeKeyboard") as GazeKeyboard;
            var button = control.FindChild("ChangeLayout") as Button;

            button.Click += OnChangeLayout;

            _gazeKeyboard.Target = _textControl;
            _gazeKeyboard.PredictionTargets = predictions;

            var uri = new Uri("ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/MinAAC.xaml");
            var layoutFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            _layoutsFolder = await layoutFile.GetParentAsync();
            await _gazeKeyboard.TryLoadLayoutAsync(layoutFile);

            var speakButton = control.FindChild("SpeakButton") as Button;
            speakButton.Click += OnSpeak;

            var fileOpenButton = control.FindChild("FileOpenButton") as Button;
            fileOpenButton.Click += OnFileOpen;

            var fileSaveButton = control.FindChild("FileSaveButton") as Button;
            fileSaveButton.Click += OnFileSave;

            WarnUserToPlugInDevice();
        }

        private async void OnSpeak(object sender, RoutedEventArgs e)
        {
            var text = _textControl.Text.ToString();
            var stream = await _speechSynthesizer.SynthesizeTextToStreamAsync(text);
            _mediaElement.SetSource(stream, stream.ContentType);
            _mediaElement.AutoPlay = true;
            _mediaElement.Play();
        }

        private async void OnFileOpen(object sender, RoutedEventArgs e)
        {
            var picker = new GazeFileOpenPicker();
            var library = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
            picker.FileTypeFilter.Add(".txt");
            picker.CurrentFolder = library.SaveFolder;
            await picker.ShowAsync();
            if (picker.SelectedItem != null)
            {
                _textControl.Text = await FileIO.ReadTextAsync(picker.SelectedItem);
            }
        }

        private async void OnFileSave(object sender, RoutedEventArgs e)
        {
            var library = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
            var picker = new GazeFileSavePicker();
            picker.FileTypeFilter.Add(".txt");
            picker.CurrentFolder = library.SaveFolder;
            await picker.ShowAsync();
            if (picker.SelectedItem != null)
            {
                await FileIO.WriteTextAsync(picker.SelectedItem, _textControl.Text);
            }
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
                await _gazeKeyboard.TryLoadLayoutAsync(file);
            }
        }

        private void GazeInput_IsDeviceAvailableChanged(object sender, object e)
        {
            WarnUserToPlugInDevice();
        }

        private void WarnUserToPlugInDevice()
        {
            if (GazeInput.IsDeviceAvailable)
            {
                //WarningText.Visibility = Visibility.Collapsed;
            }
            else
            {
                //WarningText.Visibility = Visibility.Visible;
            }
        }

    }
}