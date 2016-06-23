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
using System.Threading.Tasks;

using Microsoft.Windows.Toolkit.SampleApp.Pages;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public static Shell Current { get; private set; }

        public Shell()
        {
            InitializeComponent();

            Current = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get list of samples
            HamburgerMenu.ItemsSource = await Samples.GetCategoriesAsync();

            // Options
            HamburgerMenu.OptionsItemsSource = new[] { new Option { Glyph = "", Name = "About", PageType = typeof(About) } };
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as SampleCategory;

            if (category != null)
            {
                SetHeadersVisibility(false);
                NavigationFrame.Navigate(typeof(SamplePicker), category);
            }
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option != null)
            {
                SetHeadersVisibility(false);
                NavigationFrame.Navigate(option.PageType);
            }
        }

        public void ShowOnlyHeader(string title)
        {
            Header.Visibility = Visibility.Visible;
            Title.Text = title;
            Properties.Visibility = Visibility.Collapsed;
            CodePanel.Visibility = Visibility.Collapsed;

            CommandsPanel.Children.Clear();
        }

        private void SetHeadersVisibility(bool visible)
        {
            Header.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Footer.IsOpen = false;
            Footer.ClosedDisplayMode = visible ? AppBarClosedDisplayMode.Compact : AppBarClosedDisplayMode.Hidden;
            CommandsPanel.Children.Clear();
        }

        public async Task NavigateToSampleAsync(Sample sample)
        {
            var pageType = Type.GetType("Microsoft.Windows.Toolkit.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                SetHeadersVisibility(true);
                var propertyDesc = await sample.GetPropertyDescriptorAsync();
                DataContext = sample;
                Title.Text = sample.Name;

                Properties.Visibility = (propertyDesc != null && propertyDesc.Options.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

                XAMLSampleButton.Visibility = propertyDesc != null ? Visibility.Visible : Visibility.Collapsed;
                CodeSampleButton.Visibility = propertyDesc != null ? Visibility.Collapsed : Visibility.Visible;

                NavigationFrame.Navigate(pageType, propertyDesc);
            }
        }

        public void RegisterNewCommand(string name, RoutedEventHandler action)
        {
            var commandButton = new Button
            {
                Content = name,
                Margin = new Thickness(10, 5, 10, 5),
                Foreground = Title.Foreground
            };

            commandButton.Click += action;

            CommandsPanel.Children.Add(commandButton);
        }

        private void XAMLSampleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sample = DataContext as Sample;

            if (sample != null)
            {
                CodeRenderer.XamlSource = sample.UpdatedXamlCode;
            }

            CodePanel.Visibility = Visibility.Visible;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CodePanel.Visibility = Visibility.Collapsed;
        }

        private async void CodeSampleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sample = DataContext as Sample;

            if (sample != null)
            {
                CodeRenderer.CSharpSource = await sample.GetCSharpSource();
            }

            CodePanel.Visibility = Visibility.Visible;
        }
    }
}
