using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class ToolkitTextBox : TextBox
    {

        private UIElement _headerContentPresenter;

        public double HeaderOffsetY {
            get => (double)this.GetValue(HeaderOffsetYProperty);
            set => this.SetValue(HeaderOffsetYProperty, value);
        }

        public static readonly DependencyProperty HeaderOffsetYProperty = DependencyProperty.Register(nameof(HeaderOffsetY), typeof(double), typeof(ToolkitTextBox), new PropertyMetadata(26D));

        public double HeaderAnimationDuration {
            get => (double)this.GetValue(HeaderAnimationDurationProperty);
            set => this.SetValue(HeaderAnimationDurationProperty, value);
        }

        public static readonly DependencyProperty HeaderAnimationDurationProperty = DependencyProperty.Register(nameof(HeaderAnimationDuration), typeof(double), typeof(ToolkitTextBox), new PropertyMetadata(300D));

        public ToolkitTextBox()
        {
            this.DefaultStyleKey = typeof(ToolkitTextBox);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            MoveHeaderUp(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            MoveHeaderDown(true);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerContentPresenter = this.GetTemplateChild("HeaderContentPresenter") as UIElement;
            MoveHeaderDown(false);
        }

        protected virtual void MoveHeaderUp(bool animate)
        {
            if (this.Header != null)
            {
                var duration = animate ? HeaderAnimationDuration : 0;
                _ = Microsoft.Toolkit.Uwp.UI.Animations.AnimationExtensions.Offset(_headerContentPresenter, 0, 0, duration).StartAsync();
            }
        }

        protected virtual void MoveHeaderDown(bool animate)
        {
            if (this.Header != null && string.IsNullOrWhiteSpace(this.Text) && HeaderOffsetY > 0)
            {
                var duration = animate ? HeaderAnimationDuration : 0;
                _ = Microsoft.Toolkit.Uwp.UI.Animations.AnimationExtensions.Offset(_headerContentPresenter, 10, (float)HeaderOffsetY, duration).StartAsync();
            }
        }
    }
}
