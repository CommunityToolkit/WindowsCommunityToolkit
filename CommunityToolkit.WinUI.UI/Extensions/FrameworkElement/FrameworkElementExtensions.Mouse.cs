// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace CommunityToolkit.WinUI.UI
{
    /// <inheritdoc cref="FrameworkElementExtensions"/>
    public static partial class FrameworkElementExtensions
    {
        private static readonly object _cursorLock = new object();
        private static readonly InputCursor _defaultCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        private static readonly Dictionary<InputSystemCursorShape, InputCursor> _cursors =
            new Dictionary<InputSystemCursorShape, InputCursor> { { InputSystemCursorShape.Arrow, _defaultCursor } };

        /// <summary>
        /// Dependency property for specifying the target <see cref="InputSystemCursorShape"/> to be shown
        /// over the target <see cref="FrameworkElement"/>.
        /// </summary>
        public static readonly DependencyProperty CursorProperty =
            DependencyProperty.RegisterAttached("Cursor", typeof(InputSystemCursorShape), typeof(FrameworkElementExtensions), new PropertyMetadata(InputSystemCursorShape.Arrow, CursorChanged));

        /// <summary>
        /// Set the target <see cref="InputSystemCursorShape"/>.
        /// </summary>
        /// <param name="element">Object where the selector cursor type should be shown.</param>
        /// <param name="value">Target cursor type value.</param>
        public static void SetCursor(FrameworkElement element, InputSystemCursorShape value)
        {
            element.SetValue(CursorProperty, value);
        }

        /// <summary>
        /// Get the current <see cref="InputSystemCursorShape"/>.
        /// </summary>
        /// <param name="element">Object where the selector cursor type should be shown.</param>
        /// <returns>Cursor type set on target element.</returns>
        public static InputSystemCursorShape GetCursor(FrameworkElement element)
        {
            return (InputSystemCursorShape)element.GetValue(CursorProperty);
        }

        private static void CursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null)
            {
                throw new NullReferenceException(nameof(element));
            }

            var value = (InputSystemCursorShape)e.NewValue;

            // lock ensures InputCursor creation and event handlers attachment/detachment is atomic
            lock (_cursorLock)
            {
                if (!_cursors.ContainsKey(value))
                {
                    _cursors[value] = InputSystemCursor.Create(value);
                }

                // make sure event handlers are not attached twice to element
                element.PointerEntered -= Element_PointerEntered;
                element.PointerEntered += Element_PointerEntered;
                element.PointerExited -= Element_PointerExited;
                element.PointerExited += Element_PointerExited;
                element.Unloaded -= ElementOnUnloaded;
                element.Unloaded += ElementOnUnloaded;
            }
        }

        private static void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Window.Current == null)
            {
                return;
            }

            InputSystemCursorShape cursor = GetCursor((FrameworkElement)sender);

            // Window.Current.CoreWindow.PointerCursor = _cursors[cursor];
        }

        private static void Element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Window.Current == null)
            {
                return;
            }

            // when exiting change the cursor to the target Mouse.Cursor value of the new element
            InputCursor cursor;
            if (sender != e.OriginalSource && e.OriginalSource is FrameworkElement newElement)
            {
                cursor = _cursors[GetCursor(newElement)];
            }
            else
            {
                cursor = _defaultCursor;
            }

            // Window.Current.CoreWindow.PointerCursor = cursor;
        }

        private static void ElementOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Window.Current == null)
            {
                return;
            }

            // when the element is programatically unloaded, reset the cursor back to default
            // this is necessary when click triggers immediate change in layout and PointerExited is not called
            // Window.Current.CoreWindow.PointerCursor = _defaultCursor;
        }
    }
}