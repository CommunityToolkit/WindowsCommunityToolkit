// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides common Dependency properties for KeyFrames
    /// </summary>
    public abstract class KeyFrame : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Key"/> dependency property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(double), typeof(KeyFrame), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the key of the key frame
        /// Value should be between 0.0 and 1.0
        /// </summary>
        public double Key
        {
            get { return (double)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
    }
}
