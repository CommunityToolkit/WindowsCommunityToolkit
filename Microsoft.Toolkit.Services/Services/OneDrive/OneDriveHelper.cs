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
