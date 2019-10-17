// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Devices.Input;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Triggers
{
    /// <summary>
    /// Enables a state based on input type used
    /// </summary>
    public class InputTypeTrigger : StateTriggerBase
    {
        /// <summary>
        /// Gets or sets the type of the pointer used.
        /// </summary>
        /// <value>The type of the pointer.</value>
        public PointerDeviceType PointerType
        {
            get { return (PointerDeviceType)GetValue(PointerTypeProperty); }
            set { SetValue(PointerTypeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PointerType"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty PointerTypeProperty =
            DependencyProperty.Register(nameof(PointerType), typeof(PointerDeviceType), typeof(InputTypeTrigger), new PropertyMetadata(PointerDeviceType.Pen));

        /// <summary>
        /// Gets or sets the target element.
        /// </summary>
        public FrameworkElement TargetElement
        {
            get { return GetValue(TargetElementProperty) as FrameworkElement; }
            set { SetValue(TargetElementProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TargetElement"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(nameof(TargetElement), typeof(string), typeof(InputTypeTrigger), new PropertyMetadata(string.Empty, OnTargetElementPropertyChanged));

        private static void OnTargetElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (InputTypeTrigger)d;
            var valOld = e.OldValue as FrameworkElement;
            if (valOld != null)
            {
                valOld.PointerPressed -= obj.TargetElement_PointerPressed;
            }

            var val = e.NewValue as FrameworkElement;
            if (val != null)
            {
                val.PointerPressed += obj.TargetElement_PointerPressed;
            }
        }

        private void TargetElement_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SetActive(e.Pointer.PointerDeviceType == PointerType);
        }
    }
}
