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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
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
        private CarouselPanel carouselPanel;

        /// <summary>
        /// Gets or sets selected Index
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
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
        public double ItemDepth
        {
            get { return (double)GetValue(ItemDepthProperty); }
            set { SetValue(ItemDepthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Depth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemDepthProperty = DependencyProperty.Register("ItemDepth", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnCarouselPropertyChanged));

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
        /// Gets or sets a value indicating whether the transformation opposite beetween the selected item
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
            carouselControl.UpdatePositions();
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
            this.DefaultStyleKey = typeof(Carousel);
            this.ManipulationMode = ManipulationModes.All;
            this.IsHitTestVisible = true;

            // Activating the focus visual default behavior
            this.UseSystemFocusVisuals = true;

            // Events registered
            this.RegisterPropertyChangedCallback(ItemsSourceProperty, (d, dp) => ((Carousel)d).UpdatePositions());
            this.PointerWheelChanged += OnPointerWheelChanged;
            this.PointerReleased += CarouselControl_PointerReleased;
            this.KeyDown += Keyboard_KeyUp;
        }

        /// <summary>
        /// Launch an update positions (and animations) on the ItemsPanel
        /// </summary>
        private void UpdatePositions()
        {
            if (this.ItemsPanel == null)
            {
                return;
            }

            var itemsPanel = this.GetItemsPanel();

            if (itemsPanel == null)
            {
                return;
            }

            itemsPanel.UpdatePosition();
        }

        /// <summary>
        /// Subsribe to a Dependency Property Changed Event
        /// </summary>
        public void SubscribePropertyChanged(string property, PropertyChangedCallback propertyChangedCallback)
        {
            Binding b = new Binding { Path = new PropertyPath(property), Source = this };
            var prop = DependencyProperty.RegisterAttached(property, typeof(object), typeof(Control), new PropertyMetadata(null, propertyChangedCallback));

            this.SetBinding(prop, b);
        }

        private void CarouselControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Focus(FocusState.Pointer);
            e.Handled = true;
        }

        private void Keyboard_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                    if (this.Orientation == Orientation.Vertical)
                    {
                        if (this.SelectedIndex < this.Items.Count - 1)
                        {
                            this.SelectedIndex++;
                        }

                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                    if (this.Orientation == Orientation.Vertical)
                    {
                        if (this.SelectedIndex > 0)
                        {
                            this.SelectedIndex--;
                        }

                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        if (this.SelectedIndex > 0)
                        {
                            this.SelectedIndex--;
                        }

                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        if (this.SelectedIndex < this.Items.Count - 1)
                        {
                            this.SelectedIndex++;
                        }

                        e.Handled = true;
                    }

                    break;
            }
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (carouselPanel == null)
            {
                carouselPanel = this.GetItemsPanel();
            }

            if (carouselPanel == null)
            {
                return;
            }

            carouselPanel.OnManipulationDelta(this, e);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            if (carouselPanel == null)
            {
                carouselPanel = this.GetItemsPanel();
            }

            if (carouselPanel == null)
            {
                return;
            }

            carouselPanel.OnManipulationCompleted(this, e);
        }

        internal void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var index = e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 ? -1 : 1;
            if (index == -1 && this.SelectedIndex > 0)
            {
                this.SelectedIndex--;
            }

            if (index == 1 && this.SelectedIndex < this.Items.Count - 1)
            {
                this.SelectedIndex++;
            }

            e.Handled = true;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (this.ItemTemplate == null)
            {
                return;
            }

            var contentControl = element as ContentControl;

            // load data templated
            var contentElement = this.ItemTemplate.LoadContent() as FrameworkElement;

            if (contentElement == null)
            {
                return;
            }

            contentControl.DataContext = item;
            contentControl.Content = contentElement;
            contentControl.Opacity = 1;
            contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
            contentControl.IsTabStop = false;

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
