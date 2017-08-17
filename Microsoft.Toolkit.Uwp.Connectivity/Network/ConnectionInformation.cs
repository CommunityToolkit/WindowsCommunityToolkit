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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Microsoft.Toolkit.Uwp.Connectivity
{
    /// <summary>
    /// This class exposes information about the network connectivity.
    /// </summary>
    public class ConnectionInformation
    {
        private readonly List<string> networkNames = new List<string>();

        /// <summary>
        /// Updates  the current object based on profile passed.
        /// </summary>
        /// <param name="profile">instance of <see cref="ConnectionProfile"/></param>
        public virtual void UpdateConnectionInformation(ConnectionProfile profile)
        {
            networkNames.Clear();

            if (profile == null)
            {
                ConnectionType = ConnectionType.Unknown;
                ConnectivityLevel = NetworkConnectivityLevel.None;
                IsInternetAvailable = false;
                ConnectionCost = null;
                SignalStrength = null;

                return;
            }

            switch (profile.NetworkAdapter.IanaInterfaceType)
            {
                case 6:
                    ConnectionType = ConnectionType.Ethernet;
                    break;

                case 71:
                    ConnectionType = ConnectionType.WiFi;
                    break;

                case 243:
                case 244:
                    ConnectionType = ConnectionType.Data;
                    break;

                default:
                    ConnectionType = ConnectionType.Unknown;
                    break;
            }

            ConnectivityLevel = profile.GetNetworkConnectivityLevel();
            ConnectionCost = profile.GetConnectionCost();
            SignalStrength = profile.GetSignalBars();

            var names = profile.GetNetworkNames();
            if (names?.Count > 0)
            {
                networkNames.AddRange(names);
            }

            switch (ConnectivityLevel)
            {
                case NetworkConnectivityLevel.None:
                case NetworkConnectivityLevel.LocalAccess:
                    IsInternetAvailable = false;
                    break;

                default:
                    IsInternetAvailable = true;
                    break;
            }
        }

        /// <summary>
        /// Gets a value indicating whether if the current internet connection is metered.
        /// </summary>
        public bool IsInternetOnMeteredConnection
        {
            get
            {
                return ConnectionCost?.NetworkCostType != NetworkCostType.Unrestricted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether internet is available across all connections.
        /// </summary>
        /// <returns>True if internet can be reached.</returns>
        public bool IsInternetAvailable { get; private set; }

        /// <summary>
        /// Gets connection type for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="ConnectionType"/></returns>
        public ConnectionType ConnectionType { get; private set; }

        /// <summary>
        /// Gets connectivity level for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/></returns>
        public NetworkConnectivityLevel ConnectivityLevel { get; private set; }

        /// <summary>
        /// Gets connection cost for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/></returns>
        public ConnectionCost ConnectionCost { get; private set; }

        /// <summary>
        /// Gets signal strength for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/></returns>
        public byte? SignalStrength { get; private set; }

        /// <summary>
        /// Gets signal strength for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/></returns>
        public IReadOnlyList<string> NetworkNames
        {
            get
            {
                return networkNames.AsReadOnly();
            }
        }
    }
}
