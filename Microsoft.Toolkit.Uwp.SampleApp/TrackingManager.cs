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
using GoogleAnalytics;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class TrackingManager
    {
        private static readonly Tracker Tracker = AnalyticsManager.Current.CreateTracker(string.Empty);

        static TrackingManager()
        {
            try
            {
                AnalyticsManager.Current.ReportUncaughtExceptions = true;
                AnalyticsManager.Current.AutoAppLifetimeMonitoring = true;
            }
            catch
            {
                // Ignoring error
            }
        }

        public static void TrackException(Exception ex)
        {
            try
            {
                Tracker.Send(HitBuilder.CreateException("Exception: " + ex.Message + "->" + ex.StackTrace, false).Build());
            }
            catch
            {
                // Ignore error
            }
        }

        public static void TrackEvent(string category, string action, string label = "", long value = 0)
        {
            try
            {
                Tracker.Send(HitBuilder.CreateCustomEvent(category, action, label, value).Build());
            }
            catch
            {
                // Ignore error
            }
        }

        public static void TrackPage(string pageName)
        {
            try
            {
                Tracker.ScreenName = pageName;
                Tracker.Send(HitBuilder.CreateScreenView().Build());
            }
            catch
            {
                // Ignore error
            }
        }
    }
}
