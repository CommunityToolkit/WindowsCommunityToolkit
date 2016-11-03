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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using Microsoft.Advertising.WinRT.UI;
#pragma warning disable SA1208 // System using directives must be placed before other using directives
    using System;
#pragma warning restore SA1208 // System using directives must be placed before other using directives
    using Windows.System.Profile;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    // This control written per https://msdn.microsoft.com/windows/uwp/monetize/migrate-from-admediatorcontrol-to-adcontrol

    /// <summary>A UWP AdMediator control</summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class UwpAdMediator : UserControl
    {
        public int MobileAdWidth
        {
            get { return (int)GetValue(MobileAdWidthProperty); }
            set { SetValue(MobileAdWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileAdWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileAdWidthProperty =
            DependencyProperty.Register("MobileAdWidth", typeof(int), typeof(UwpAdMediator), new PropertyMetadata(320));

        public int DesktopAdWidth
        {
            get { return (int)GetValue(DesktopAdWidthProperty); }
            set { SetValue(DesktopAdWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DesktopAdWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesktopAdWidthProperty =
            DependencyProperty.Register("DesktopAdWidth", typeof(int), typeof(UwpAdMediator), new PropertyMetadata(728));

        public int MobileAdHeight
        {
            get { return (int)GetValue(MobileAdHeightProperty); }
            set { SetValue(MobileAdHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileAdHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileAdHeightProperty =
            DependencyProperty.Register("MobileAdHeight", typeof(int), typeof(UwpAdMediator), new PropertyMetadata(50));

        public int DesktopAdHeight
        {
            get { return (int)GetValue(DesktopAdHeightProperty); }
            set { SetValue(DesktopAdHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DesktopAdHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesktopAdHeightProperty =
            DependencyProperty.Register("DesktopAdHeight", typeof(int), typeof(UwpAdMediator), new PropertyMetadata(90));

        public string MobileApplicationId
        {
            get { return (string)GetValue(MobileApplicationIdProperty); }
            set { SetValue(MobileApplicationIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileApplicationId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileApplicationIdProperty =
            DependencyProperty.Register("MobileApplicationId", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public string DesktopApplicationId
        {
            get { return (string)GetValue(DesktopApplicationIdProperty); }
            set { SetValue(DesktopApplicationIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DesktopApplicationId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesktopApplicationIdProperty =
            DependencyProperty.Register("DesktopApplicationId", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public string MobileAdUnit
        {
            get { return (string)GetValue(MobileAdUnitProperty); }
            set { SetValue(MobileAdUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileAdUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileAdUnitProperty =
            DependencyProperty.Register("MobileAdUnit", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public string DesktopAdUnit
        {
            get { return (string)GetValue(DesktopAdUnitProperty); }
            set { SetValue(DesktopAdUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DesktopAdUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesktopAdUnitProperty =
            DependencyProperty.Register("DesktopAdUnit", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public string AdDuplexAppKey
        {
            get { return (string)GetValue(AdDuplexAppKeyProperty); }
            set { SetValue(AdDuplexAppKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdDuplexAppKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdDuplexAppKeyProperty =
            DependencyProperty.Register("AdDuplexAppKey", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public string AdDuplexAdUnit
        {
            get { return (string)GetValue(AdDuplexAdUnitProperty); }
            set { SetValue(AdDuplexAdUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdDuplexAdUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdDuplexAdUnitProperty =
            DependencyProperty.Register("AdDuplexAdUnit", typeof(string), typeof(UwpAdMediator), new PropertyMetadata(null));

        public int AdRefreshSeconds
        {
            get { return (int)GetValue(AdRefreshSecondsProperty); }
            set { SetValue(AdRefreshSecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdRefreshSeconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdRefreshSecondsProperty =
            DependencyProperty.Register("AdRefreshSeconds", typeof(int), typeof(int), new PropertyMetadata(35));

        public int MaxErrorsPerRefresh
        {
            get { return (int)GetValue(MaxErrorsPerRefreshProperty); }
            set { SetValue(MaxErrorsPerRefreshProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxErrorsPerRefresh.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxErrorsPerRefreshProperty =
            DependencyProperty.Register("MaxErrorsPerRefresh", typeof(int), typeof(UwpAdMediator), new PropertyMetadata(3));

        // Dispatch timer to fire at each ad refresh interval.
        private DispatcherTimer myAdRefreshTimer = new DispatcherTimer();

        // Global variables used for mediation decisions.
        private Random randomGenerator = new Random();
        private int errorCountCurrentRefresh = 0;  // Prevents infinite redirects.
        private int adDuplexWeight = 0;            // Will be set by GetAdDuplexWeight().

        // Microsoft and AdDuplex controls for banner ads.
        private AdControl myMicrosoftBanner = null;
        private AdDuplex.AdControl myAdDuplexBanner = null;

        private string MicrosoftAppId => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile") ? this.MobileApplicationId : this.DesktopApplicationId;

        private string MicrosoftAdUnit => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile") ? this.MobileAdUnit : this.DesktopAdUnit;

        private int AdWidth => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile") ? this.MobileAdWidth : this.DesktopAdWidth;

        private int AdHeight => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile") ? this.MobileAdHeight : this.DesktopAdHeight;

        public UwpAdMediator()
        {
            this.InitializeComponent();

            this.Loaded += UwpAdMediator_Loaded;
        }

        private void UwpAdMediator_Loaded(object sender, RoutedEventArgs e)
        {
            myAdGrid.Width = this.AdWidth;
            myAdGrid.Height = this.AdHeight;

            adDuplexWeight = GetAdDuplexWeight();
            RefreshBanner();

            // Start the timer to refresh the banner at the desired interval.
            myAdRefreshTimer.Interval = new TimeSpan(0, 0, this.AdRefreshSeconds);
            myAdRefreshTimer.Tick += MyAdRefreshTimer_Tick;
            myAdRefreshTimer.Start();
        }

        private int GetAdDuplexWeight()
        {
            return 0;

            //// TODO: Change this logic to fit your needs.
            //// This example uses Microsoft ads first in Canada and Mexico, then
            //// AdDuplex as fallback. In France, AdDuplex is first. In other regions,
            //// this example uses a weighted average approach, with 50% to AdDuplex.

            // int returnValue = 0;
            // switch (GlobalizationPreferences.HomeGeographicRegion)
            // {
            //    case "CA":
            //    case "MX":
            //        returnValue = 0;
            //        break;
            //    case "FR":
            //        returnValue = 100;
            //        break;
            //    default:
            //        returnValue = 50;
            //        break;
            // }
            // return returnValue;
        }

        private void ActivateMicrosoftBanner()
        {
            // Return if you hit the error limit for this refresh interval.
            if (errorCountCurrentRefresh >= this.MaxErrorsPerRefresh)
            {
                myAdGrid.Visibility = Visibility.Collapsed;
                return;
            }

            //// Use random number generator and house ads weight to determine whether
            //// to use paid ads or house ads. Paid is the default. You could alternatively
            //// write a method similar to GetAdDuplexWeight and override by region.
            // string myAdUnit = myMicrosoftPaidUnitId;
            // int houseWeight = HOUSE_AD_WEIGHT;
            // int randomInt = randomGenerator.Next(0, 100);
            // if (randomInt < houseWeight)
            // {
            //    myAdUnit = myMicrosoftHouseUnitId;
            // }

            // Hide the AdDuplex control if it is showing.
            if (myAdDuplexBanner != null)
            {
                myAdDuplexBanner.Visibility = Visibility.Collapsed;
            }

            // Initialize or display the Microsoft control.
            if (myMicrosoftBanner == null)
            {
                myMicrosoftBanner = new AdControl();
                myMicrosoftBanner.ApplicationId = this.MicrosoftAppId;
                myMicrosoftBanner.AdUnitId = this.MicrosoftAdUnit;
                myMicrosoftBanner.Width = this.AdWidth;
                myMicrosoftBanner.Height = this.AdHeight;
                myMicrosoftBanner.IsAutoRefreshEnabled = false;

                myMicrosoftBanner.AdRefreshed += MyMicrosoftBanner_AdRefreshed;
                myMicrosoftBanner.ErrorOccurred += MyMicrosoftBanner_ErrorOccurred;

                myAdGrid.Children.Add(myMicrosoftBanner);
            }
            else
            {
                myMicrosoftBanner.Visibility = Visibility.Visible;
                myMicrosoftBanner.Refresh();
            }
        }

        private void ActivateAdDuplexBanner()
        {
            // Return if you hit the error limit for this refresh interval.
            if (errorCountCurrentRefresh >= this.MaxErrorsPerRefresh)
            {
                myAdGrid.Visibility = Visibility.Collapsed;
                return;
            }

            // Hide the Microsoft control if it is showing.
            if (myMicrosoftBanner != null)
            {
                myMicrosoftBanner.Visibility = Visibility.Collapsed;
            }

            // Initialize or display the AdDuplex control.
            if (myAdDuplexBanner == null)
            {
                myAdDuplexBanner = new AdDuplex.AdControl();
                myAdDuplexBanner.AppKey = this.AdDuplexAppKey;
                myAdDuplexBanner.AdUnitId = this.AdDuplexAdUnit;
                myAdDuplexBanner.Width = this.AdWidth;
                myAdDuplexBanner.Height = this.AdHeight;
                myAdDuplexBanner.RefreshInterval = this.AdRefreshSeconds;

                myAdDuplexBanner.AdLoaded += MyAdDuplexBanner_AdLoaded;
                myAdDuplexBanner.AdCovered += MyAdDuplexBanner_AdCovered;
                myAdDuplexBanner.AdLoadingError += MyAdDuplexBanner_AdLoadingError;
                myAdDuplexBanner.NoAd += MyAdDuplexBanner_NoAd;

                myAdGrid.Children.Add(myAdDuplexBanner);
            }
            else
            {
                myAdDuplexBanner.Visibility = Visibility.Visible;
            }
        }

        private void MyAdRefreshTimer_Tick(object sender, object e)
        {
            RefreshBanner();
        }

        private void RefreshBanner()
        {
            // Reset the error counter for this refresh interval and
            // make sure the ad grid is visible.
            errorCountCurrentRefresh = 0;
            myAdGrid.Visibility = Visibility.Visible;

            // Display ad from AdDuplex.
            if (adDuplexWeight == 100)
            {
                ActivateAdDuplexBanner();
            }

            // Display Microsoft ad.
            else if (adDuplexWeight == 0)
            {
                ActivateMicrosoftBanner();
            }

            // Use weighted approach.
            else
            {
                int randomInt = randomGenerator.Next(0, 100);
                if (randomInt < adDuplexWeight)
                {
                    ActivateAdDuplexBanner();
                }
                else
                {
                    ActivateMicrosoftBanner();
                }
            }
        }

        private void MyMicrosoftBanner_AdRefreshed(object sender, RoutedEventArgs e)
        {
            // Add your code here as necessary.
        }

        private void MyMicrosoftBanner_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            errorCountCurrentRefresh++;
            ActivateAdDuplexBanner();
        }

        private void MyAdDuplexBanner_AdLoaded(object sender, AdDuplex.Banners.Models.BannerAdLoadedEventArgs e)
        {
            // Add your code here as necessary.
        }

        private void MyAdDuplexBanner_NoAd(object sender, AdDuplex.Common.Models.NoAdEventArgs e)
        {
            errorCountCurrentRefresh++;
            ActivateMicrosoftBanner();
        }

        private void MyAdDuplexBanner_AdLoadingError(object sender, AdDuplex.Common.Models.AdLoadingErrorEventArgs e)
        {
            errorCountCurrentRefresh++;
            ActivateMicrosoftBanner();
        }

        private void MyAdDuplexBanner_AdCovered(object sender, AdDuplex.Banners.Core.AdCoveredEventArgs e)
        {
            errorCountCurrentRefresh++;
            ActivateMicrosoftBanner();
        }
    }
}
