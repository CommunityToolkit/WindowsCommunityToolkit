// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace CommunityToolkit.WinUI.UI.Behaviors
{
    /// <summary>
    /// A class for listening to an element enter or exit the ScrollViewer viewport
    /// </summary>
    public partial class ViewportBehavior
    {
        /// <summary>
        /// The IsFullyInViewport value of the associated element
        /// </summary>
        public static readonly DependencyProperty IsFullyInViewportProperty =
            DependencyProperty.Register(nameof(IsFullyInViewport), typeof(bool), typeof(ViewportBehavior), new PropertyMetadata(default(bool), OnIsFullyInViewportChanged));

        /// <summary>
        /// The IsInViewport value of the associated element
        /// </summary>
        public static readonly DependencyProperty IsInViewportProperty =
            DependencyProperty.Register(nameof(IsInViewport), typeof(bool), typeof(ViewportBehavior), new PropertyMetadata(default(bool), OnIsInViewportChanged));

        /// <summary>
        /// The IsAlwaysOn value of the associated element
        /// </summary>
        public static readonly DependencyProperty IsAlwaysOnProperty =
            DependencyProperty.Register(nameof(IsAlwaysOn), typeof(bool), typeof(ViewportBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a value indicating whether this behavior will remain attached after the associated element enters the viewport. When false, the behavior will remove itself after entering.
        /// </summary>
        public bool IsAlwaysOn
        {
            get { return (bool)GetValue(IsAlwaysOnProperty); }
            set { SetValue(IsAlwaysOnProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether associated element is fully in the ScrollViewer viewport
        /// </summary>
        public bool IsFullyInViewport
        {
            get { return (bool)GetValue(IsFullyInViewportProperty); }
            private set { SetValue(IsFullyInViewportProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether associated element is in the ScrollViewer viewport
        /// </summary>
        public bool IsInViewport
        {
            get { return (bool)GetValue(IsInViewportProperty); }
            private set { SetValue(IsInViewportProperty, value); }
        }

        /// <summary>
        /// Event tracking when the object is fully within the viewport or not
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">EventArgs</param>
        private static void OnIsFullyInViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ViewportBehavior)d;
            var value = (bool)e.NewValue;

            if (value)
            {
                obj.EnteredViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);

                if (!obj.IsAlwaysOn)
                {
                    Interaction.GetBehaviors(obj.AssociatedObject).Remove(obj);
                }
            }
            else
            {
                obj.ExitingViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event tracking the state of the object as it moves into and out of the viewport
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">EventArgs</param>
        private static void OnIsInViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ViewportBehavior)d;
            var value = (bool)e.NewValue;

            if (value)
            {
                obj.EnteringViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);
            }
            else
            {
                obj.ExitedViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);
            }
        }
    }
}
