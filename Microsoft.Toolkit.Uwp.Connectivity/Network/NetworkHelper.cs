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
using Windows.Networking.Connectivity;

namespace Microsoft.Toolkit.Uwp.Connectivity
{
    /// <summary>
    /// This class exposes functionality of NetworkInformation through a singleton.
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static NetworkHelper _instance;

        /// <summary>
        /// Event raised when the network changes.
        /// </summary>
        public event EventHandler NetworkChanged;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static NetworkHelper Instance => _instance ?? (_instance = new NetworkHelper());

        /// <summary>
        /// Gets instance of <see cref="ConnectionInformation"/>.
        /// </summary>
        public ConnectionInformation ConnectionInformation { get; } = new ConnectionInformation();

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkHelper"/> class.
        /// </summary>
        protected NetworkHelper()
        {
            ConnectionInformation = new ConnectionInformation();

            OnNetworkStatusChanged(null);

            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="NetworkHelper"/> class.
        /// </summary>
        ~NetworkHelper()
        {
            NetworkInformation.NetworkStatusChanged -= OnNetworkStatusChanged;
        }

        private void OnNetworkStatusChanged(object sender)
        {
            ConnectionProfile profile = null;
            try
            {
                profile = NetworkInformation.GetInternetConnectionProfile();
            }
            catch
            {
            }

            ConnectionInformation.UpdateConnectionInformation(profile);

            NetworkChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
