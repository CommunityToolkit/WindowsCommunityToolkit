// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graph;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Get icon of DriveItem
    /// </summary>
    internal class DriveItemIconConverter : IValueConverter
    {
        private static readonly string OfficeIcon = "https://static2.sharepointonline.com/files/fabric/assets/brand-icons/document/png/{0}_32x1_5.png";
        private static readonly string LocalIcon = "ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Graph/Assets/{0}";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DriveItem driveItem = value as DriveItem;

            if (driveItem.Folder != null)
            {
                return string.Format(LocalIcon, "folder.svg");
            }
            else if (driveItem.File != null)
            {
                if (driveItem.File.MimeType.StartsWith("image"))
                {
                    return string.Format(LocalIcon, "photo.png");
                }
                else if (driveItem.File.MimeType.StartsWith("application/vnd.openxmlformats-officedocument"))
                {
                    int index = driveItem.Name.LastIndexOf('.');
                    if (index != -1)
                    {
                        string ext = driveItem.Name.Substring(index + 1).ToLower();
                        return string.Format(OfficeIcon, ext);
                    }
                }
            }
            else if (driveItem.Package != null)
            {
                switch (driveItem.Package.Type)
                {
                    case "oneNote":
                        return string.Format(OfficeIcon, "one");
                }
            }

            return string.Format(LocalIcon, "genericfile.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
