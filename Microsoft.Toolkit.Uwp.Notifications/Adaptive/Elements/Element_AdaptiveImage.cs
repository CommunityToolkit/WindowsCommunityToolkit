// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    internal sealed class Element_AdaptiveImage : IElement_TileBindingChild, IElement_ToastBindingChild, IElement_AdaptiveSubgroupChild, IHaveXmlName, IHaveXmlNamedProperties
    {
        internal const AdaptiveImagePlacement DEFAULT_PLACEMENT = AdaptiveImagePlacement.Inline;
        internal const AdaptiveImageCrop DEFAULT_CROP = AdaptiveImageCrop.Default;
        internal const AdaptiveImageAlign DEFAULT_ALIGN = AdaptiveImageAlign.Default;

        [NotificationXmlAttribute("id")]
        public int? Id { get; set; }

        [NotificationXmlAttribute("src")]
        public string Src { get; set; }

        [NotificationXmlAttribute("alt")]
        public string Alt { get; set; }

        [NotificationXmlAttribute("addImageQuery")]
        public bool? AddImageQuery { get; set; }

        [NotificationXmlAttribute("placement", DEFAULT_PLACEMENT)]
        public AdaptiveImagePlacement Placement { get; set; } = DEFAULT_PLACEMENT;

        [NotificationXmlAttribute("hint-align", DEFAULT_ALIGN)]
        public AdaptiveImageAlign Align { get; set; } = DEFAULT_ALIGN;

        [NotificationXmlAttribute("hint-crop", DEFAULT_CROP)]
        public AdaptiveImageCrop Crop { get; set; } = DEFAULT_CROP;

        [NotificationXmlAttribute("hint-removeMargin")]
        public bool? RemoveMargin { get; set; }

        private int? _overlay;

        [NotificationXmlAttribute("hint-overlay")]
        public int? Overlay
        {
            get
            {
                return _overlay;
            }

            set
            {
                if (value != null)
                {
                    Element_TileBinding.CheckOverlayValue(value.Value);
                }

                _overlay = value;
            }
        }

        [NotificationXmlAttribute("spritesheet-src")]
        public string SpriteSheetSrc { get; set; }

        [NotificationXmlAttribute("spritesheet-height")]
        public uint? SpriteSheetHeight { get; set; }

        [NotificationXmlAttribute("spritesheet-fps")]
        public uint? SpriteSheetFps { get; set; }

        [NotificationXmlAttribute("spritesheet-startingFrame")]
        public uint? SpriteSheetStartingFrame { get; set; }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "image";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("id", Id);
            yield return new("src", Src);
            yield return new("alt", Alt);
            yield return new("addImageQuery", AddImageQuery);

            if (Placement != DEFAULT_PLACEMENT)
            {
                yield return new("placement", Placement.ToPascalCaseString());
            }

            if (Align != DEFAULT_ALIGN)
            {
                yield return new("hint-align", Align.ToPascalCaseString());
            }

            if (Crop != DEFAULT_CROP)
            {
                yield return new("hint-crop", Crop.ToPascalCaseString());
            }

            yield return new("hint-removeMargin", RemoveMargin);
            yield return new("hint-overlay", Overlay);
            yield return new("spritesheet-src", SpriteSheetSrc);
            yield return new("spritesheet-height", SpriteSheetHeight);
            yield return new("spritesheet-fps", SpriteSheetFps);
            yield return new("spritesheet-startingFrame", SpriteSheetStartingFrame);
        }
    }
}