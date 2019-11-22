// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="EyedropperToolButton"/> control helps use <see cref="Eyedropper"/> in view.
    /// </summary>
    public partial class EyedropperToolButton : ButtonBase
    {
        private const string NormalState = "Normal";
        private const string PointerOverState = "PointerOver";
        private const string PressedState = "Pressed";
        private const string DisabledState = "Disabled";
        private const string EyedropperEnabledState = "EyedropperEnabled";
        private const string EyedropperEnabledPointerOverState = "EyedropperEnabledPointerOver";
        private const string EyedropperEnabledPressedState = "EyedropperEnabledPressed";
        private const string EyedropperEnabledDisabledState = "EyedropperEnabledDisabled";

        private readonly Eyedropper _eyedropper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EyedropperToolButton"/> class.
        /// </summary>
        public EyedropperToolButton()
        {
            DefaultStyleKey = typeof(EyedropperToolButton);
            RegisterPropertyChangedCallback(IsEnabledProperty, OnIsEnabledChanged);
            _eyedropper = new Eyedropper();
            this.Loaded += EyedropperToolButton_Loaded;
        }

        /// <summary>
        /// Occurs when the Color property has changed.
        /// </summary>
        public event TypedEventHandler<EyedropperToolButton, EyedropperColorChangedEventArgs> ColorChanged;

        /// <summary>
        /// Occurs when the eyedropper begins to take color.
        /// </summary>
        public event TypedEventHandler<EyedropperToolButton, EventArgs> PickStarted;

        /// <summary>
        /// Occurs when the eyedropper stops to take color.
        /// </summary>
        public event TypedEventHandler<EyedropperToolButton, EventArgs> PickCompleted;

        private void HookUpEvents()
        {
            Click += EyedropperToolButton_Click;
            Unloaded += EyedropperToolButton_Unloaded;
            ActualThemeChanged += EyedropperToolButton_ActualThemeChanged;
            Window.Current.SizeChanged += Window_SizeChanged;
            _eyedropper.ColorChanged += Eyedropper_ColorChanged;
            _eyedropper.PickStarted += Eyedropper_PickStarted;
            _eyedropper.PickCompleted += Eyedropper_PickCompleted;
        }

        private void UnhookEvents()
        {
            Click -= EyedropperToolButton_Click;
            Unloaded -= EyedropperToolButton_Unloaded;
            ActualThemeChanged -= EyedropperToolButton_ActualThemeChanged;
            Window.Current.SizeChanged -= Window_SizeChanged;
            _eyedropper.ColorChanged -= Eyedropper_ColorChanged;
            _eyedropper.PickStarted -= Eyedropper_PickStarted;
            _eyedropper.PickCompleted -= Eyedropper_PickCompleted;
            if (TargetElement != null)
            {
                TargetElement = null;
            }

            if (EyedropperEnabled)
            {
                EyedropperEnabled = false;
            }
        }

        private void EyedropperToolButton_Loaded(object sender, RoutedEventArgs e)
        {
            HookUpEvents();
        }

        private void EyedropperToolButton_Unloaded(object sender, RoutedEventArgs e)
        {
            UnhookEvents();
        }

        private void EyedropperToolButton_ActualThemeChanged(FrameworkElement sender, object args)
        {
            _eyedropper.RequestedTheme = this.ActualTheme;
        }

        /// <inheritdoc />
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledPointerOverState : PointerOverState, true);
        }

        /// <inheritdoc />
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledState : NormalState, true);
        }

        /// <inheritdoc />
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledState : NormalState, true);
        }

        private void Eyedropper_PickStarted(Eyedropper sender, EventArgs args)
        {
            PickStarted?.Invoke(this, args);
        }

        private void Eyedropper_PickCompleted(Eyedropper sender, EventArgs args)
        {
            EyedropperEnabled = false;
            PickCompleted?.Invoke(this, args);
        }

        private void Eyedropper_ColorChanged(Eyedropper sender, EyedropperColorChangedEventArgs args)
        {
            Color = args.NewColor;
            ColorChanged?.Invoke(this, args);
        }

        private void OnIsEnabledChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (IsEnabled)
            {
                if (IsPressed)
                {
                    VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledPressedState : PressedState, true);
                }
                else if (IsPointerOver)
                {
                    VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledPointerOverState : PointerOverState, true);
                }
                else
                {
                    VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledState : NormalState, true);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, EyedropperEnabled ? EyedropperEnabledDisabledState : DisabledState, true);
            }
        }

        private void EyedropperToolButton_Click(object sender, RoutedEventArgs e)
        {
            EyedropperEnabled = !EyedropperEnabled;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateEyedropperWorkArea();
        }

        private async void UpdateEyedropperWorkArea()
        {
            if (TargetElement != null)
            {
                var transform = TargetElement.TransformToVisual(Window.Current.Content);
                var position = transform.TransformPoint(default(Point));
                _eyedropper.WorkArea = new Rect(position, new Size(TargetElement.ActualWidth, TargetElement.ActualHeight));
                await _eyedropper.UpdateAppScreenshotAsync();
            }
        }
    }
}
