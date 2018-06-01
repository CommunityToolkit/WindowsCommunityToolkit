// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RoundImageEx control extends the default ImageBrush platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    [TemplatePart(Name = PartImageRectangle, Type = typeof(Rectangle))]
    [Obsolete("Use CornerRadius on ImageEx instead for 16299 and above.", false)]
    public partial class RoundImageEx : ImageExBase
    {
        /// <summary>
        /// The name of the root rectangle in the template
        /// </summary>
        protected const string PartImageRectangle = "ImageRectangle";

        /// <summary>
        /// Gets the root rectangle of the image
        /// </summary>
        protected Rectangle ImageRectangle { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundImageEx"/> class.
        /// </summary>
        public RoundImageEx()
            : base()
        {
            DefaultStyleKey = typeof(RoundImageEx);
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            ImageRectangle = GetTemplateChild(PartImageRectangle) as Rectangle;

            base.OnApplyTemplate();
        }
    }
}