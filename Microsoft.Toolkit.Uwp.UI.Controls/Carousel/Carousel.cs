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
    public class Carousel : ItemsControl
    {
        /// <summary>
        /// Gets or sets the selected Item
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(Carousel), new PropertyMetadata(null, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets selected Index
        /// </summary>
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

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets duration of the easing function animation (ms)
        /// </summary>
        public int TransitionDuration
        {
            get { return (int)GetValue(TransitionDurationProperty); }
            set { SetValue(TransitionDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TransitionDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register("TransitionDuration", typeof(int), typeof(Carousel), new PropertyMetadata(200));

        /// <summary>
        /// Gets or sets depth of non Selected Index Items
        /// </summary>
        public int ItemDepth
        {
            get { return (int)GetValue(ItemDepthProperty); }
            set { SetValue(ItemDepthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Depth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemDepthProperty = DependencyProperty.Register("ItemDepth", typeof(int), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets easing function to apply for each Transition
        /// </summary>
        public EasingFunctionBase EasingFunction
        {
            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EasingFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(Carousel), new PropertyMetadata(new ExponentialEase { EasingMode = EasingMode.EaseOut }));

        /// <summary>
        /// Gets or sets the item margin
        /// </summary>
        public int ItemMargin
        {
            get { return (int)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the items rendered transformations should be opposite compared to the selected item
        /// If false, all the items (except the selected item) will have the exact same transformations
        /// If true, all the items where index > selected index will have an opposite tranformation (Rotation X Y and Z will be multiply by -1)
        /// </summary>
        public bool InvertPositive
        {
            get { return (bool)GetValue(InvertPositiveProperty); }
            set { SetValue(InvertPositiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InvertPostive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvertPositiveProperty =
            DependencyProperty.Register("InvertPositive", typeof(bool), typeof(Carousel), new PropertyMetadata(true, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets rotation angle on X
        /// </summary>
        public double ItemRotationX
        {
            get { return (double)GetValue(ItemRotationXProperty); }
            set { SetValue(ItemRotationXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRotationXProperty = DependencyProperty.Register("ItemRotationX", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets rotation angle on Y
        /// </summary>
        public double ItemRotationY
        {
            get { return (double)GetValue(ItemRotationYProperty); }
            set { SetValue(ItemRotationYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRotationYProperty = DependencyProperty.Register("ItemRotationY", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets rotation angle on Z
        /// </summary>
        public double ItemRotationZ
        {
            get { return (double)GetValue(ItemRotationZProperty); }
            set { SetValue(ItemRotationZProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRotationZProperty = DependencyProperty.Register("ItemRotationZ", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

        /// <summary>
        /// Gets or sets the Carousel orientation. Horizontal or Vertical
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxViewableItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Carousel), new PropertyMetadata(Orientation.Horizontal, OnCarouselPropertyChanged));

        /// <summary>
        /// Each time a property change, we have to reposition
        /// </summary>
        private static void OnCarouselPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            Carousel carouselControl = (Carousel)d;

            if (e.Property == SelectedIndexProperty)
            {
                var item = carouselControl.Items[(int)e.NewValue];
                carouselControl.FocusContainerFromIndex((int)e.NewValue);

                // double check
                if (carouselControl.SelectedItem != item)
                {
                    carouselControl.SetValue(SelectedItemProperty, item);

                    return;
                }
            }
            else if (e.Property == SelectedItemProperty)
            {
                var index = carouselControl.Items.IndexOf(e.NewValue);

                // double check
                if (carouselControl.SelectedIndex != index)
                {
                    carouselControl.SetValue(SelectedIndexProperty, index);

                    return;
                }
            }

            carouselControl.UpdatePositions();
        }

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
        /// Return ItemsPanel
        /// </summary>
        /// <returns>Gets the ItemsPanel used for the carousel</returns>
        internal CarouselPanel GetItemsPanel()
        {
            return this.FindDescendant<CarouselPanel>();
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
                if (SelectedItem != null && Items[carouselControl.SelectedIndex] != SelectedItem)
                {
                    var index = carouselControl.Items.IndexOf(SelectedItem);

                    // Double check
                    if (carouselControl.SelectedIndex != index)
                    {
                        carouselControl.SetValue(SelectedIndexProperty, index);
                    }
                }
                else if (SelectedItem == null && SelectedIndex >= 0)
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

            itemsPanel.UpdatePosition();
        }

        private void CarouselControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Focus(FocusState.Pointer);
            e.Handled = true;
        }

        private void Keyboard_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                    if (Orientation == Orientation.Vertical)
                    {
                        if (SelectedIndex < Items.Count - 1)
                        {
                            SelectedIndex++;
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
                    if (Orientation == Orientation.Vertical)
                    {
                        if (SelectedIndex > 0)
                        {
                            SelectedIndex--;
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
                    if (Orientation == Orientation.Horizontal)
                    {
                        if (SelectedIndex > 0)
                        {
                            SelectedIndex--;
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
                    if (Orientation == Orientation.Horizontal)
                    {
                        if (SelectedIndex < Items.Count - 1)
                        {
                            SelectedIndex++;
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

        internal void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var index = e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 ? -1 : 1;
            if (index == -1 && SelectedIndex > 0)
            {
                SelectedIndex--;
            }

            if (index == 1 && SelectedIndex < Items.Count - 1)
            {
                SelectedIndex++;
            }

            e.Handled = true;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (ItemTemplate == null)
            {
                return;
            }

            var contentControl = element as ContentControl;

            // load data templated
            var contentElement = ItemTemplate.LoadContent() as FrameworkElement;

            if (contentElement == null)
            {
                return;
            }

            contentControl.DataContext = item;
            contentControl.Content = contentElement;
            contentControl.Opacity = 1;
            contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
            contentControl.IsTabStop = Items.IndexOf(item) == SelectedIndex;
            contentControl.UseSystemFocusVisuals = true;
            contentControl.Tag = "CarouselItem";

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
    }
}
