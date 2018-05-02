using Microsoft.Graph;
using System;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public class DriveItemIconConverter : IValueConverter
    {
        private static readonly string s_officeIcon = "https://static2.sharepointonline.com/files/fabric/assets/brand-icons/document/png/{0}_32x1_5.png";
        private static readonly string s_localIcon = "ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Graph/Assets/{0}";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DriveItem driveItem = value as DriveItem;

            if (driveItem.Folder != null)
            {
                return string.Format(s_localIcon, "folder.svg");
            }
            else if (driveItem.File != null)
            {
                if (driveItem.File.MimeType.StartsWith("image"))
                {
                    return string.Format(s_localIcon, "photo.png");
                }
                else if (driveItem.File.MimeType.StartsWith("application/vnd.openxmlformats-officedocument"))
                {
                    int index = driveItem.Name.LastIndexOf('.');
                    if (index != -1)
                    {
                        string ext = driveItem.Name.Substring(index + 1);
                        return string.Format(s_officeIcon, ext);
                    }
                }
            }
            else if (driveItem.Package != null)
            {
                switch (driveItem.Package.Type)
                {
                    case "oneNote":
                        return string.Format(s_officeIcon, "one");
                }
            }

            return string.Format(s_localIcon, "genericfile.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
