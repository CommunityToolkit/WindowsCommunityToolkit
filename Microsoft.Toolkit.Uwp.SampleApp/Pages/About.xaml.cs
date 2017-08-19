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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.Pages
{
    public sealed partial class About : INotifyPropertyChanged
    {
        private IEnumerable<Sample> _recentSamples;

        public IEnumerable<Sample> RecentSamples
        {
            get
            {
                return _recentSamples;
            }

            set
            {
                _recentSamples = value;
                OnPropertyChanged();
            }
        }

        private List<GitHubRelease> _githubReleases;

        public List<GitHubRelease> GitHubReleases
        {
            get
            {
                return _githubReleases;
            }

            set
            {
                _githubReleases = value;
                OnPropertyChanged();
            }
        }

        public About()
        {
            InitializeComponent();
            var t = Init();
        }

        public static Visibility VisibleIfCollectionEmpty(IEnumerable<Sample> collection)
        {
            return collection != null && collection.Count() > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.ShowOnlyHeader("About");

            var packageVersion = Package.Current.Id.Version;
            Version.Text = $"Version {packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
        }

        private async Task Init()
        {
            RecentSamples = await Samples.GetRecentSamples();
            GitHubReleases = await Data.GitHub.GetPublishedReleases();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if (button.DataContext is Sample sample)
            {
                Shell.Current.NavigateToSample(sample);
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
