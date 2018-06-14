// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation.Metadata;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Helper class that provides attached properties to enable any TextBox with the Surface Dial. Rotate to change the value by StepValue between MinValue and MaxValue, and tap to go to the Next focus element from a TextBox
    /// </summary>
    public class SurfaceDialTextbox
    {
       /// <summary>
        /// If you provide the Controller yourself, set this to true so you won't add new menu items.
        /// </summary>
        public static readonly DependencyProperty ForceMenuItemProperty =
            DependencyProperty.RegisterAttached("ForceMenuItem", typeof(bool), typeof(SurfaceDialTextbox), new PropertyMetadata(false));

        /// <summary>
        /// Set the default icon of the menu item that gets added. A user will most likely not see this. Defaults to the Ruler icon.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(RadialControllerMenuKnownIcon), typeof(SurfaceDialTextbox), new PropertyMetadata(RadialControllerMenuKnownIcon.Ruler));

        /// <summary>
        /// The amount the TextBox will be modified for each rotation step on the Surface Dial. This can be any double value.
        /// </summary>
        public static readonly DependencyProperty StepValueProperty =
            DependencyProperty.RegisterAttached("StepValue", typeof(double), typeof(SurfaceDialTextbox), new PropertyMetadata(0d, new PropertyChangedCallback(StepValueChanged)));

        /// <summary>
        /// A flag to enable or disable haptic feedback when rotating the dial for the give TextBox. This is enabled by default.
        /// </summary>
        public static readonly DependencyProperty EnableHapticFeedbackProperty =
            DependencyProperty.RegisterAttached("EnableHapticFeedback", typeof(bool), typeof(SurfaceDialTextbox), new PropertyMetadata(true));

        /// <summary>
        /// Sets the minimum value the TextBox can have when modifying it using a Surface Dial. Default is -100.0
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.RegisterAttached("MinValue", typeof(double), typeof(SurfaceDialTextbox), new PropertyMetadata(-100d));

        /// <summary>
        /// Sets the maxium value the TextBox can have when modifying it using a Surface Dial. Default is 100.0
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.RegisterAttached("MaxValue", typeof(double), typeof(SurfaceDialTextbox), new PropertyMetadata(100d));

        /// <summary>
        /// TapToNext is a feature you can set to automatically try to focus the next focusable element from the Surface Dial enabled TextBox. This is on dy default.
        /// </summary>
        public static readonly DependencyProperty EnableTapToNextControlProperty =
            DependencyProperty.RegisterAttached("EnableTapToNextControl", typeof(bool), typeof(SurfaceDialTextbox), new PropertyMetadata(true));

        /// <summary>
        /// EnableMinMax limits the value in the textbox to your spesificed Min and Max values, see the other properties.
        /// </summary>
        public static readonly DependencyProperty EnableMinMaxValueProperty =
            DependencyProperty.RegisterAttached("EnableMinMaxValue", typeof(bool), typeof(SurfaceDialTextbox), new PropertyMetadata(false));

        /// <summary>
        /// Getter of the EnableMinMax property
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static bool GetEnableMinMaxValue(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableMinMaxValueProperty);
        }

        /// <summary>
        /// Setter of the EnableMinMax property
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetEnableMinMaxValue(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableMinMaxValueProperty, value);
        }

        /// <summary>
        /// Getter of the TapToNext flag.
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static bool GetEnableTapToNextControl(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableTapToNextControlProperty);
        }

        /// <summary>
        /// Setter of the TapToNext flag.
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetEnableTapToNextControl(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableTapToNextControlProperty, value);
        }

        /// <summary>
        /// Getter of the MaxValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static double GetMaxValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MaxValueProperty);
        }

        /// <summary>
        /// Setter of the MaxValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetMaxValue(DependencyObject obj, double value)
        {
            obj.SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Getter of the MinValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static double GetMinValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MinValueProperty);
        }

        /// <summary>
        /// Setter of the MinValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetMinValue(DependencyObject obj, double value)
        {
            obj.SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// Setter of the StepValue.
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static double GetStepValue(DependencyObject obj)
        {
            return (double)obj.GetValue(StepValueProperty);
        }

        /// <summary>
        /// Getter of the StepValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetStepValue(DependencyObject obj, double value)
        {
            obj.SetValue(StepValueProperty, value);
        }

        /// <summary>
        /// Getter of the Icon
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static RadialControllerMenuKnownIcon GetIcon(DependencyObject obj)
        {
            return (RadialControllerMenuKnownIcon)obj.GetValue(IconProperty);
        }

        /// <summary>
        /// Setter of the Icon
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetIcon(DependencyObject obj, RadialControllerMenuKnownIcon value)
        {
            obj.SetValue(IconProperty, value);
        }

        /// <summary>
        /// Setter of the Haptic Feedback property
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static bool GetEnableHapticFeedback(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableHapticFeedbackProperty);
        }

        /// <summary>
        /// Getter of the Haptic Feedback property
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetEnableHapticFeedback(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableHapticFeedbackProperty, value);
        }

        /// <summary>
        /// Getter of the Force Menu Item flag
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static bool GetForceMenuItem(DependencyObject obj)
        {
            return (bool)obj?.GetValue(ForceMenuItemProperty);
        }

        /// <summary>
        /// Setter of the Force Menu Item flag
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetForceMenuItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ForceMenuItemProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether this attached proeprty is supported
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (!ApiInformation.IsTypePresent("Windows.UI.Input.RadialController"))
                {
                    return false;
                }

                if (!RadialController.IsSupported())
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// The Surface Dial controller instance itself
        /// </summary>
        private static RadialController _controller;

        /// <summary>
        /// A default menu item that will be used for this to function. It will automatically be cleaned up when you move away from the TextBox, and created on Focus.
        /// </summary>
        private static RadialControllerMenuItem _stepTextMenuItem;

        /// <summary>
        /// The textbox itself needed to refernece the current TextBox that is being modified
        /// </summary>
        private static TextBox _textBox;

        /// <summary>
        /// Gets or sets the controller for the Surface Dial. The RadialController can be set from your app logic in case you use Surface Dial in other custom cases than on a TextBox.
        /// This helper class will do everything for you, but if you want to control the Menu Items and/or wish to use the same Surface Dial insta
        /// This is the property for the static controller so you can access it if needed.
        /// </summary>
        public static RadialController Controller
        {
            get
            {
                return _controller;
            }

            set
            {
                _controller = value;
            }
        }

        /// <summary>
        /// This function gets called every time there is a rotational change on the connected Surface Dial while a Surface Dial enabled TextBox is in focus.
        /// This function ensures that the TextBox stays within the set range between MinValue and MaxValue while rotating the Surface Dial.
        /// It defaults the content of the TextBox to 0.0 if a non-numerical value is detected.
        /// </summary>
        /// <param name="sender">The RadialController being used.</param>
        /// <param name="args">The arguments of the changed event.</param>
        private static void Controller_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (_textBox == null)
            {
                return;
            }

            string t = _textBox.Text;
            double nr;

            if (double.TryParse(t, out nr))
            {
                nr += args.RotationDeltaInDegrees * GetStepValue(_textBox);
                if (GetEnableMinMaxValue(_textBox))
                {
                    if (nr < GetMinValue(_textBox))
                    {
                        nr = GetMinValue(_textBox);
                    }

                    if (nr > GetMaxValue(_textBox))
                    {
                        nr = GetMaxValue(_textBox);
                    }
                }
            }
            else
            {
                // default to zero if content is not a number
                nr = 0.0d;
            }

            _textBox.Text = nr.ToString("0.00");
        }

        /// <summary>
        /// Sets up the events needed for the current TextBox so it can trigger on GotFocus and LostFocus
        /// </summary>
        /// <param name="d">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="e">The arguments of the changed event.</param>
        private static void StepValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!IsSupported)
            {
                return;
            }

            var textBox = d as TextBox;

            if (textBox == null)
            {
                return;
            }

            textBox.GotFocus -= TextBox_GotFocus;
            textBox.LostFocus -= TextBox_LostFocus;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
        }

        /// <summary>
        /// When the focus of the TextBox is lost, ensure we clean up the events and Surface Dial menu item.
        /// </summary>
        /// <param name="sender">The TextBox in being affected.</param>
        /// <param name="e">The event arguments.</param>
        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_textBox == null)
            {
                return;
            }

            if (GetForceMenuItem(_textBox))
            {
                _controller.Menu.Items.Remove(_stepTextMenuItem);
            }

            _controller.RotationChanged -= Controller_RotationChanged;
            if (GetEnableTapToNextControl(_textBox))
            {
                _controller.ButtonClicked -= Controller_ButtonClicked;
            }

            _textBox = null;
        }

        /// <summary>
        /// When a Surface Dial TextBox gets focus, ensure the proper events are setup, and connect the Surface Dial itself.
        /// </summary>
        /// <param name="sender">The TextBox in being affected.</param>
        /// <param name="e">The event arguments.</param>
        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _textBox = sender as TextBox;

            if (_textBox == null)
            {
                return;
            }

            if (!IsSupported)
            {
                return;
            }

            _controller = _controller ?? RadialController.CreateForCurrentView();

            if (GetForceMenuItem(_textBox))
            {
                _stepTextMenuItem = RadialControllerMenuItem.CreateFromKnownIcon("Step Text Box", GetIcon(_textBox));
                _controller.Menu.Items.Add(_stepTextMenuItem);
                _controller.Menu.SelectMenuItem(_stepTextMenuItem);
            }

            _controller.UseAutomaticHapticFeedback = GetEnableHapticFeedback(_textBox);
            _controller.RotationResolutionInDegrees = 1;
            _controller.RotationChanged += Controller_RotationChanged;
            if (GetEnableTapToNextControl(_textBox))
            {
                _controller.ButtonClicked += Controller_ButtonClicked;
            }
        }

        /// <summary>
        /// If the TapToNext flag is enabled, this function will try to take the focus to the next focusable element.
        /// </summary>
        /// <param name="sender">The RadialController being used.</param>
        /// <param name="args">The arguments of the changed event.</param>
        private static void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
        }
    }
}
