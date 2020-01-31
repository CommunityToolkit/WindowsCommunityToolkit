// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#if NETFX_CORE
using Microsoft.Services.Store.Engagement;
#else
using Uno.Extensions;
using Uno.Logging;
#endif

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class TrackingManager
    {
#if NETFX_CORE
       private static StoreServicesCustomEventLogger logger;

        static TrackingManager()
        {
            try
            {
                logger = StoreServicesCustomEventLogger.GetDefault();
            }
            catch
            {
                // Ignoring error
            }
        }
#endif

        public static void TrackException(Exception ex)
        {
#if NETFX_CORE
            try
            {
                logger.Log($"exception - {ex.Message} - {ex.StackTrace}");
            }
            catch
            {
                // Ignore error
            }
#elif HAS_UNO
			typeof(TrackingManager).Log().Error("Unhandled exception", ex);
#endif
        }


		public static void TrackEvent(string category, string action, string label = "", long value = 0)
        {
#if NETFX_CORE
           try
            {
                logger.Log($"{category} - {action} - {label} - {value.ToString()}");
            }
            catch
            {
                // Ignore error
            }
#endif
        }

        public static void TrackPage(string pageName)
        {
#if NETFX_CORE
           try
            {
                logger.Log($"pageView - {pageName}");
            }
            catch
            {
                // Ignore error
            }
#endif
        }
    }
}
