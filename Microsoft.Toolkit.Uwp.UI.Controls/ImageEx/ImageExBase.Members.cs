// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Shared Code for ImageEx and RoundImageEx
    /// </summary>
    public partial class ImageExBase
    {
        /// <summary>
        /// Identifies the <see cref="Stretch"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageExBase), new PropertyMetadata(Stretch.Uniform));

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ImageExBase), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Identifies the <see cref="DecodePixelHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecodePixelHeightProperty = DependencyProperty.Register(nameof(DecodePixelHeight), typeof(int), typeof(ImageExBase), new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="DecodePixelType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecodePixelTypeProperty = DependencyProperty.Register(nameof(DecodePixelType), typeof(int), typeof(ImageExBase), new PropertyMetadata(DecodePixelType.Physical));

        /// <summary>
        /// Identifies the <see cref="DecodePixelWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecodePixelWidthProperty = DependencyProperty.Register(nameof(DecodePixelWidth), typeof(int), typeof(ImageExBase), new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="IsCacheEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCacheEnabledProperty = DependencyProperty.Register(nameof(IsCacheEnabled), typeof(bool), typeof(ImageExBase), new PropertyMetadata(false));

        /// <summary>
        /// Returns a mask that represents the alpha channel of an image as a <see cref="CompositionBrush"/>
        /// </summary>
        /// <returns><see cref="CompositionBrush"/></returns>
        public abstract CompositionBrush GetAlphaMask();

        /// <summary>
        /// Event raised if the image failed loading.
        /// </summary>
        public event ImageExFailedEventHandler ImageExFailed;

        /// <summary>
        /// Event raised when the image is successfully loaded and opened.
        /// </summary>
        public event ImageExOpenedEventHandler ImageExOpened;

        /// <summary>
        /// Event raised when the control is initialized.
        /// </summary>
        public event EventHandler ImageExInitialized;

        /// <summary>
        /// Gets a value indicating whether control has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets or sets the CornerRadius for underlying image. <para/>
        /// Used to created rounded/circular images.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets DecodePixelHeight for underlying bitmap
        /// </summary>
        public int DecodePixelHeight
        {
            get { return (int)GetValue(DecodePixelHeightProperty); }
            set { SetValue(DecodePixelHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets DecodePixelType for underlying bitmap
        /// </summary>
        public DecodePixelType DecodePixelType
        {
            get { return (DecodePixelType)GetValue(DecodePixelTypeProperty); }
            set { SetValue(DecodePixelTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets DecodePixelWidth for underlying bitmap
        /// </summary>
        public int DecodePixelWidth
        {
            get { return (int)GetValue(DecodePixelWidthProperty); }
            set { SetValue(DecodePixelWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stretch behavior of the image
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets cache state
        /// </summary>
        public bool IsCacheEnabled
        {
            get { return (bool)GetValue(IsCacheEnabledProperty); }
            set { SetValue(IsCacheEnabledProperty, value); }
        }
    }
}