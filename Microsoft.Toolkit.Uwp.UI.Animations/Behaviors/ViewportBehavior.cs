// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// A class for listening element enter or exit the viewport
    /// </summary>
    public class ViewportBehavior : BehaviorBase<FrameworkElement>
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
        /// Associated element fully enter the viewport event
        /// </summary>
        public event EventHandler EnteredViewport;

        /// <summary>
        /// Associated element fully exit the viewport event
        /// </summary>
        public event EventHandler ExitedViewport;

        /// <summary>
        /// Associated element enter the viewport event
        /// </summary>
        public event EventHandler EnteringViewport;

        /// <summary>
        /// Associated element exit the viewport event
        /// </summary>
        public event EventHandler ExitingViewport;

        /// <summary>
        /// Gets a value indicating whether associated element is fully in the viewport
        /// </summary>
        public bool IsFullyInViewport
        {
            get { return (bool)GetValue(IsFullyInViewportProperty); }
            private set { SetValue(IsFullyInViewportProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether associated element is in the viewport
        /// </summary>
        public bool IsInViewport
        {
            get { return (bool)GetValue(IsInViewportProperty); }
            private set { SetValue(IsInViewportProperty, value); }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
        }

        private static void OnIsFullyInViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ViewportBehavior)d;
            var value = (bool)e.NewValue;
            if (value)
            {
                obj.EnteredViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);
            }
            else
            {
                obj.ExitingViewport?.Invoke(obj.AssociatedObject, EventArgs.Empty);
            }
        }

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

        private void AssociatedObject_LayoutUpdated(object sender, object e)
        {
            FrameworkElement hostElement = AssociatedObject;
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(hostElement) as FrameworkElement;
                if (parent == null)
                {
                    break;
                }

                if (parent is ScrollViewer)
                {
                    hostElement = parent;
                    break;
                }

                hostElement = parent;
            }

            if (hostElement != null)
            {
                var associatedControlRect = AssociatedObject.TransformToVisual(hostElement)
                    .TransformBounds(new Rect(0, 0, AssociatedObject.ActualWidth, AssociatedObject.ActualHeight));
                var rootContentRect = new Rect(0, 0, hostElement.ActualWidth, hostElement.ActualHeight);

                if (rootContentRect.Contains(new Point(associatedControlRect.Left, associatedControlRect.Top)) ||
                    rootContentRect.Contains(new Point(associatedControlRect.Right, associatedControlRect.Top)) ||
                    rootContentRect.Contains(new Point(associatedControlRect.Right, associatedControlRect.Bottom)) ||
                    rootContentRect.Contains(new Point(associatedControlRect.Left, associatedControlRect.Bottom)))
                {
                    IsInViewport = true;

                    if (rootContentRect.Contains(new Point(associatedControlRect.Left, associatedControlRect.Top)) &&
                        rootContentRect.Contains(new Point(associatedControlRect.Right, associatedControlRect.Top)) &&
                        rootContentRect.Contains(new Point(associatedControlRect.Right, associatedControlRect.Bottom)) &&
                        rootContentRect.Contains(new Point(associatedControlRect.Left, associatedControlRect.Bottom)))
                    {
                        IsFullyInViewport = true;
                    }
                    else
                    {
                        IsFullyInViewport = false;
                    }
                }
                else
                {
                    IsInViewport = false;
                }
            }
        }
    }
}