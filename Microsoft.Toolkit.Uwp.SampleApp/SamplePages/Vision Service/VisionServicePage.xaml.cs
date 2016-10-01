using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Services.CognitiveServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class VisionServicePage : Page
    {
        private VisionService _visionService;
        private IRandomAccessStream _imageFileSteam;

        public VisionServicePage()
        {
            InitializeComponent();
            Loaded += VisionServicePage_Loaded;

            TagImages.Visibility = Visibility.Collapsed;
            TagUrls.Visibility = Visibility.Collapsed;
            ResultTextbox.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _imageFileSteam?.Dispose();
            base.OnNavigatedFrom(e);
        }

        private void VisionServicePage_Loaded(object sender, RoutedEventArgs e)
        {
            var languageList = new List<ComboboxItemValue>
            {
                new ComboboxItemValue {Text = "AutoDetect", Value = "unk"},
                new ComboboxItemValue {Text = "English", Value = "en"}
            };

            var detectOrientationList = new List<ComboboxItemValue>
            {
                new ComboboxItemValue { Text = "False", Value = "False" },
                new ComboboxItemValue { Text = "True", Value = "True" }
            };
            OcrImageLanguages.ItemsSource = OcrUrlLanguages.ItemsSource = languageList;
            OcrImageDetectOrientation.ItemsSource = OcrUrlDetectOrientation.ItemsSource = detectOrientationList;

            OcrImageDetectOrientation.SelectedIndex =
                OcrImageLanguages.SelectedIndex =
                OcrUrlDetectOrientation.SelectedIndex =
                OcrUrlLanguages.SelectedIndex = 0;
        }

        private void SetApiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            _visionService = new VisionService(ApiKey.Text);
            TagImages.Visibility = Visibility.Visible;
            TagUrls.Visibility = Visibility.Visible;
            ResultTextbox.Visibility = Visibility.Visible;
            HideApiPanel();
        }

        private async void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".gif");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");
            StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                _imageFileSteam = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(_imageFileSteam.CloneStream());
                PickerDisplayImage.Source = bitmapImage;
            }
        }

        private async void TagImageFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null)
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var result = await _visionService.GetTagsAsync(_imageFileSteam);
            ResultTextbox.Text = result.ToString();

            Shell.Current.DisplayWaitRing = false;
        }

        private async void OcrImageFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null || _imageFileSteam == null)
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var selectedLanguage = (OcrImageLanguages.SelectedItem as ComboboxItemValue)?.Value;
            var selectedOrientation = (OcrImageDetectOrientation.SelectedItem as ComboboxItemValue)?.Value;
            bool detectOrientation = selectedOrientation != null && bool.Parse(selectedOrientation);

            var result = await _visionService.OcrAsync(_imageFileSteam, selectedLanguage, detectOrientation);
            ResultTextbox.Text = result.ToString();

            Shell.Current.DisplayWaitRing = false;
        }

        private async void TagUrlFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null || string.IsNullOrWhiteSpace(ImageUrl.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var result =
                        await
                            _visionService.GetTagsAsync(ImageUrl.Text);

            ResultTextbox.Text = result.ToString();
            Shell.Current.DisplayWaitRing = false;
        }

        private async void OcrUrlFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null || string.IsNullOrWhiteSpace(ImageUrl.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var selectedLanguage = (OcrUrlLanguages.SelectedItem as ComboboxItemValue)?.Value;
            var selectedOrientation = (OcrUrlDetectOrientation.SelectedItem as ComboboxItemValue)?.Value;
            bool detectOrientation = selectedOrientation != null && bool.Parse(selectedOrientation);

            var result = await _visionService.OcrAsync(ImageUrl.Text, selectedLanguage, detectOrientation);
            ResultTextbox.Text = result.ToString();

            Shell.Current.DisplayWaitRing = false;
        }

        private void ApiKeyPanelExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApiKeyPanel.Visibility == Visibility.Visible)
            {
                HideApiPanel();
            }
            else
            {
                ShowApiPanel();
            }
        }

        private void ShowApiPanel()
        {
            ApiKeyPanelExpandButton.Content = "";
            ApiKeyPanel.Visibility = Visibility.Visible;
        }

        private void HideApiPanel()
        {
            ApiKeyPanelExpandButton.Content = "";
            ApiKeyPanel.Visibility = Visibility.Collapsed;
        }

        private void TagImagesExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagImagePanel.Visibility == Visibility.Visible)
            {
                HideTagImagePanel();
            }
            else
            {
                ShowTagImagePanel();
            }
        }

        private void ShowTagImagePanel()
        {
            TagImagesExpandButton.Content = "";
            TagImagePanel.Visibility = Visibility.Visible;
        }

        private void HideTagImagePanel()
        {
            TagImagesExpandButton.Content = "";
            TagImagePanel.Visibility = Visibility.Collapsed;
        }

        private void TagUrlExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagUrlPanel.Visibility == Visibility.Visible)
            {
                HideTagUrlPanel();
            }
            else
            {
                ShowTagUrlPanel();
            }
        }

        private void ShowTagUrlPanel()
        {
            TagUrlExpandButton.Content = "";
            TagUrlPanel.Visibility = Visibility.Visible;
        }

        private void HideTagUrlPanel()
        {
            TagUrlExpandButton.Content = "";
            TagUrlPanel.Visibility = Visibility.Collapsed;
        }

        internal class ComboboxItemValue
        {
            public string Text { get; set; }

            public string Value { get; set; }
        }


    }
}
