// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations.Helpers;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed class CustomTextScalingCalculator : IScalingCalculator
    {
        /// <inheritdoc/>
        public Point GetScaling(UIElement source, UIElement target)
        {
            var sourceTextElement = source?.FindDescendantOrSelf<TextBlock>();
            var targetTextElement = target?.FindDescendantOrSelf<TextBlock>();
            if (sourceTextElement is not null && targetTextElement is not null)
            {
                var scale = targetTextElement.FontSize / sourceTextElement.FontSize;
                return new Point(scale, scale);
            }

            return new Point(1, 1);
        }
    }
}