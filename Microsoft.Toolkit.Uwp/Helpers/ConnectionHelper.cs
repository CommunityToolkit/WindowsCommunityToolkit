﻿// ******************************************************************
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
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class provides static helper methods for connections.
    /// </summary>
    public static class ConnectionHelper
    {
        /// <summary>
        /// Gets a value indicating whether if the current internet connection is metered.
        /// </summary>
        public static bool IsInternetOnMeteredConnection
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();

                return profile?.GetConnectionCost().NetworkCostType != NetworkCostType.Unrestricted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether internet is available across all connections.
        /// </summary>
        /// <returns>True if internet can be reached.</returns>
        public static bool IsInternetAvailable
        {
            get
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    return false;
                }

                var profile = NetworkInformation.GetInternetConnectionProfile();

                NetworkConnectivityLevel level = profile.GetNetworkConnectivityLevel();

                switch (level)
                {
                    case NetworkConnectivityLevel.None:
                    case NetworkConnectivityLevel.LocalAccess:
                        return false;

                    default:
                        return true;
                }
            }
        }

        /// <summary>
        /// Gets connection type for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="ConnectionType"/></returns>
        public static ConnectionType ConnectionType
        {
            get
            {
                ConnectionProfile profile = null;
                ConnectionType connectionType = ConnectionType.Unknown;
                try
                {
                    profile = NetworkInformation.GetInternetConnectionProfile();
                }
                catch
                {
                }

                if (profile != null)
                {
                    switch (profile.NetworkAdapter.IanaInterfaceType)
                    {
                        case 6:
                            connectionType = ConnectionType.Ethernet;
                            break;

                        case 71:
                            connectionType = ConnectionType.WiFi;
                            break;

                        case 243:
                        case 244:
                            connectionType = ConnectionType.Data;
                            break;

                        default:
                            connectionType = ConnectionType.Unknown;
                            break;
                    }
                }

                return connectionType;
            }
        }

    /// <summary>
    /// Gets connection ssid for the current Wifi Connection.
    /// </summary>
    /// <returns> string value of current ssid/></returns>
    ///
    public static async Task<string> GetNetworkSsidAsync()
        {
            var pProfileFilter = new ConnectionProfileFilter();
            System.Collections.Generic.IReadOnlyList<ConnectionProfile> x = await NetworkInformation.FindConnectionProfilesAsync(pProfileFilter);

            try
            {
                WiFiAdapter firstAdapter;
                var access = await WiFiAdapter.RequestAccessAsync();
                if (access != WiFiAccessStatus.Allowed)
                {
                   throw new UnauthorizedAccessException("Acess Denied for wifi Control");
                }
                else
                {
                    var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                    if (result.Count >= 1)
                    {
                        firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    }
                    else
                    {
                        return "No WiFi Adapters detected on this machine";
                    }

                    var connectedProfile = await firstAdapter.NetworkAdapter.GetConnectedProfileAsync();
                    if (connectedProfile != null)
                    {
                        return connectedProfile.ProfileName;
                    }
                    else if (connectedProfile == null)
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }
    }
}
