using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    class CarouselSlot : ContentControl
    {
        internal static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(CarouselSlot), new PropertyMetadata(null));

        private Storyboard _storyboard;

        internal CarouselSlot()
        {
            Tapped += OnTapped;
        }

        internal double X { get; set; }

        internal ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }
        
        internal void MoveX(double x, double duration = 0)
        {
            if (_storyboard != null)
            {
                _storyboard.Pause();
                _storyboard = null;
            }
            if (duration > 0)
            {
                _storyboard = this.AnimateX(x, duration, new CircleEase());
            }
            else
            {
                this.TranslateX(x);
            }
            X = x;
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ItemClickCommand != null)
            {
                var contentControl = sender as ContentControl;
                if (contentControl != null)
                {
                    if (ItemClickCommand.CanExecute(contentControl.Content))
                    {
                        ItemClickCommand.Execute(contentControl.Content);
                    }
                }
            }
        }
    }
}
