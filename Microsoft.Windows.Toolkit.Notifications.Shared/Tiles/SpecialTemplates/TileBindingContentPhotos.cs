// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Animates through a slideshow of photos. Supported on all sizes.
    /// </summary>
    public sealed class TileBindingContentPhotos : ITileBindingContent
    {
        /// <summary>
        /// Animates through a slideshow of photos. Supported on all sizes.
        /// </summary>
        public TileBindingContentPhotos() { }

        /// <summary>
        /// Up to 12 images can be provided (Mobile will only display up to 9), which will be used for the slideshow. Adding more than 12 will throw an exception.
        /// </summary>
        public IList<TileBasicImage> Images { get; private set; } = new LimitedList<TileBasicImage>(12);

        internal TileTemplateNameV3 GetTemplateName(TileSize size)
        {
            return TileSizeToAdaptiveTemplateConverter.Convert(size);
        }

        internal void PopulateElement(Element_TileBinding binding, TileSize size)
        {
            binding.Presentation = TilePresentation.Photos;

            foreach (var img in Images)
                binding.Children.Add(img.ConvertToElement());
        }
    }
}