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

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample page demonstrating view extensions
    /// </summary>
    public sealed partial class ViewExtensionsPage : Page
    {
        private const string SBBackgroundColor = "StatusBar_BackgroundColor";
        private const string SBBackgroundOpacity = "StatusBar_BackgroundOpacity";
        private const string SBForegroundColor = "StatusBar_ForegroundColor";
        private const string SBIsVisible = "StatusBar_IsVisible";
        private const string AVTitle = "ApplicationView_Title";
        private const string TBBackgroundColor = "TitleBar_BackgroundColor";
        private const string TBForegroundColor = "TitleBar_ForegroundColor";

        private IDictionary<string, object> _expando;

        public ViewExtensionsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _expando = DataContext as IDictionary<string, object>;
            if (_expando != null)
            {
                foreach (var prop in _expando)
                {
                    var valueHolder = prop.Value as ValueHolder;
                    switch (prop.Key)
                    {
                        case SBBackgroundColor:
                            valueHolder.PropertyChanged += SBBackgroundColor_PropertyChanged;
                            break;
                        case SBBackgroundOpacity:
                            valueHolder.PropertyChanged += SBBackgroundOpacity_PropertyChanged;
                            break;
                        case SBForegroundColor:
                            valueHolder.PropertyChanged += SBForegroundColor_PropertyChanged;
                            break;
                        case SBIsVisible:
                            valueHolder.PropertyChanged += SBIsVisible_PropertyChanged;
                            break;
                        case AVTitle:
                            valueHolder.PropertyChanged += AVTitle_PropertyChanged;
                            break;
                        case TBBackgroundColor:
                            valueHolder.PropertyChanged += TBBackgroundColor_PropertyChanged;
                            break;
                        case TBForegroundColor:
                            valueHolder.PropertyChanged += TBForegroundColor_PropertyChanged;
                            break;
                    }
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            StatusBar.SetIsVisible(this, false);

            ApplicationView.SetTitle(this, string.Empty);

            var lightGreyBrush = (Color)Application.Current.Resources["Grey-04"];
            var brandColor = (Color)Application.Current.Resources["Brand-Color"];

            TitleBar.SetButtonBackgroundColor(this, brandColor);
            TitleBar.SetButtonForegroundColor(this, lightGreyBrush);
            TitleBar.SetBackgroundColor(this, brandColor);
            TitleBar.SetForegroundColor(this, lightGreyBrush);

            if (_expando != null)
            {
                foreach (var prop in _expando)
                {
                    var valueHolder = prop.Value as ValueHolder;
                    switch (prop.Key)
                    {
                        case SBBackgroundColor:
                            valueHolder.PropertyChanged -= SBBackgroundColor_PropertyChanged;
                            break;
                        case SBBackgroundOpacity:
                            valueHolder.PropertyChanged -= SBBackgroundOpacity_PropertyChanged;
                            break;
                        case SBForegroundColor:
                            valueHolder.PropertyChanged -= SBForegroundColor_PropertyChanged;
                            break;
                        case SBIsVisible:
                            valueHolder.PropertyChanged -= SBIsVisible_PropertyChanged;
                            break;
                        case AVTitle:
                            valueHolder.PropertyChanged -= AVTitle_PropertyChanged;
                            break;
                        case TBBackgroundColor:
                            valueHolder.PropertyChanged -= TBBackgroundColor_PropertyChanged;
                            break;
                        case TBForegroundColor:
                            valueHolder.PropertyChanged -= TBForegroundColor_PropertyChanged;
                            break;
                    }
                }
            }
        }

        private void SBBackgroundColor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var value = _expando[SBBackgroundColor] as ValueHolder;
            StatusBar.SetBackgroundColor(this, (value.Value as SolidColorBrush).Color);
        }

        private void SBBackgroundOpacity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var value = _expando[SBBackgroundOpacity] as ValueHolder;
            StatusBar.SetBackgroundOpacity(this, (double)value.Value);
        }

        private void SBForegroundColor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = _expando[SBForegroundColor] as ValueHolder;
            StatusBar.SetForegroundColor(this, (value.Value as SolidColorBrush).Color);
        }

        private void SBIsVisible_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = _expando[SBIsVisible] as ValueHolder;
            StatusBar.SetIsVisible(this, (bool)value.Value);
        }

        private void AVTitle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = _expando[AVTitle] as ValueHolder;
            ApplicationView.SetTitle(this, (string)value.Value);
        }

        private void TBBackgroundColor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = _expando[TBBackgroundColor] as ValueHolder;
            TitleBar.SetBackgroundColor(this, (value.Value as SolidColorBrush).Color);
        }

        private void TBForegroundColor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = _expando[TBForegroundColor] as ValueHolder;
            TitleBar.SetForegroundColor(this, (value.Value as SolidColorBrush).Color);
        }
    }
}
