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
                new ComboboxItemValue { Text = "AutoDetect", Value = "unk"},
                new ComboboxItemValue { Text = "English", Value = "en"}
            };
            var detectOrientationList = new List<ComboboxItemValue>
            {
                new ComboboxItemValue { Text = "False", Value = "False" },
                new ComboboxItemValue { Text = "True", Value = "True" }
            };
            var visualFeaturesList = new List<ComboboxItemValue>
            {
                new ComboboxItemValue { Text = "Categories", Value = "Categories" },
                new ComboboxItemValue { Text = "Tags", Value = "Tags" },
                new ComboboxItemValue { Text = "Description", Value = "Description" },
                new ComboboxItemValue { Text = "Faces", Value = "Faces" },
                new ComboboxItemValue { Text = "ImageType", Value = "ImageType" },
                new ComboboxItemValue { Text = "Color", Value = "Color" },
                new ComboboxItemValue { Text = "Adult", Value = "Adult" }
            };
            OcrImageLanguages.ItemsSource = languageList;
            OcrImageDetectOrientation.ItemsSource = detectOrientationList;
            AnalyzeImageVisualFeatures.ItemsSource = visualFeaturesList;

            OcrImageDetectOrientation.SelectedIndex =
                OcrImageLanguages.SelectedIndex = AnalyzeImageVisualFeatures.SelectedIndex = 0;
        }

        private void SetApiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ApiKey.Text))
            {
                return;
            }

            _visionService = new VisionService(ApiKey.Text);
            TagImages.Visibility = Visibility.Visible;
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
            if (_visionService == null ||
                (_imageFileSteam == null && ImagePanel.Visibility == Visibility.Visible) ||
                (string.IsNullOrWhiteSpace(ImageUrl.Text) && UrlPanel.Visibility == Visibility.Visible))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;
            try
            {
                ImageTags result;
                if (ImagePanel.Visibility == Visibility)
                {
                    result = await _visionService.GetTagsAsync(_imageFileSteam);
                }
                else
                {
                    result = await _visionService.GetTagsAsync(ImageUrl.Text);
                }

                ResultTextbox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                ResultTextbox.Text = ex.ToString();
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void OcrImageFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null ||
                (_imageFileSteam == null && ImagePanel.Visibility == Visibility.Visible) ||
                (string.IsNullOrWhiteSpace(ImageUrl.Text) && UrlPanel.Visibility == Visibility.Visible))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var selectedLanguage = (OcrImageLanguages.SelectedItem as ComboboxItemValue)?.Value;
            var selectedOrientation = (OcrImageDetectOrientation.SelectedItem as ComboboxItemValue)?.Value;
            bool detectOrientation = selectedOrientation != null && bool.Parse(selectedOrientation);

            try
            {
                ImageOCR result;
                if (ImagePanel.Visibility == Visibility)
                {
                    result = await _visionService.OcrAsync(_imageFileSteam, selectedLanguage, detectOrientation);
                }
                else
                {
                    result = await _visionService.OcrAsync(ImageUrl.Text, selectedLanguage, detectOrientation);
                }

                ResultTextbox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                ResultTextbox.Text = ex.ToString();
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void AnalyzeImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null ||
                (_imageFileSteam == null && ImagePanel.Visibility == Visibility.Visible) ||
                (string.IsNullOrWhiteSpace(ImageUrl.Text) && UrlPanel.Visibility == Visibility.Visible))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var visualFeatures = (AnalyzeImageVisualFeatures.SelectedItem as ComboboxItemValue)?.Value;
            var selectedOrientation = AnalyzeImageDetails.Text;

            try
            {
                ImageAnalysis result;
                if (ImagePanel.Visibility == Visibility)
                {
                    result = await _visionService.AnalyzeImageAsync(_imageFileSteam, visualFeatures, selectedOrientation);
                }
                else
                {
                    result = await _visionService.AnalyzeImageAsync(ImageUrl.Text, visualFeatures, selectedOrientation);
                }

                ResultTextbox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                ResultTextbox.Text = ex.ToString();
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void DescribeImage_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null ||
                (_imageFileSteam == null && ImagePanel.Visibility == Visibility.Visible) ||
                (string.IsNullOrWhiteSpace(ImageUrl.Text) && UrlPanel.Visibility == Visibility.Visible))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            int maxNumberOfCandidate = 1;
            if (!int.TryParse(DescribeImageMaxNumberOfCandidate.Text, out maxNumberOfCandidate))
            {
                return;
            }

            try
            {
                ImageDescription result;
                if (ImagePanel.Visibility == Visibility)
                {
                    result = await _visionService.DescribeImageAsync(_imageFileSteam, maxNumberOfCandidate);
                }
                else
                {
                    result = await _visionService.DescribeImageAsync(ImageUrl.Text, maxNumberOfCandidate);
                }

                ResultTextbox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                ResultTextbox.Text = ex.ToString();
            }

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
            if (VisionPanel.Visibility == Visibility.Visible)
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
            VisionPanel.Visibility = Visibility.Visible;
        }

        private void HideTagImagePanel()
        {
            TagImagesExpandButton.Content = "";
            VisionPanel.Visibility = Visibility.Collapsed;
        }

        internal class ComboboxItemValue
        {
            public string Text { get; set; }

            public string Value { get; set; }
        }

        private void ShowImageUrlPanel_Checked(object sender, RoutedEventArgs e)
        {
            ShowImageUrlPanel.Content = "Process From Image";

            ImagePanel.Visibility = Visibility.Collapsed;
            UrlPanel.Visibility = Visibility.Visible;
        }

        private void ShowImageUrlPanel_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowImageUrlPanel.Content = "Process From Url";

            UrlPanel.Visibility = Visibility.Collapsed;
            ImagePanel.Visibility = Visibility.Visible;
        }
    }
}
