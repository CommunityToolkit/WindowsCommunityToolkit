using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Allows <see cref="FrameworkElement.ActualHeight"/> and <see cref="FrameworkElement.ActualWidth"/> to be bound using a wrapper.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    [Bindable]
    public class BindableActualSizeBehavior : BehaviorBase<FrameworkElement>, INotifyPropertyChanged
    {

        /// <summary>
        /// Attaches the behavior to the associated object.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if attaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Initialize()
        {
            var result = AssignSizeChangedHandler();
            return result;
        }

        /// <summary>
        /// Detaches the behavior from the associated object.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if detaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Uninitialize()
        {
            RemoveSizeChangedHandler();
            return true;
        }

        /// <summary>
        /// The <see cref="FrameworkElement.ActualHeight"/> that can be bound.
        /// </summary>
        public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.Register(
            nameof(ActualHeightProperty), typeof(double), typeof(BindableActualSizeBehavior), new PropertyMetadata(null));

        // <summary>
        /// The <see cref="FrameworkElement.ActualWidth"/> that can be bound.
        /// </summary>
        public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.Register(
            nameof(ActualWidthProperty), typeof(double), typeof(BindableActualSizeBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the value for AssociatedObject's ActualHeight.
        /// </summary>
        /// <remarks>
        /// Set this using the header of a ListView or GridView.
        /// </remarks>
        public double ActualHeight => AssociatedObject.ActualHeight;

        /// <summary>
        /// Gets or sets the value for AssociatedObject's ActualWidth.
        /// </summary>
        /// <remarks>
        /// Set this using the header of a ListView or GridView.
        /// </remarks>
        public double ActualWidth => AssociatedObject.ActualHeight;

        /// <summary>
        /// Uses Composition API to get the UIElement and sets an ExpressionAnimation
        /// The ExpressionAnimation uses the height of the UIElement to calculate an opacity value
        /// for the Header as it is scrolling off-screen. The opacity reaches 0 when the Header
        /// is entirely scrolled off.
        /// </summary>
        /// <returns><c>true</c> if the assignment was successfull; otherwise, <c>false</c>.</returns>
        private bool AssignSizeChangedHandler()
        {
            if (AssociatedObject == null)
            {
                return false;
            }

            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;

            return true;
        }

        /// <summary>
        /// Remove the opacity animation from the UIElement.
        /// </summary>
        private void RemoveSizeChangedHandler()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        /// <summary>
        /// Raises the correct event based on how the actual size of the AssociatedObject changed.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private void AssociatedObjectOnSizeChanged(object o, SizeChangedEventArgs args)
        {
            var previousSize = args.PreviousSize;
            var newSize = args.NewSize;

            if (previousSize.Height != newSize.Height)
            {
                OnPropertyChanged(nameof(ActualHeight));
            }

            if (previousSize.Width != newSize.Width)
            {
                OnPropertyChanged(nameof(ActualWidth));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
