using System;
using GoogleAnalytics;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class TrackingManager
    {
        private static readonly Tracker Tracker = AnalyticsManager.Current.CreateTracker("UA-91148254-1");

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
