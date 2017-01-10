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

        /// <summary>
        /// Specifies in wich order to sort the file or folder
        /// </summary>
        public enum OrderBy
        {
            /// <summary>
            /// Do nothing
            /// </summary>
            None,

            /// <summary>
            /// Use the name to order the items
            /// </summary>
            Name,

            /// <summary>
            /// Use the size to order the items
            /// </summary>
            Size,
        }
    }
}
