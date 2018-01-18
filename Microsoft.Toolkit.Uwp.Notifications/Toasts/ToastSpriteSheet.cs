using Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Specifies a sprite sheet. New in Fall Creators Update.
    /// </summary>
    public sealed class ToastSpriteSheet
    {
        private string _source;

        /// <summary>
        /// Gets or sets the URI of the sprite sheet (Required).
        /// Can be from your application package, application data, or the internet.
        /// Internet sources must obey the toast image size restrictions.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { BaseImageHelper.SetSource(ref _source, value); }
        }

        /// <summary>
        /// Gets or sets the frame-height of the sprite sheet. Required value that must be greater than 0.
        /// </summary>
        public uint? FrameHeight { get; set; }

        /// <summary>
        /// Gets or sets the frames per second at which to animate the sprite sheet. Required value that must be greater than 0 but no larger than 120.
        /// </summary>
        public uint? Fps { get; set; }

        /// <summary>
        /// Gets or sets the starting frame of the sprite sheet. If not specified, it will start at frame zero.
        /// </summary>
        public uint? StartingFrame { get; set; }

        internal void PopulateImageElement(Element_AdaptiveImage image)
        {
            if (Source == null)
            {
                throw new NullReferenceException("Source cannot be null on ToastSpriteSheet");
            }

            image.SpriteSheetSrc = Source;
            image.SpriteSheetHeight = FrameHeight;
            image.SpriteSheetFps = Fps;
            image.SpriteSheetStartingFrame = StartingFrame;
        }
    }
}
