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
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;

namespace Microsoft.Windows.Toolkit
{
    /// <summary>
    /// This class provides static helper methods.
    /// </summary>
    public static partial class Helpers
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
        /// Check internet availability across all connections.
        /// </summary>
        /// <returns>True if internet can be reached.</returns>
        public static bool IsInternetAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            return NetworkInformation.GetInternetConnectionProfile() != null;
        }
    }
}
