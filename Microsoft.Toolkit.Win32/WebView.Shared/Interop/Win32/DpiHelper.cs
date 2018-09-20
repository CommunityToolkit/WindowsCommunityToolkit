// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop
{
    /// <summary>
    /// Class for scaling coordinates according to current DPI scaling set in Windows
    /// </summary>
    internal static class DpiHelper
    {
        internal const double LogicalDpi = 96d;

        // The primary screen's (device) current DPI
        private static double _deviceDpi = LogicalDpi;

        private static bool _enableHighDpi;
        private static bool _isInitialized;
        private static bool _enableDpiChangedMessageHandling;
        private static double _logicalToDeviceUnitsScalingFactor;

        internal static int DeviceDpi
        {
            get
            {
                Initialize();
                return (int)_deviceDpi;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to enable processing of WM_DPICHANGED and related messages
        /// </summary>
        internal static bool EnableDpiChangedMessageHandling
        {
            get
            {
                Initialize();
                if (_enableDpiChangedMessageHandling)
                {
                    // We can't cache this because different top level windows can have different DPI awareness contexts
                    var dpiAwareness = NativeMethods.GetThreadDpiAwarenessContext();
                    return NativeMethods.AreDpiAwarenessContextsEqual(
                        dpiAwareness,
                        NativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
                }

                return false;
            }
        }

        internal static bool IsPerMonitorDpiAware
        {
            get
            {
                Initialize();
                if (_enableHighDpi)
                {
                    var result = UnsafeNativeMethods.GetProcessDpiAwareness(
                        IntPtr.Zero, // Current process
                        out PROCESS_DPI_AWARENESS value);

                    if (result != 0)
                    {
                        // Some sort of error
                        return false;
                    }

                    return value == PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether scaling is required when converting between logical-device units, if the application opted in the automatic scaling
        /// </summary>
        /// <value><see langword="true" /> if scaling is required; otherwise, <see langword="false" />.</value>
        internal static bool IsScalingRequired
        {
            get
            {
                Initialize();
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
                return _deviceDpi != LogicalDpi;
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
            }
        }

        private static double LogicalToDeviceUnitsScalingFactor
        {
            get
            {
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
                if (_logicalToDeviceUnitsScalingFactor == 0d)
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
                {
                    Initialize();
                    _logicalToDeviceUnitsScalingFactor = _deviceDpi / LogicalDpi;
                }

                return _logicalToDeviceUnitsScalingFactor;
            }
        }

        public static int Scale(double value, double scalingFactor) => (int)Math.Round(scalingFactor * value);

        // Sets DPI awareness for the process. Returns true if DPI awareness is successfully set; otherwise, false.
        public static bool SetPerMonitorDpiAwareness()
        {
            // Only works if we're on RS2 or later and have ComCtl v6
            if (OSVersionHelper.IsWindows10CreatorsOrGreater)
            {
                const int rs2AndAboveDpiFlag = NativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2;
                return NativeMethods.SetProcessDpiAwarenessContext(rs2AndAboveDpiFlag);
            }

            return false;
        }

        internal static void Initialize()
        {
            if (!_isInitialized)
            {
                if (OSVersionHelper.IsWindows10AnniversaryOrGreater)
                {
                    // Primary screen DPI might be 96, but other screens may not be
                    _enableDpiChangedMessageHandling = true;
                    _enableHighDpi = true;
                }

                if (_enableHighDpi)
                {
                    _deviceDpi = GetSystemDpi();
                }

                _isInitialized = true;
            }
        }

        // Transforms a horizontal or vertical integer coordinate from logical to device units
        // by scaling it up  for current DPI and rounding to nearest integer value
        internal static int LogicalToDeviceUnits(double value, int devicePixels = 0)
        {
            if (devicePixels == 0)
            {
                return (int)Math.Round(LogicalToDeviceUnitsScalingFactor * value);
            }

            double scalingFactor = devicePixels / LogicalDpi;
            return Scale(value, scalingFactor);
        }

        // Returns the system DPI
        private static double GetSystemDpi()
        {
            var newDpiX = 0;
            var hDC = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
            if (hDC != IntPtr.Zero)
            {
                newDpiX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, hDC), NativeMethods.LOGPIXELSX);
                UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, hDC));
            }

            return newDpiX;
        }
    }
}