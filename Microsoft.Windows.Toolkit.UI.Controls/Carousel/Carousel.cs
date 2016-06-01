using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The Carousel offer an alternative to items visualization adding horizontal scroll to a set of items. 
    /// The Carousel control is responsive by design, optimizing the visualization in the different form factors. 
    /// You can control properties like the AspectRatio, MaxItems, MinHeight, MaxHeight, GradientOpacity and AlignmentX to properly behave depending on the resolution and space available.
    /// </summary>
    [TemplatePart(Name = "Container", Type = typeof(Panel))]
    [TemplatePart(Name = "PrevArrow", Type = typeof(Button))]
    [TemplatePart(Name = "NextArrow", Type = typeof(Button))]
    [TemplatePart(Name = "Gradient", Type = typeof(LinearGradientBrush))]
    [TemplatePart(Name = "Clip", Type = typeof(RectangleGeometry))]
    public sealed partial class Carousel : Control
    {
        private Panel _container;
        private Button _prevArrow;
        private Button _nextArrow;
        private LinearGradientBrush _gradient;
        private RectangleGeometry _clip;

        private readonly List<object> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Carousel"/> class.
        /// </summary>
        public Carousel()
        {
            _items = new List<object>();
            DefaultStyleKey = typeof(Carousel);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture children controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_prevArrow != null)
            {
                _prevArrow.Click -= OnPrevArrowClick;
            }

            if (_nextArrow != null)
            {
                _nextArrow.Click -= OnNextArrowClick;
            }

            if (_container != null)
            {
                _container.ManipulationDelta -= OnManipulationDelta;
                _container.ManipulationCompleted -= OnManipulationCompleted;
            }

            SizeChanged -= OnSizeChanged;

            _container = GetTemplateChild("Container") as Panel;
            _prevArrow = GetTemplateChild("PrevArrow") as Button;
            _nextArrow = GetTemplateChild("NextArrow") as Button;
            _gradient = GetTemplateChild("Gradient") as LinearGradientBrush;
            _clip = GetTemplateChild("Clip") as RectangleGeometry;

            BuildSlots();
            ItemsSourceChanged(ItemsSource as IEnumerable);

            if (_container != null)
            {
                _container.ManipulationDelta += OnManipulationDelta;
                _container.ManipulationCompleted += OnManipulationCompleted;
                _container.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;
            }

            if (_prevArrow != null)
            {
                _prevArrow.Click += OnPrevArrowClick;
            }

            if (_nextArrow != null)
            {
                _nextArrow.Click += OnNextArrowClick;
            }

            SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the FrameworkElement-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is availabl</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            double contentWidth = availableSize.Width;
            double width = contentWidth / MaxItems;
            double height = width / AspectRatio;
            if (height < MinHeight)
            {
                height = MinHeight;
                width = height * AspectRatio;
            }
            if (height > MaxHeight)
            {
                height = MaxHeight;
                width = height * AspectRatio;
            }
            var size = new Size(width, height);
            base.MeasureOverride(size);
            return size;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a FrameworkElement derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            if (_container != null)
            {
                double slotWidth = Math.Round(Math.Min(size.Width, Math.Max(_container.ActualWidth/MaxItems, size.Height*AspectRatio)), 2);
                double factor = Math.Round(_slotWidth/slotWidth, 2);
                factor = factor == 0 ? 1 : factor;
                _slotWidth = Math.Round(slotWidth, 2);
                _offset = Math.Round((_offset/factor).Mod(_slotWidth), 2);

                var positions = GetPositions(_slotWidth).ToArray();
                var controls = _container.Children.Cast<CarouselSlot>().OrderBy(r => r.X).ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    var position = positions[n];
                    var control = controls[n];
                    control.MoveX(position.X + _offset);
                    control.Width = _slotWidth;
                    control.Height = _container.ActualHeight;
                }
            }

            return size;
        }

        /// <summary>
        /// Called before the PointerEntered event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            _prevArrow?.FadeIn(500.0);
            _nextArrow?.FadeIn(500.0);
            base.OnPointerEntered(e);
        }

        /// <summary>
        /// Called before the PointerExited event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            _prevArrow?.FadeOut(500.0);
            _nextArrow?.FadeOut(500.0);
            base.OnPointerExited(e);
        }

        private void OnPrevArrowClick(object sender, RoutedEventArgs e)
        {
            AnimatePrev();
        }
        private void OnNextArrowClick(object sender, RoutedEventArgs e)
        {
            AnimateNext();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_prevArrow != null)
            {
                _prevArrow.Height = e.NewSize.Height;
            }

            if (_nextArrow != null)
            {
                _nextArrow.Height = e.NewSize.Height;
            }

            ApplyGradient();

            if (_clip == null || _container == null)
            {
                return;
            }
            _clip.Rect = new Rect(new Point(), new Size(_container.ActualWidth, _container.ActualHeight));
        }

        private void ApplyGradient()
        {
            if (_gradient == null)
            {
                return;
            }

            if (MaxItems > 2)
            {
                double factor = 1.0 / MaxItems;
                int index = MaxItems / 2;
                int count = 1;
                if (MaxItems % 2 == 0)
                {
                    index--;
                    count++;
                }
                _gradient.GradientStops[1].Offset = factor * index;
                _gradient.GradientStops[2].Offset = factor * (index + count);
            }
        }
    }
}
