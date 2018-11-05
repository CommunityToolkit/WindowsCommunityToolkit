// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;


namespace LottieViewer
{
    public delegate void ScrubberValueChangedEventHandler(Scrubber sender, ScrubberValueChangedEventArgs args);

    public sealed class ScrubberValueChangedEventArgs
    {
        internal ScrubberValueChangedEventArgs(double oldValue, double newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public double OldValue { get; }
        public double NewValue { get; }
    }
    public sealed partial class Scrubber : UserControl
    {
        readonly CompositionPropertySet _properties;
        readonly SpriteVisual _trackbar;
        readonly ShapeVisual _thumb;
        readonly CompositionBrush _transparentBrush;
        readonly CompositionBrush _scrubberEnabledBrush;
        readonly CompositionBrush _scrubberDisabledBrush;

        public event ScrubberValueChangedEventHandler ValueChanged;

        public Scrubber()
        {
            this.InitializeComponent();

            const float thumbRadius = 8;
            const float thumbStrokeThickness = 1.5F;

            var c = Window.Current.Compositor;

            // Get the brushes
            _scrubberEnabledBrush = c.CreateColorBrush((Color)App.Current.Resources["LottieBasic"]);
            _scrubberDisabledBrush = c.CreateColorBrush((Color)App.Current.Resources["DisabledColor"]);

            // Forward slider change events.
            _slider.ValueChanged += (sender, e) => ValueChanged?.Invoke(this, new ScrubberValueChangedEventArgs(e.OldValue, e.NewValue));

            // Create comp objects for the trackbar and the thumb.
            var container = c.CreateContainerVisual();

            // Save the properties. These will be used to animate the trackbard and thumb.
            _properties = container.Properties;

            // Create the trackbar
            _trackbar = c.CreateSpriteVisual();
            container.Children.InsertAtTop(_trackbar);

            // Move the trackbar into the horizontal track of the slider.
            _trackbar.Offset = new System.Numerics.Vector3(1, 18, 0);

            // Create the thumb.
            _thumb = c.CreateShapeVisual();
            var thumbEllipse = c.CreateEllipseGeometry();
            thumbEllipse.Radius = new System.Numerics.Vector2(thumbRadius);
            thumbEllipse.Center = new System.Numerics.Vector2(thumbRadius + thumbStrokeThickness);

            var thumbShape = c.CreateSpriteShape(thumbEllipse);
            thumbShape.FillBrush = _scrubberEnabledBrush;
            thumbShape.StrokeBrush = _scrubberEnabledBrush;
            thumbShape.StrokeThickness = thumbStrokeThickness;
            _thumb.Shapes.Add(thumbShape);
            _thumb.Size = new System.Numerics.Vector2((thumbRadius + thumbStrokeThickness) * 2);
            _thumb.Offset = new System.Numerics.Vector3(0, 7, 0);
            container.Children.InsertAtTop(_thumb);

            _transparentBrush = c.CreateColorBrush(Colors.Transparent);
            _trackbar.Brush = _scrubberEnabledBrush;

            _properties.InsertScalar("Width", 0);

            ElementCompositionPreview.SetElementChildVisual(_slider, container);

            // Change colors on enabled/disabled transitions.
            IsEnabledChanged += (sender, e) =>
            {
                if ((bool)e.NewValue)
                {
                    // Becoming enabled.
                    _trackbar.Brush = _scrubberEnabledBrush;
                    thumbShape.FillBrush = _scrubberEnabledBrush;
                    thumbShape.StrokeBrush = _scrubberEnabledBrush;
                }
                else
                {
                    // Becoming disabled.
                    _trackbar.Brush = _scrubberDisabledBrush;
                    thumbShape.FillBrush = _scrubberDisabledBrush;
                    thumbShape.StrokeBrush = _scrubberDisabledBrush;
                }
            };
        }

        /// <summary>
        /// The current value of the scrubber. Value from 0 to 1.
        /// </summary>
        public double Value
        {
            get => _slider.Value;
            set => _slider.Value = value;
        }

        // Associates the given object with the scrubber. The object is expected to have a property called "Progress"
        // that the scrubber position will be bound to.
        internal void SetAnimatedCompositionObject(CompositionObject obj)
        {
            var c = Window.Current.Compositor;
            var rectAnim = c.CreateExpressionAnimation("Vector2(comp.Progress * our.Width, 2)");
            rectAnim.SetReferenceParameter("comp", obj);
            rectAnim.SetReferenceParameter("our", _properties);
            _trackbar.StartAnimation("Size", rectAnim);

            // For thumbRadius = 11:
            //var thumbAnim = c.CreateExpressionAnimation("Vector3((comp.Progress * (our.Width - 22)) - 1, 6, 0)");
            // For thumbRadius = 8
            var thumbAnim = c.CreateExpressionAnimation("Vector3((comp.Progress * (our.Width - 16)) - 1, 9.5, 0)");
            thumbAnim.SetReferenceParameter("comp", obj);
            thumbAnim.SetReferenceParameter("our", _properties);
            _thumb.StartAnimation("Offset", thumbAnim);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            // Update the size of the progress rectangle.
            _properties.InsertScalar("Width", (float)finalSize.Width - 2);

            return base.ArrangeOverride(finalSize);
        }
    }
}
