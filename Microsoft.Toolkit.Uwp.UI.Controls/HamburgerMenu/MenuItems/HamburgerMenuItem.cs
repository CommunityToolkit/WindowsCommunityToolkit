// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenuItem provides an abstract implementation for HamburgerMenu entries.
    /// </summary>
    [Obsolete("The HamburgerMenuItem will be removed alongside the HamburgerMenu in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public abstract class HamburgerMenuItem : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TargetPageType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register(nameof(TargetPageType), typeof(Type), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
         /// Identifies the <see cref="Tag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(nameof(Tag), typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that specifies label to display.
        /// </summary>
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }

            set
            {
                SetValue(LabelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies the page to navigate to (if you use the HamburgerMenu with a Frame content)
        /// </summary>
        public Type TargetPageType
        {
            get
            {
                return (Type)GetValue(TargetPageTypeProperty);
            }

            set
            {
                SetValue(TargetPageTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies an user specific value.
        /// </summary>
        public object Tag
        {
            get
            {
                return GetValue(TagProperty);
            }

            set
            {
                SetValue(TagProperty, value);
            }
        }
    }
}
