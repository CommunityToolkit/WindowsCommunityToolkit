using Microsoft.Xaml.Interactivity;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// This behavior listens to a key down event on the associated <see cref="UIElement"/> when it is loaded and executes an action.
    /// </summary>
    [TypeConstraint(typeof(FrameworkElement))]
    public class KeyDownTriggerBehavior : Trigger<FrameworkElement>
    {

        /// <summary>
        /// The DP to store the <see cref="Key"/> property value.
        /// </summary>
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(VirtualKey),
            typeof(KeyDownTriggerBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the key to listen when the associated object is loaded.
        /// </summary>
        public VirtualKey Key
        {
            get => (VirtualKey)GetValue(KeyProperty);
            set => SetValue(KeyProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            ((FrameworkElement)AssociatedObject).KeyDown += OnAssociatedObjectKeyDown;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            ((FrameworkElement)AssociatedObject).KeyDown -= OnAssociatedObjectKeyDown;
        }

        /// <summary>
        /// Invokes the current actions when the <see cref="Key"/> is pressed.
        /// </summary>
        /// <param name="sender">The source <see cref="UIElement"/> instance.</param>
        /// <param name="keyRoutedEventArgs">The arguments for the event (unused).</param>
        private void OnAssociatedObjectKeyDown(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
        {
            if (keyRoutedEventArgs.Key == Key)
            {
                keyRoutedEventArgs.Handled = true;
                Interaction.ExecuteActions(sender, Actions, keyRoutedEventArgs);
            }
        }
    }
}