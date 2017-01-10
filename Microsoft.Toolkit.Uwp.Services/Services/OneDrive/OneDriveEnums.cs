using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// OneDriveService enums
    /// </summary>
    public class OneDriveEnums
    {
        /// <summary>
        /// Specifies which account to use.
        /// </summary>
        public enum AccountProviderType
        {
            /// <summary>
            /// Uses an Azure Active Directory account
            /// </summary>
            Adal,

            /// <summary>
            /// Uses an Microsoft Account
            /// </summary>
            Msa,

            /// <summary>
            /// Uses Windows OnlineId
            /// </summary>
            OnlineId
        }
    }
}
