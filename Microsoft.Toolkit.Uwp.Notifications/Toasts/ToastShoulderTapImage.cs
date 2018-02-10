﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Specifies the image to be displayed on a My People shoulder tap notification. New in Fall Creators Update.
    /// </summary>
    public sealed class ToastShoulderTapImage : IBaseImage
    {
        private string _source;

        /// <summary>
        /// Gets or sets the URI of the image (Required). This will be used if the sprite sheet isn't provided, or
        /// if the sprite sheet cannot be loaded. Can be from your application package, application data, or the internet.
        /// Internet images must obey the toast image size restrictions.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { BaseImageHelper.SetSource(ref _source, value); }
        }

        /// <summary>
        /// Gets or sets an optional sprite sheet that can be used instead of the image to display an animated sprite sheet.
        /// </summary>
        public ToastSpriteSheet SpriteSheet { get; set; }

        /// <summary>
        /// Gets or sets a description of the image, for users of assistive technologies.
        /// </summary>
        public string AlternateText { get; set; }

        /// <summary>
        /// Gets or sets a value whether Windows should append a query string to the image URI supplied in the <see cref="Source"/> property.
        /// Use this attribute if your server hosts images and can handle query strings, either by retrieving an image variant based on the
        /// query strings or by ignoring the query string and returning the image as specified without the query string.
        /// This query string specifies scale, contrast setting, and language.
        /// </summary>
        public bool? AddImageQuery { get; set; }

        internal Element_AdaptiveImage ConvertToElement()
        {
            Element_AdaptiveImage image = BaseImageHelper.CreateBaseElement(this);

            if (SpriteSheet != null)
            {
                SpriteSheet.PopulateImageElement(image);
            }

            return image;
        }
    }
}
