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
using Microsoft.Services.Store.Engagement;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class TrackingManager
    {
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

        public static void TrackException(Exception ex)
        {
            try
            {
                logger.Log($"exception - {ex.Message} - {ex.StackTrace}");
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
                logger.Log($"{category} - {action} - {label} - {value.ToString()}");
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
                logger.Log($"pageView - {pageName}");
            }
            catch
            {
                // Ignore error
            }
        }
    }
}
