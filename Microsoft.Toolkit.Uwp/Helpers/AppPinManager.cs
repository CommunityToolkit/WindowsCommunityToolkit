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
    /// Enumeration listing all Pin results
    /// </summary>
    public enum PinResult
    {
        /// <summary>
        ///  Unsupported Device
        /// </summary>
        UnsupportedDevice = 0,

        /// <summary>
        ///  Unsupported Windows 10 OS ( Pin support Version StartMenu >= 15063 ,TaskBar >= 16299)
        /// </summary>
        UnsupportedOs = 1,

        /// <summary>
        /// pin access is denied
        /// </summary>
        PinNotAllowed = 2,

        /// <summary>
        /// App has added startMenu or TaskBar
        /// </summary>
        PinPresent = 3,

        /// <summary>
        /// App has already is avaliable in StartMenu orTaskBar
        /// </summary>
        PinAlreadyPresent = 4
    }

    /// <summary>
    /// This class provides static helper methods help to add the app in startmenu or TaskBar.
    /// </summary>
    public static class AppPinManager
    {
        /// <summary>
        /// Pin the current app in Windows TaskBar
        /// </summary>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> CurrentAppToTaskBarAsync()
        {
            var pinResult = PinResult.UnsupportedOs;

            if (ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                if (TaskbarManager.GetDefault().IsSupported)
                {
                    if (TaskbarManager.GetDefault().IsPinningAllowed)
                    {
                        if (await TaskbarManager.GetDefault().IsCurrentAppPinnedAsync())
                        {
                            pinResult = PinResult.PinAlreadyPresent;
                        }
                        else
                        {
                            if (await TaskbarManager.GetDefault().RequestPinCurrentAppAsync())
                            {
                                pinResult = PinResult.PinPresent;
                            }
                            else
                            {
                                pinResult = PinResult.PinAlreadyPresent;
                            }
                        }
                    }
                    else
                    {
                        pinResult = PinResult.PinNotAllowed;
                    }
                }
                else
                {
                    pinResult = PinResult.UnsupportedDevice;
                }
            }

            return pinResult;
        }

        /// <summary>
        ///  Pin Specific App in Windows TaskBar
        /// </summary>
        /// <param name="appListEntry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> SpecificAppToTaskBarAsync(AppListEntry appListEntry)
        {
            var pinResult = PinResult.UnsupportedOs;

            if (ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                if (TaskbarManager.GetDefault().IsSupported)
                {
                    if (TaskbarManager.GetDefault().IsPinningAllowed)
                    {
                        if (await TaskbarManager.GetDefault().IsAppListEntryPinnedAsync(appListEntry))
                        {
                            pinResult = PinResult.PinAlreadyPresent;
                        }
                        else
                        {
                            if (await TaskbarManager.GetDefault().RequestPinAppListEntryAsync(appListEntry))
                            {
                                pinResult = PinResult.PinPresent;
                            }
                            else
                            {
                                pinResult = PinResult.PinAlreadyPresent;
                            }
                        }
                    }
                    else
                    {
                        pinResult = PinResult.PinNotAllowed;
                    }
                }
                else
                {
                    pinResult = PinResult.UnsupportedDevice;
                }
            }

            return pinResult;
        }

        /// <summary>
        /// Pin Specific App in Windows StartMenu
        /// </summary>
        /// <param name="entry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> SpecificAppToStartMenuAsync(AppListEntry entry)
        {
            var resultPinResult = PinResult.UnsupportedOs;
            if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager"))
            {
                if (StartScreenManager.GetDefault().SupportsAppListEntry(entry))
                {
                    if (await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry))
                    {
                        resultPinResult = PinResult.PinAlreadyPresent;
                    }
                    else
                    {
                        if (await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry))
                        {
                            resultPinResult = PinResult.PinPresent;
                        }
                        else
                        {
                            resultPinResult = PinResult.PinAlreadyPresent;
                        }
                    }
                }
                else
                {
                    resultPinResult = PinResult.UnsupportedDevice;
                }
            }

            return resultPinResult;
        }

        /// <summary>
        /// Pin Specific App in Windows StartMenu based on User
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="entry">AppListEntry</param>
        /// <returns>PinResult</returns>
        public static async Task<PinResult> UserSpecificAppToStartMenuAsync(User user, AppListEntry entry)
        {
            var resultPinResult = PinResult.UnsupportedOs;
            if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager"))
            {
                if (StartScreenManager.GetForUser(user).SupportsAppListEntry(entry))
                {
                    if (await StartScreenManager.GetForUser(user).ContainsAppListEntryAsync(entry))
                    {
                        resultPinResult = PinResult.PinAlreadyPresent;
                    }
                    else
                    {
                        if (await StartScreenManager.GetForUser(user).RequestAddAppListEntryAsync(entry))
                        {
                            resultPinResult = PinResult.PinPresent;
                        }
                        else
                        {
                            resultPinResult = PinResult.PinAlreadyPresent;
                        }
                    }
                }
                else
                {
                    resultPinResult = PinResult.UnsupportedDevice;
                }
            }

            return resultPinResult;
        }

    }
}
