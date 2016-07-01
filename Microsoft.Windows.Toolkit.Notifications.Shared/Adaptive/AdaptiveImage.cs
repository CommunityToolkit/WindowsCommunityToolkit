// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using Microsoft.Windows.Toolkit.Notifications.Adaptive.Elements;

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// An inline image.
    /// </summary>
    public sealed class AdaptiveImage : IBaseImage, IAdaptiveChild, IAdaptiveSubgroupChild
    {
        /// <summary>
        /// Initializes a new inline image.
        /// </summary>
        public AdaptiveImage() { }

        /// <summary>
        /// Control the desired cropping of the image.
        /// </summary>
        public AdaptiveImageCrop HintCrop { get; set; }

        /// <summary>
        /// By default, images have an 8px margin around them. You can remove this margin by setting this property to true.
        /// </summary>
        public bool? HintRemoveMargin { get; set; }

        /// <summary>
        /// The horizontal alignment of the image.
        /// </summary>
        public AdaptiveImageAlign HintAlign { get; set; }

        private string _source;
        /// <summary>
        /// Required. The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { BaseImageHelper.SetSource(ref _source, value); }
        }

        /// <summary>
        /// A description of the image, for users of assistive technologies.
        /// </summary>
        public string AlternateText { get; set; }

        /// <summary>
        /// Set to true to allow Windows to append a query string to the image URI supplied in the Tile notification. Use this attribute if your server hosts images and can handle query strings, either by retrieving an image variant based on the query strings or by ignoring the query string and returning the image as specified without the query string. This query string specifies scale, contrast setting, and language.
        /// </summary>
        public bool? AddImageQuery { get; set;}

        internal Element_AdaptiveImage ConvertToElement()
        {
            Element_AdaptiveImage image = BaseImageHelper.CreateBaseElement(this);

            image.Crop = HintCrop;
            image.RemoveMargin = HintRemoveMargin;
            image.Align = HintAlign;
            image.Placement = AdaptiveImagePlacement.Inline;

            return image;
        }

        /// <summary>
        /// Returns the image's source string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Source == null)
                return "Source is null";

            return Source;
        }
    }
}