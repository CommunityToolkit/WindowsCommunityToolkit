// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive
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
