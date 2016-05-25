using System.Linq;
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

        /// <summary>
        /// Check if current internet connection is metered.
        /// </summary>
        public static bool IsInternetOnMeteredConnection
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();

                return profile?.GetConnectionCost().NetworkCostType != NetworkCostType.Unrestricted;
            }
        }
    }
}
