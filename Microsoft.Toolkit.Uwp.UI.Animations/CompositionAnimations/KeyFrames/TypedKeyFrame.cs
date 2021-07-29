// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides type implementation of <see cref="KeyFrame"/>
    /// </summary>
    /// <typeparam name="T">The type of property being animated</typeparam>
    public abstract class TypedKeyFrame<T> : KeyFrame
    {
        /// <summary>
        /// Gets or sets the value at the specific key
        /// </summary>
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(TypedKeyFrame<T>), new PropertyMetadata(default(T)));
    }
}
