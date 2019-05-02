// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Services.Facebook;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class FacebookPhotoTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FacebookPhotoTemplate { get; set; }

        public DataTemplate FacebookAlbumTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item.GetType() == typeof(FacebookPhoto))
            {
                return FacebookPhotoTemplate;
            }

            if (item.GetType() == typeof(FacebookAlbum))
            {
                return FacebookAlbumTemplate;
            }

            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
