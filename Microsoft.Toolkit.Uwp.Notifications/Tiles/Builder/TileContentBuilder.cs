// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    public partial class TileContentBuilder
    {
        public const TileSize AllSize = TileSize.Small | TileSize.Medium | TileSize.Large | TileSize.Wide;

        public TileContent Content
        {
            get; private set;
        }

        private TileVisual Visual
        {
            get
            {
                if (Content.Visual == null)
                {
                    Content.Visual = new TileVisual();
                }

                return Content.Visual;
            }
        }

        private TileBinding SmallTile
        {
            get
            {
                return Visual.TileSmall;
            }

            set
            {
                Visual.TileSmall = value;
            }
        }

        private TileBinding MediumTile
        {
            get
            {
                return Visual.TileMedium;
            }

            set
            {
                Visual.TileMedium = value;
            }
        }

        private TileBinding WideTile
        {
            get
            {
                return Visual.TileWide;
            }

            set
            {
                Visual.TileWide = value;
            }
        }

        private TileBinding LargeTile
        {
            get
            {
                return Visual.TileLarge;
            }

            set
            {
                Visual.TileLarge = value;
            }
        }

        public TileContentBuilder()
        {
            Content = new TileContent();
        }

        public TileContentBuilder AddTile(TileSize size, ITileBindingContent tileContent = null)
        {
            if (size.HasFlag(TileSize.Small))
            {
                SmallTile = new TileBinding();
                SmallTile.Content = tileContent ?? new TileBindingContentAdaptive();
            }

            if (size.HasFlag(TileSize.Medium))
            {
                MediumTile = new TileBinding();
                MediumTile.Content = tileContent ?? new TileBindingContentAdaptive();
            }

            if (size.HasFlag(TileSize.Wide))
            {
                WideTile = new TileBinding();
                WideTile.Content = tileContent ?? new TileBindingContentAdaptive();
            }

            if (size.HasFlag(TileSize.Large))
            {
                LargeTile = new TileBinding();
                LargeTile.Content = tileContent ?? new TileBindingContentAdaptive();
            }

            return this;
        }

        public TileContentBuilder SetBranding(TileBranding branding, TileSize size = AllSize)
        {
            if (size == AllSize)
            {
                // Set on visual.
                Visual.Branding = branding;
            }
            else
            {
                if (size.HasFlag(TileSize.Small) && SmallTile != null)
                {
                    SmallTile.Branding = branding;
                }

                if (size.HasFlag(TileSize.Medium) && MediumTile != null)
                {
                    MediumTile.Branding = branding;
                }

                if (size.HasFlag(TileSize.Wide) && WideTile != null)
                {
                    WideTile.Branding = branding;
                }

                if (size.HasFlag(TileSize.Large) && LargeTile != null)
                {
                    LargeTile.Branding = branding;
                }
            }

            return this;
        }

        public TileContentBuilder SetDisplayName(string displayName, TileSize size = AllSize)
        {
            if (size == AllSize)
            {
                // Set on visual.
                Visual.DisplayName = displayName;
            }
            else
            {
                if (size.HasFlag(TileSize.Small) && SmallTile != null)
                {
                    SmallTile.DisplayName = displayName;
                }

                if (size.HasFlag(TileSize.Medium) && MediumTile != null)
                {
                    MediumTile.DisplayName = displayName;
                }

                if (size.HasFlag(TileSize.Wide) && WideTile != null)
                {
                    WideTile.DisplayName = displayName;
                }

                if (size.HasFlag(TileSize.Large) && LargeTile != null)
                {
                    LargeTile.DisplayName = displayName;
                }
            }

            return this;
        }

        public TileContentBuilder SetBackgroundImage(Uri imageUri, TileSize size = AllSize, string alternateText = default(string), bool? addImageQuery = default(bool?), int? hintOverlay = default(int?), TileBackgroundImageCrop hintCrop = TileBackgroundImageCrop.Default)
        {
            TileBackgroundImage backgroundImage = new TileBackgroundImage();
            backgroundImage.Source = imageUri.OriginalString;
            backgroundImage.HintCrop = hintCrop;

            if (alternateText != default(string))
            {
                backgroundImage.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                backgroundImage.AddImageQuery = addImageQuery;
            }

            if (hintOverlay != default(int?))
            {
                backgroundImage.HintOverlay = hintOverlay;
            }

            return SetBackgroundImage(backgroundImage, size);
        }

        public TileContentBuilder SetBackgroundImage(TileBackgroundImage backgroundImage, TileSize size = AllSize)
        {
            // Set to any available tile at the moment of calling.

            if (size.HasFlag(TileSize.Small) && SmallTile != null)
            {
                GetAdaptiveTileContent(SmallTile).BackgroundImage = backgroundImage;
            }

            if (size.HasFlag(TileSize.Medium) && MediumTile != null)
            {
                GetAdaptiveTileContent(MediumTile).BackgroundImage = backgroundImage;
            }

            if (size.HasFlag(TileSize.Wide) && WideTile != null)
            {
                GetAdaptiveTileContent(WideTile).BackgroundImage = backgroundImage;
            }

            if (size.HasFlag(TileSize.Large) && LargeTile != null)
            {
                GetAdaptiveTileContent(LargeTile).BackgroundImage = backgroundImage;
            }

            return this;
        }

        public TileContentBuilder SetPeekImage(Uri imageUri, TileSize size = AllSize, string alternateText = default(string), bool? addImageQuery = default(bool?), int? hintOverlay = default(int?), TilePeekImageCrop hintCrop = TilePeekImageCrop.Default)
        {
            TilePeekImage peekImage = new TilePeekImage();
            peekImage.Source = imageUri.OriginalString;
            peekImage.HintCrop = hintCrop;

            if (alternateText != default(string))
            {
                peekImage.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                peekImage.AddImageQuery = addImageQuery;
            }

            if (hintOverlay != default(int?))
            {
                peekImage.HintOverlay = hintOverlay;
            }

            return SetPeekImage(peekImage, size);
        }

        public TileContentBuilder SetPeekImage(TilePeekImage peekImage, TileSize size = AllSize)
        {
            // Set to any available tile at the moment of calling.

            if (size.HasFlag(TileSize.Small) && SmallTile != null)
            {
                GetAdaptiveTileContent(SmallTile).PeekImage = peekImage;
            }

            if (size.HasFlag(TileSize.Medium) && MediumTile != null)
            {
                GetAdaptiveTileContent(MediumTile).PeekImage = peekImage;
            }

            if (size.HasFlag(TileSize.Wide) && WideTile != null)
            {
                GetAdaptiveTileContent(WideTile).PeekImage = peekImage;
            }

            if (size.HasFlag(TileSize.Large) && LargeTile != null)
            {
                GetAdaptiveTileContent(LargeTile).PeekImage = peekImage;
            }

            return this;
        }

        public TileContentBuilder SetActivationArgument(string args, TileSize size = AllSize)
        {
            if (size == AllSize)
            {
                Visual.Arguments = args;
            }
            else
            {
                if (size.HasFlag(TileSize.Small) && SmallTile != null)
                {
                    SmallTile.Arguments = args;
                }

                if (size.HasFlag(TileSize.Medium) && MediumTile != null)
                {
                    MediumTile.Arguments = args;
                }

                if (size.HasFlag(TileSize.Wide) && WideTile != null)
                {
                    WideTile.Arguments = args;
                }

                if (size.HasFlag(TileSize.Large) && LargeTile != null)
                {
                    LargeTile.Arguments = args;
                }
            }

            return this;
        }

        public TileContentBuilder AddText(string text, TileSize size = AllSize, AdaptiveTextStyle? hintStyle = null, bool? hintWrap = default(bool?), int? hintMaxLines = default(int?), int? hintMinLines = default(int?), AdaptiveTextAlign? hintAlign = null, string language = default(string))
        {
            // Create the adaptive text.
            AdaptiveText adaptive = new AdaptiveText()
            {
                Text = text
            };

            if (hintStyle != null)
            {
                adaptive.HintStyle = hintStyle.Value;
            }

            if (hintAlign != null)
            {
                adaptive.HintAlign = hintAlign.Value;
            }

            if (hintWrap != default(bool?))
            {
                adaptive.HintWrap = hintWrap;
            }

            if (hintMaxLines != default(int?))
            {
                adaptive.HintMaxLines = hintMaxLines;
            }

            if (hintMinLines != default(int?) && hintMinLines > 0)
            {
                adaptive.HintMinLines = hintMinLines;
            }

            if (language != default(string))
            {
                adaptive.Language = language;
            }

            // Add to the tile content
            return AddAdaptiveTileVisualChild(adaptive, size);
        }

        public TileContentBuilder AddAdaptiveTileVisualChild(ITileBindingContentAdaptiveChild child, TileSize size = AllSize)
        {
            if (size.HasFlag(TileSize.Small) && SmallTile != null && GetAdaptiveTileContent(SmallTile) != null)
            {
                GetAdaptiveTileContent(SmallTile).Children.Add(child);
            }

            if (size.HasFlag(TileSize.Medium) && MediumTile != null && GetAdaptiveTileContent(MediumTile) != null)
            {
                GetAdaptiveTileContent(MediumTile).Children.Add(child);
            }

            if (size.HasFlag(TileSize.Wide) && WideTile != null && GetAdaptiveTileContent(MediumTile) != null)
            {
                GetAdaptiveTileContent(WideTile).Children.Add(child);
            }

            if (size.HasFlag(TileSize.Large) && LargeTile != null && GetAdaptiveTileContent(LargeTile) != null)
            {
                GetAdaptiveTileContent(LargeTile).Children.Add(child);
            }

            return this;
        }

        public TileContent GetTileContent()
        {
            return Content;
        }

        private TileBindingContentAdaptive GetAdaptiveTileContent(TileBinding binding)
        {
            return binding.Content as TileBindingContentAdaptive;
        }
    }

#endif
}
