// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_ToastImage : IElement_ToastBindingChild, IHaveXmlName, IHaveXmlNamedProperties
    {
        internal const ToastImagePlacement DEFAULT_PLACEMENT = ToastImagePlacement.Inline;
        internal const bool DEFAULT_ADD_IMAGE_QUERY = false;
        internal const ToastImageCrop DEFAULT_CROP = ToastImageCrop.None;

        public string Src { get; set; }

        public string Alt { get; set; }

        public bool AddImageQuery { get; set; } = DEFAULT_ADD_IMAGE_QUERY;

        public ToastImagePlacement Placement { get; set; } = DEFAULT_PLACEMENT;

        public ToastImageCrop Crop { get; set; } = DEFAULT_CROP;

        /// <inheritdoc/>
        string IHaveXmlName.Name => "image";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("src", Src);
            yield return new("alt", Alt);

            if (AddImageQuery != DEFAULT_ADD_IMAGE_QUERY)
            {
                yield return new("addImageQuery", AddImageQuery);
            }

            if (Placement != DEFAULT_PLACEMENT)
            {
                yield return new("placement", Placement.ToPascalCaseString());
            }

            if (Crop != DEFAULT_CROP)
            {
                yield return new("crop", Crop.ToPascalCaseString());
            }
        }
    }

    /// <summary>
    /// Specify the desired cropping of the image.
    /// </summary>
    public enum ToastImageCrop
    {
        /// <summary>
        /// Default value. Image is not cropped.
        /// </summary>
        None,

        /// <summary>
        /// Image is cropped to a circle shape.
        /// </summary>
        Circle
    }

    internal enum ToastImagePlacement
    {
        Inline,
        AppLogoOverride,
        Hero
    }
}