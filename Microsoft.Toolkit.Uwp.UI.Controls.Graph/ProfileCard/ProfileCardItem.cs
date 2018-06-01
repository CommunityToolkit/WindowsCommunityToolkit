// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class ProfileCardItem
    {
        public string NormalMail { get; set; }

        public string LargeProfileTitle { get; set; }

        public string LargeProfileMail { get; set; }

        public BitmapImage UserPhoto { get; set; }

        public ViewType DisplayMode { get; set; }

        public ProfileCardItem Clone()
        {
            return (ProfileCardItem)MemberwiseClone();
        }
    }
}
