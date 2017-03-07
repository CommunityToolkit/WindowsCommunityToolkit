using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The<see cref="FlyoutView" /> is an open-ended container that can show arbitrary UI as its content.
    /// </summary>
    /// <seealso cref="ContentControl" />
    [TemplatePart(Name = PartOverlay, Type = typeof(Grid))]
    [TemplatePart(Name = PartContent, Type = typeof(Grid))]
    public partial class FlyoutView : ContentControl
    {
        /// <summary>
        /// Placement mode
        /// </summary>
        public enum PlacementType
        {
            /// <summary>
            /// View appears from the left
            /// </summary>
            Left,

            /// <summary>
            /// View appears from the top
            /// </summary>
            Top,

            /// <summary>
            /// View appears from the right
            /// </summary>
            Right,

            /// <summary>
            /// View appears from the bottom
            /// </summary>
            Bottom
        }

        private const string PartOverlay = "PART_OVERLAY";
        private const string PartContent = "PART_CONTENT";

        private Grid _overlay;
        private Grid _content;

        private Storyboard _openStoryboard;
        private Storyboard _closeStoryboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutView"/> class.
        /// </summary>
        public FlyoutView()
        {
            DefaultStyleKey = typeof(FlyoutView);
        }

        protected override void OnApplyTemplate()
        {
            _overlay = GetTemplateChild(PartOverlay) as Grid;
            _content = GetTemplateChild(PartContent) as Grid;

            if (_content != null)
            {
                _content.Loaded += OnContentLoaded;
                HandleOpening();
            }

            if (_overlay != null)
            {
                _overlay.Tapped += OnOverlayTapped;
            }

            UpdateFromLayout();

            base.OnApplyTemplate();
        }

        private void OnContentLoaded(object sender, object o)
        {
            _content.Loaded -= OnContentLoaded;

            UpdateFromLayout();
        }

        private void UpdateFromLayout()
        {
            if (_content == null || _overlay == null)
            {
                return;
            }

            double overflow;
            if (Placement == PlacementType.Left
                || Placement == PlacementType.Right)
            {
                overflow = Placement == PlacementType.Left
                   ? -ContentWidth
                   : ContentWidth;
            }
            else
            {
                overflow = Placement == PlacementType.Top
                ? -ContentHeight
                : ContentHeight;
            }

            UpdatePlacement(overflow);
            UpdateAnimations(overflow);
        }

        private void UpdatePlacement(double overflow)
        {
            if (Placement == PlacementType.Left
                || Placement == PlacementType.Right)
            {
                _content.Width = ContentWidth;
                _content.Height = double.NaN;
                _content.RenderTransform = new CompositeTransform { TranslateX = overflow, TranslateY = 0 };
                _content.HorizontalAlignment = Placement == PlacementType.Left
                    ? HorizontalAlignment.Left
                    : HorizontalAlignment.Right;
                _content.VerticalAlignment = VerticalAlignment.Stretch;
            }
            else
            {
                _content.Width = double.NaN;
                _content.Height = ContentHeight;
                _content.RenderTransform = new CompositeTransform { TranslateX = 0, TranslateY = overflow };
                _content.VerticalAlignment = Placement == PlacementType.Top
                    ? VerticalAlignment.Top
                    : VerticalAlignment.Bottom;
                _content.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }

        private void UpdateAnimations(double overflow)
        {
            var targetProperty = Placement == PlacementType.Left || Placement == PlacementType.Right
                ? "(UIElement.RenderTransform).(CompositeTransform.TranslateX)"
                : "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";

            // Create animations
            _openStoryboard = new Storyboard();
            var openAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(500),
                From = overflow,
                To = 0,
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseIn, Power = 2 }
            };
            Storyboard.SetTargetProperty(openAnimation, targetProperty);
            Storyboard.SetTarget(openAnimation, _content);
            _openStoryboard.Children.Add(openAnimation);

            _closeStoryboard = new Storyboard();
            var closeAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(500),
                From = 0,
                To = overflow,
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut, Power = 2 }
            };
            Storyboard.SetTargetProperty(closeAnimation, targetProperty);
            Storyboard.SetTarget(closeAnimation, _content);
            _closeStoryboard.Children.Add(closeAnimation);
        }

        private void OnOverlayTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (IsOpen)
            {
                IsOpen = false;
            }
        }
    }
}
