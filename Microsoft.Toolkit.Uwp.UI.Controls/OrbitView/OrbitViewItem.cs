﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// ContentControl used as the container for OrbitView items
    /// </summary>
    [TemplatePart(Name = _transformName, Type = typeof(CompositeTransform))]
    [TemplateVisualState(Name = VsNormal, GroupName = CommonStateGroup)]
    [TemplateVisualState(Name = VsPressed, GroupName = CommonStateGroup)]
    [TemplateVisualState(Name = VsPointerOver, GroupName = CommonStateGroup)]
    public class OrbitViewItem : ContentControl
    {
        private const string CommonStateGroup = "CommonStates";
        private const string VsNormal = "Normal";
        private const string VsPressed = "Pressed";
        private const string VsPointerOver = "PointerOver";

        private const string _transformName = "Transform";
        private CompositeTransform _transform;
        private bool _isClickEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitViewItem"/> class.
        /// Creates a new instance of <see cref="OrbitViewItem"/>
        /// </summary>
        public OrbitViewItem()
        {
            DefaultStyleKey = typeof(OrbitViewItem);
        }

        /// <summary>
        /// Gets or sets a value indicating whether item is invokable.
        /// </summary>
        internal bool IsClickEnabled
        {
            get
            {
                return _isClickEnabled;
            }

            set
            {
                if (value != _isClickEnabled)
                {
                    _isClickEnabled = value;
                    if (value)
                    {
                        EnableItemInteraction();
                    }
                    else
                    {
                        DisableItemInteraction();
                    }
                }
            }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes call ApplyTemplate
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SizeChanged -= OrbitViewItemControl_SizeChanged;

            if (IsClickEnabled)
            {
                EnableItemInteraction();
            }

            _transform = GetTemplateChild(_transformName) as CompositeTransform;
            if (_transform != null)
            {
                SizeChanged += OrbitViewItemControl_SizeChanged;
            }
        }

        private void OrbitViewItemControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_transform != null)
            {
                _transform.CenterX = e.NewSize.Width / 2;
                _transform.CenterY = e.NewSize.Height / 2;
            }
        }

        private void EnableItemInteraction()
        {
            DisableItemInteraction();

            IsTabStop = true;
            UseSystemFocusVisuals = true;
            PointerEntered += Control_PointerEntered;
            PointerExited += Control_PointerExited;
            PointerPressed += Control_PointerPressed;
            PointerReleased += Control_PointerReleased;
            KeyDown += Control_KeyDown;
            KeyUp += Control_KeyUp;
        }

        private void DisableItemInteraction()
        {
            IsTabStop = false;
            UseSystemFocusVisuals = false;
            PointerEntered -= Control_PointerEntered;
            PointerExited -= Control_PointerExited;
            PointerPressed -= Control_PointerPressed;
            PointerReleased -= Control_PointerReleased;
            KeyDown -= Control_KeyDown;
            KeyUp -= Control_KeyUp;
        }

        private void Control_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                VisualStateManager.GoToState(this, VsNormal, true);
            }
        }

        private void Control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                VisualStateManager.GoToState(this, VsPressed, true);
            }
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VsNormal, true);
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VsPressed, true);
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VsNormal, true);
        }

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VsPointerOver, true);
        }
    }
}
