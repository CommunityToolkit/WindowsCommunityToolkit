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

using Microsoft.Toolkit.Uwp.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NetworkHelperPage : Page
    {
        public NetworkHelperPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsInternetAvailableText.Text = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable ? "Yes" : "No";
            IsInternetOnMeteredConnectionText.Text = NetworkHelper.Instance.ConnectionInformation.IsInternetOnMeteredConnection ? "Yes" : "No";
            ConnectionTypeText.Text = NetworkHelper.Instance.ConnectionInformation.ConnectionType.ToString();
            SignalBarsText.Text = NetworkHelper.Instance.ConnectionInformation.SignalStrength.GetValueOrDefault(0).ToString();
            NetworkNamesText.Text = string.Join(", ", NetworkHelper.Instance.ConnectionInformation.NetworkNames);
        }
    }
}
