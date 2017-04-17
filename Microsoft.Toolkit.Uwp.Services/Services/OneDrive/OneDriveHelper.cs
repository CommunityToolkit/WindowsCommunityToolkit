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

using System.Text;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// OneDriveHelper Type
    /// </summary>
    public static class OneDriveHelper
    {
        /// <summary>
        /// Transform the Windows Storage collision Option into OneDriveConflict Behavior
        /// </summary>
        /// <param name="collisionOption">Windows storage string collision option</param>
        /// <returns>The transformed option</returns>
        public static string TransformCollisionOptionToConflictBehavior(string collisionOption)
        {
            if (collisionOption.Equals("GenerateUniqueName"))
            {
                return "rename";
            }

            if (collisionOption.Equals("ReplaceExisting") || collisionOption.Equals("OpenIfExists"))
            {
                return "replace";
            }

            return "fail";
        }

        /// <summary>
        /// Transform enum into string array
        /// </summary>
        /// <param name="scopes">onedrive scopes</param>
        /// <returns>a string array containing the OneDrive permissions</returns>
        public static string[] TransformScopes(OneDriveScopes scopes)
        {
            StringBuilder sb = new StringBuilder();
            if ((scopes & OneDriveScopes.AppFolder) == OneDriveScopes.AppFolder)
            {
                sb.Append("onedrive.appfolder,");
            }

            if ((scopes & OneDriveScopes.OfflineAccess) == OneDriveScopes.OfflineAccess)
            {
                sb.Append("offline_access,");
            }

            if ((scopes & OneDriveScopes.ReadOnly) == OneDriveScopes.ReadOnly)
            {
                sb.Append("onedrive.readonly,");
            }

            if ((scopes & OneDriveScopes.ReadWrite) == OneDriveScopes.ReadWrite)
            {
                sb.Append("onedrive.readwrite");
            }

            if ((scopes & OneDriveScopes.WlSignin) == OneDriveScopes.WlSignin)
            {
                sb.Append("wl.signin");
            }

            return sb.ToString().Split(',');
        }

        /// <summary>
        /// Transform Orderby enum into the orderby string
        /// </summary>
        /// <param name="order">orderby enum</param>
        /// <returns>a string array containing the OneDrive Order by string</returns>
        public static string TransformOrderByToODataString(OrderBy order)
        {
            switch (order)
            {
                case OrderBy.Name:
                    return "name asc";
                case OrderBy.NameDesc:
                    return "name desc";
                case OrderBy.Size:
                    return "size asc";
                case OrderBy.SizeDesc:
                    return "size desc";
                case OrderBy.Date:
                    return "lastModifiedDateTime asc";
                case OrderBy.DateDesc:
                    return "lastModifiedDateTime desc";
            }

            return string.Empty;
        }
    }
}
