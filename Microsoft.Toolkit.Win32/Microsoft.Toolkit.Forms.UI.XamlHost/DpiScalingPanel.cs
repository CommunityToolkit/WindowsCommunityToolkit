// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    /// Panel that implements a scale factor for the XAML content using a render transform
    /// </summary>
    internal class DpiScalingPanel : Windows.UI.Xaml.Controls.Panel
    {
        public DpiScalingPanel()
        {
        }

        /// <summary>
        /// Measures wrapped UWP XAML content using passed in size availableSize
        /// </summary>
        /// <param name="availableSize">Available Size</param>
        /// <returns>XAML DesiredSize</returns>
        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            Windows.Foundation.Size desiredSize = new Windows.Foundation.Size(0, 0);

            Windows.UI.Xaml.UIElement element = Child;

            if (element != null)
            {
                element.Measure(new Windows.Foundation.Size(availableSize.Width / _scalingFactor, availableSize.Height / _scalingFactor));
                desiredSize.Width = element.DesiredSize.Width * _scalingFactor;
                desiredSize.Height = element.DesiredSize.Height * _scalingFactor;
            }

            desiredSize.Width = Math.Min(desiredSize.Width, availableSize.Width);
            desiredSize.Height = Math.Min(desiredSize.Height, availableSize.Height);

            return desiredSize;
        }

        /// <summary>
        /// Arranges wrapped UWP XAML content using passed in size constraint
        /// </summary>
        /// <param name="finalSize">Final Size</param>
        /// <returns>Size</returns>
        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            Windows.UI.Xaml.UIElement element = Child;

            if (element != null)
            {
                Windows.Foundation.Rect finalRect = new Windows.Foundation.Rect(0, 0, finalSize.Width / _scalingFactor, finalSize.Height / _scalingFactor);
                element.Arrange(finalRect);
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        ///    Gets or sets XAML content
        /// </summary>
        public Windows.UI.Xaml.UIElement Child
        {
            get
            {
                return Children.Count > 0 ? Children[0] : null;
            }

            set
            {
                Children.Clear();
                Children.Add(value);

                SetScalingFactor(_scalingFactor);
            }
        }

        /// <summary>
        ///    Sets the scaling factor of the panel
        /// <param name="newScalingFactor">New scaling factor</param>
        /// </summary>
        public void SetScalingFactor(double newScalingFactor)
        {
            // Do not touch any user set render transform on the XAML element
            // if scaling is not necessary and was not enabled before
            if (newScalingFactor == 1.0f && _scalingFactor == 1.0f)
            {
                return;
            }

            _scalingFactor = newScalingFactor;

            Windows.UI.Xaml.UIElement element = Child;

            if (element == null)
            {
                return;
            }

            if (newScalingFactor == 1.0f)
            {
                element.RenderTransform = null;
                return;
            }

            Windows.UI.Xaml.Media.ScaleTransform newScaleTransform = new Windows.UI.Xaml.Media.ScaleTransform();
            newScaleTransform.ScaleX = newScalingFactor;
            newScaleTransform.ScaleY = newScalingFactor;

            element.RenderTransform = newScaleTransform;
        }

        /// <summary>
        ///    The currently applied scaling factor
        /// </summary>
        private double _scalingFactor = 1.0f;
    }
}