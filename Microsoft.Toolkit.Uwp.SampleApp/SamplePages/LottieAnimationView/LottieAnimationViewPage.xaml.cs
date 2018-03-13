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
using System.Collections.Generic;
using System.IO;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// LottieAnimationViewPage sample page
    /// </summary>
    public sealed partial class LottieAnimationViewPage : Page, IXamlRenderListener
    {
        private LottieAnimationView _lottieAnimationView;
        private RangeSelector _rangeSelector;
        private Grid _lottieAnimationViewGrid;

        public LottieAnimationViewPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            _lottieAnimationView = control.FindChildByName("Container") as LottieAnimationView;
            _lottieAnimationView.Loaded += (s, e) =>
            {
                _rangeSelector = VisualTree.FindDescendant<RangeSelector>(Window.Current.Content as Frame);
                UpdateRangeSelector();
            };

            _lottieAnimationViewGrid = control.FindChildByName("LottieAnimationViewGrid") as Grid;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Resume/Pause", ResumePauseButton_Click);
            Shell.Current.RegisterNewCommand("Open File", OpenFileButton_Click);
            Shell.Current.RegisterNewCommand("Background White", BackgroundColorButton_Click);
        }

        private void UpdateRangeSelector()
        {
            if (_rangeSelector != null)
            {
                _rangeSelector.Minimum = _lottieAnimationView.StartFrame;
                _rangeSelector.Maximum = _lottieAnimationView.EndFrame;

                var sample = DataContext as Sample;
                var propDict = sample.PropertyDescriptor.Expando as IDictionary<string, object>;
                (propDict["MinFrame"] as ValueHolder).Value = _rangeSelector.Minimum;
                (propDict["MaxFrame"] as ValueHolder).Value = _rangeSelector.Maximum;
            }
        }

        private void ResumePauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lottieAnimationView.IsAnimating)
            {
                _lottieAnimationView.PauseAnimation();
            }
            else
            {
                if (_lottieAnimationView.TimesRepeated >= _lottieAnimationView.RepeatCount)
                {
                    _lottieAnimationView.PlayAnimation();
                }
                else
                {
                    _lottieAnimationView.ResumeAnimation();
                }
            }
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".json");
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                await _lottieAnimationView.SetAnimationAsync(new StreamReader(await file.OpenStreamForReadAsync()));
                UpdateRangeSelector();
                _lottieAnimationView.PlayAnimation();
            }
        }

        private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
        {
            var backgroundColorButton = (Button)e.OriginalSource;
            var solidColorBrush = (SolidColorBrush)_lottieAnimationViewGrid.Background;
            if (solidColorBrush != null)
            {
                if (solidColorBrush.Color == Colors.White)
                {
                    backgroundColorButton.Content = "Background Black";
                    _lottieAnimationViewGrid.Background = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    backgroundColorButton.Content = "Background White";
                    _lottieAnimationViewGrid.Background = new SolidColorBrush(Colors.White);
                }
            }
        }
    }
}
