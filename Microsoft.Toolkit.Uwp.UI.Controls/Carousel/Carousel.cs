// ******************************************************************
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A modern UI Carousel control. Really flexible. Works with touch, keyboard, mouse.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ItemsControl"/>
    public class Carousel : ItemsControl
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for EasingFunction. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(Carousel), new PropertyMetadata(new ExponentialEase { EasingMode = EasingMode.EaseOut }));

        /// <summary>
        /// Using a DependencyProperty as the backing store for InvertPostive. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty InvertPositiveProperty = DependencyProperty.Register("InvertPositive", typeof(bool), typeof(Carousel), new PropertyMetadata(true, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsCircular. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsCircularProperty = DependencyProperty.Register("IsCircular", typeof(bool), typeof(Carousel), new PropertyMetadata(false, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Depth. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemDepthProperty = DependencyProperty.Register("ItemDepth", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for TranslateX. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Rotation. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemRotationXProperty = DependencyProperty.Register("ItemRotationX", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Rotation. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemRotationYProperty = DependencyProperty.Register("ItemRotationY", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Rotation. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemRotationZProperty = DependencyProperty.Register("ItemRotationZ", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for MaxViewableItems. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Carousel), new PropertyMetadata(Orientation.Horizontal, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for SelectedIndex. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(-1, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for SelectedItem. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Carousel), new PropertyMetadata(null, OnCarouselPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for TransitionDuration. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register("TransitionDuration", typeof(int), typeof(Carousel), new PropertyMetadata(200));

        /// <summary>
        /// Initializes a new instance of the <see cref="Carousel"/> class.
        /// </summary>
        public Carousel()
        {
            // Set style
            DefaultStyleKey = typeof(Carousel);
            SetValue(AutomationProperties.NameProperty, "Carousel");
            IsHitTestVisible = true;

            IsTabStop = false;
            TabNavigation = KeyboardNavigationMode.Once;

            // Events registered
            PointerWheelChanged += OnPointerWheelChanged;
            PointerReleased += CarouselControl_PointerReleased;
            KeyDown += Keyboard_KeyUp;

            // Register ItemSource changed to get correct SelectedItem and SelectedIndex
            RegisterPropertyChangedCallback(ItemsSourceProperty, (d, dp) =>
            {
                var carouselControl = (Carousel)d;
                var itemsPanel = carouselControl.GetItemsPanel();

                // Prioritize the SelectedItem over the SelectedIndex
                if (SelectedItem != null && (Items.Count() <= carouselControl.SelectedIndex || Items[carouselControl.SelectedIndex] != SelectedItem))
                {
                    var index = carouselControl.Items.IndexOf(SelectedItem);

                    // Double check
                    if (carouselControl.SelectedIndex != index)
                    {
                        carouselControl.SetValue(SelectedIndexProperty, index);
                    }
                }
                else if (SelectedItem == null && SelectedIndex >= 0 && carouselControl.Items != null && carouselControl.Items.Count > 0)
                {
                    var item = carouselControl.Items[SelectedIndex];

                    // Double check
                    if (carouselControl.SelectedItem != item)
                    {
                        carouselControl.SetValue(SelectedItemProperty, item);
                    }
                }
            });
        }

        /// <summary>
        /// Occurs when the selected item has changed
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Gets or sets easing function to apply for each Transition
        /// </summary>
        /// <value>The easing function.</value>
        public EasingFunctionBase EasingFunction
        {
            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the items rendered transformations should be opposite compared to the selected item If false, all the items (except the selected item) will have the
        /// exact same transformations If true, all the items where index &gt; selected index will have an opposite tranformation (Rotation X Y and Z will be multiply by -1)
        /// </summary>
        /// <value><c>true</c> if [invert positive]; otherwise, <c>false</c>.</value>
        public bool InvertPositive
        {
            get { return (bool)GetValue(InvertPositiveProperty); }
            set { SetValue(InvertPositiveProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is circular.
        /// </summary>
        /// <value><c>true</c> if this instance is circular; otherwise, <c>false</c>.</value>
        public bool IsCircular
        {
            get { return (bool)GetValue(IsCircularProperty); }
            set { SetValue(IsCircularProperty, value); }
        }

        /// <summary>
        /// Gets or sets depth of non Selected Index Items
        /// </summary>
        /// <value>The item depth.</value>
        public int ItemDepth
        {
            get { return (int)GetValue(ItemDepthProperty); }
            set { SetValue(ItemDepthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the item margin
        /// </summary>
        /// <value>The item margin.</value>
        public int ItemMargin
        {
            get { return (int)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets rotation angle on X
        /// </summary>
        /// <value>The item rotation x.</value>
        public double ItemRotationX
        {
            get { return (double)GetValue(ItemRotationXProperty); }
            set { SetValue(ItemRotationXProperty, value); }
        }

        /// <summary>
        /// Gets or sets rotation angle on Y
        /// </summary>
        /// <value>The item rotation y.</value>
        public double ItemRotationY
        {
            get { return (double)GetValue(ItemRotationYProperty); }
            set { SetValue(ItemRotationYProperty, value); }
        }

        /// <summary>
        /// Gets or sets rotation angle on Z
        /// </summary>
        /// <value>The item rotation z.</value>
        public double ItemRotationZ
        {
            get { return (double)GetValue(ItemRotationZProperty); }
            set { SetValue(ItemRotationZProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Carousel orientation. Horizontal or Vertical
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets selected Index
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }

            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected Item
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets duration of the easing function animation (ms)
        /// </summary>
        /// <value>The duration of the transition.</value>
        public int TransitionDuration
        {
            get { return (int)GetValue(TransitionDurationProperty); }
            set { SetValue(TransitionDurationProperty, value); }
        }

        /// <summary>
        /// Return ItemsPanel
        /// </summary>
        /// <returns>Gets the ItemsPanel used for the carousel</returns>
        internal CarouselPanel GetItemsPanel()
        {
            return this.FindDescendant<CarouselPanel>();
        }

        /// <summary>
        /// Called when [pointer wheel changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        internal void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var index = e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 ? -1 : 1;

            if (index == -1 && this.SelectedIndex >= 0)
            {
                // If Circular then move to the last and if not stay within the limit.
                this.SelectedIndex = (this.IsCircular && this.SelectedIndex == 0) ? this.Items.Count - 1 : Math.Max(this.SelectedIndex - 1, 0);
            }

            if (index == 1 && this.SelectedIndex <= this.Items.Count - 1)
            {
                // If Circular then move to the first and if not stay within the limit.
                this.SelectedIndex = (this.IsCircular && this.SelectedIndex == this.Items.Count - 1) ? 0 : Math.Min(this.SelectedIndex + 1, this.Items.Count - 1);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Returns the container used for each item
        /// </summary>
        /// <returns>Returns always a ContentControl</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentControl();
        }

        /// <summary>
        /// Determines whether the specified item is (or is eligible to be) its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>**true** if the item is (or is eligible to be) its own container; otherwise, **false**.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return false;
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        /// <exception cref="InvalidCastException">Any element added to the Carousel should be at least a Control component</exception>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            FrameworkElement contentControl = element as ContentControl;

            if (contentControl == null)
            {
                contentControl = element as FrameworkElement;
            }
            else if (ItemTemplate != null && (contentControl as ContentControl) != null)
            {
                ((ContentControl)contentControl).Content = ItemTemplate.LoadContent();
            }

            if (contentControl == null)
            {
                throw new InvalidCastException("Any element added to the Carousel should be at least a Control component");
            }

            contentControl.DataContext = item;
            contentControl.Opacity = 1;
            contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
            contentControl.Tag = "CarouselItem";

            if (contentControl as Control != null)
            {
                ((Control)contentControl).IsTabStop = Items.IndexOf(item) == SelectedIndex;
                ((Control)contentControl).UseSystemFocusVisuals = true;
            }

            PlaneProjection planeProjection = new PlaneProjection();
            planeProjection.CenterOfRotationX = 0.5;
            planeProjection.CenterOfRotationY = 0.5;
            planeProjection.CenterOfRotationZ = 0.5;

            var compositeTransform = new CompositeTransform();
            compositeTransform.CenterX = 0.5;
            compositeTransform.CenterY = 0.5;
            compositeTransform.CenterY = 0.5;

            contentControl.Projection = planeProjection;
            contentControl.RenderTransform = compositeTransform;
        }

        /// <summary>
        /// Each time a property change, we have to reposition
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCarouselPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carouselControl = (Carousel)d;

            if (e.Property == SelectedIndexProperty)
            {
                var newValue = (int)e.NewValue == -1 ? null : carouselControl.Items[(int)e.NewValue];
                var oldValue = e.OldValue == null ? null : carouselControl.Items.ElementAtOrDefault((int)e.OldValue);
                if (newValue != null)
                {
                    carouselControl.FocusContainerFromIndex((int)e.NewValue);
                }

                // double check
                if (carouselControl.SelectedItem != newValue)
                {
                    carouselControl.SetValue(SelectedItemProperty, newValue);
                    var newValues = new List<object>() { newValue };
                    var oldValues = oldValue == null ? new List<object>() : new List<object>() { oldValue };

                    var args = new SelectionChangedEventArgs(oldValues, newValues);
                    carouselControl.SelectionChanged?.Invoke(carouselControl, args);
                    return;
                }
            }
            else if (e.Property == SelectedItemProperty)
            {
                var index = e.NewValue == null ? -1 : carouselControl.Items.IndexOf(e.NewValue);

                // double check
                if (carouselControl.SelectedIndex != index)
                {
                    carouselControl.SetValue(SelectedIndexProperty, index);
                    var newValues = new List<object>() { e.NewValue };
                    var oldValues = e.OldValue == null ? new List<object>() : new List<object>() { e.OldValue };

                    var args = new SelectionChangedEventArgs(oldValues, newValues);
                    carouselControl.SelectionChanged?.Invoke(carouselControl, args);
                    return;
                }
            }

            carouselControl.UpdatePositions();
        }

        /// <summary>
        /// Handles the PointerReleased event of the CarouselControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        private void CarouselControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Focus(FocusState.Pointer);
            e.Handled = true;
        }

        /// <summary>
        /// Focuses the index of the container from.
        /// </summary>
        /// <param name="index">The index.</param>
        private void FocusContainerFromIndex(int index)
        {
            var oldElem = FocusManager.GetFocusedElement() as ContentControl;
            var newElem = ContainerFromIndex(index) as ContentControl;

            if (oldElem == newElem)
            {
                return;
            }

            if (newElem != null)
            {
                newElem.IsTabStop = true;
                newElem.Focus(oldElem != null && oldElem.FocusState != FocusState.Unfocused ? oldElem.FocusState : FocusState.Programmatic);
            }

            if (oldElem != null && (string)oldElem.Tag == "CarouselItem")
            {
                oldElem.IsTabStop = false;
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the Keyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void Keyboard_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                    if (this.Orientation == Orientation.Vertical)
                    {
                        if (this.SelectedIndex <= this.Items.Count - 1)
                        {
                            // If Circular then move to the first and if not stay within the limit.
                            this.SelectedIndex = (this.IsCircular && this.SelectedIndex == this.Items.Count - 1) ? 0 : Math.Min(this.SelectedIndex + 1, this.Items.Count - 1);
                        }
                        else if (e.OriginalKey != Windows.System.VirtualKey.Down)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Down);
                        }

                        e.Handled = true;
                    }

                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                    if (this.Orientation == Orientation.Vertical)
                    {
                        if (this.SelectedIndex >= 0)
                        {
                            // If Circular then move to the last and if not stay within the limit.
                            this.SelectedIndex = (this.IsCircular && this.SelectedIndex == 0) ? this.Items.Count - 1 : Math.Max(this.SelectedIndex - 1, 0);
                        }
                        else if (e.OriginalKey != Windows.System.VirtualKey.Up)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Up);
                        }

                        e.Handled = true;
                    }

                    break;

                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        if (this.SelectedIndex >= 0)
                        {
                            // If Circular then move to the last and if not stay within the limit.
                            this.SelectedIndex = (this.IsCircular && this.SelectedIndex == 0) ? this.Items.Count - 1 : Math.Max(this.SelectedIndex - 1, 0);
                        }
                        else if (e.OriginalKey != Windows.System.VirtualKey.Left)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Left);
                        }

                        e.Handled = true;
                    }

                    break;

                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        if (this.SelectedIndex <= this.Items.Count - 1)
                        {
                            // If Circular then move to the first and if not stay within the limit.
                            this.SelectedIndex = (this.IsCircular && this.SelectedIndex == this.Items.Count - 1) ? 0 : Math.Min(this.SelectedIndex + 1, this.Items.Count - 1);
                        }
                        else if (e.OriginalKey != Windows.System.VirtualKey.Right)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Right);
                        }

                        e.Handled = true;
                    }

                    break;
            }
        }

        /// <summary>
        /// Launch an update positions (and animations) on the ItemsPanel
        /// </summary>
        private void UpdatePositions()
        {
            if (ItemsPanel == null)
            {
                return;
            }

            var itemsPanel = GetItemsPanel();

            if (itemsPanel == null)
            {
                return;
            }

            itemsPanel.UpdatePosition(true);
        }
    }
}