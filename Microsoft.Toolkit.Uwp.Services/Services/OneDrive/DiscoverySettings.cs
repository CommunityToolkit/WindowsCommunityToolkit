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

using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  DiscoverySettings type
    /// </summary>
    public class DiscoverySettings
    {
        private static readonly string DISCOVERYSETTINGS = "DiscoverySettings";
        private static readonly string SERVICERESOURCEID = "ServiceResourceId";
        private static readonly string SERVICERESOURCEENDPOINT = "ServiceEndpointUri";

        /// <summary>
        /// Gets or sets the OneDrive For Business's ServiceEndpoint uri
        /// </summary>
        public string ServiceEndpointUri { get; set; }

        /// <summary>
        /// Gets or sets the OneDrive For Business's ServiceResourceId
        /// </summary>
        public string ServiceResourceId { get; set; }

        /// <summary>
        /// ApplicationData container for  DiscoverySettings
        /// </summary>
        private static ApplicationDataContainer _container = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoverySettings"/> class.
        /// </summary>
        public DiscoverySettings()
        {
            _container = ApplicationData.Current.LocalSettings.CreateContainer(DISCOVERYSETTINGS, ApplicationDataCreateDisposition.Always);
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <returns>DiscoverySettings</returns>
        internal static DiscoverySettings Load()
        {
            _container = ApplicationData.Current.LocalSettings.CreateContainer(DISCOVERYSETTINGS, ApplicationDataCreateDisposition.Always);
            if (_container.Values.ContainsKey(SERVICERESOURCEID) && _container.Values.ContainsKey(SERVICERESOURCEENDPOINT))
            {
                return new DiscoverySettings { ServiceEndpointUri = _container.Values[SERVICERESOURCEENDPOINT].ToString(), ServiceResourceId = _container.Values[SERVICERESOURCEID].ToString() };
            }

            return null;
        }

        /// <summary>
        /// Clear settings
        /// </summary>
        internal static void Clear()
        {
            _container.Values[SERVICERESOURCEENDPOINT] = null;
            _container.Values[SERVICERESOURCEID] = null;
        }

        /// <summary>
        /// Save settings
        /// </summary>
        internal void Save()
        {
            _container.Values[SERVICERESOURCEID] = ServiceResourceId;
            _container.Values[SERVICERESOURCEENDPOINT] = ServiceEndpointUri;
        }
    }
}
