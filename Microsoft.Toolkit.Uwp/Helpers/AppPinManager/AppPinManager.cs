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
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Shell;
using Windows.UI.StartScreen;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods help to add the app in startmenu or TaskBar.
    /// </summary>
    public static class AppPinManager
    {
        /// <summary>
        /// Pin the current app in Windows TaskBar
        /// </summary>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> PinCurrentAppToTaskBarAsync()
        {
            var resultPinResult = PinResult.UnsupportedOs;

            if (!ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                return resultPinResult;
            }

            if (TaskbarManager.GetDefault().IsSupported)
            {
                if (TaskbarManager.GetDefault().IsPinningAllowed)
                {
                    if (await TaskbarManager.GetDefault().IsCurrentAppPinnedAsync())
                    {
                        resultPinResult = PinResult.PinAlreadyPresent;
                    }
                    else
                    {
                        var result = await TaskbarManager.GetDefault().RequestPinCurrentAppAsync();
                        resultPinResult = result ? PinResult.PinPresent : PinResult.PinOperationFailed;
                    }
                }
                else
                {
                    resultPinResult = PinResult.PinNotAllowed;
                }
            }
            else
            {
                resultPinResult = PinResult.UnsupportedDevice;
            }

            return resultPinResult;
        }

        /// <summary>
        ///  Pin Specific App in Windows TaskBar
        /// </summary>
        /// <param name="appListEntry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> PinSpecificAppToTaskBarAsync(AppListEntry appListEntry)
        {
            var resultPinResult = PinResult.UnsupportedOs;

            if (!ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                return resultPinResult;
            }

            if (TaskbarManager.GetDefault().IsSupported)
            {
                if (TaskbarManager.GetDefault().IsPinningAllowed)
                {
                    if (await TaskbarManager.GetDefault().IsAppListEntryPinnedAsync(appListEntry))
                    {
                        resultPinResult = PinResult.PinAlreadyPresent;
                    }
                    else
                    {
                        var result = await TaskbarManager.GetDefault().RequestPinAppListEntryAsync(appListEntry);
                        resultPinResult = result ? PinResult.PinPresent : PinResult.PinOperationFailed;
                    }
                }
                else
                {
                    resultPinResult = PinResult.PinNotAllowed;
                }
            }
            else
            {
                resultPinResult = PinResult.UnsupportedDevice;
            }

            return resultPinResult;
        }

        /// <summary>
        /// Pin Specific App in Windows StartMenu
        /// </summary>
        /// <param name="entry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> PinSpecificAppToStartMenuAsync(AppListEntry entry)
        {
            var resultPinResult = PinResult.UnsupportedOs;

            if (!ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager"))
            {
                return resultPinResult;
            }

            if (StartScreenManager.GetDefault().SupportsAppListEntry(entry))
            {
                if (await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry))
                {
                    resultPinResult = PinResult.PinAlreadyPresent;
                }
                else
                {
                    var result = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                    resultPinResult = result ? PinResult.PinPresent : PinResult.PinOperationFailed;
                }
            }
            else
            {
                resultPinResult = PinResult.UnsupportedDevice;
            }

            return resultPinResult;
        }

        /// <summary>
        /// Pin Specific App in Windows StartMenu based on User
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="entry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> PinUserSpecificAppToStartMenuAsync(User user, AppListEntry entry)
        {
            var resultPinResult = PinResult.UnsupportedOs;

            if (!ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager"))
            {
                return resultPinResult;
            }

            if (StartScreenManager.GetForUser(user).SupportsAppListEntry(entry))
            {
                if (await StartScreenManager.GetForUser(user).ContainsAppListEntryAsync(entry))
                {
                    resultPinResult = PinResult.PinAlreadyPresent;
                }
                else
                {
                    var result = await StartScreenManager.GetForUser(user).RequestAddAppListEntryAsync(entry);
                    resultPinResult = result ? PinResult.PinPresent : PinResult.PinOperationFailed;
                }
            }
            else
            {
                resultPinResult = PinResult.UnsupportedDevice;
            }

            return resultPinResult;
        }
    }
}