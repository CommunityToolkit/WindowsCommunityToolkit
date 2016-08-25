using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents methods that will handle various routed events that track property
    /// value changes.
    /// </summary>
    /// <typeparam name="T">The type of the property value where changes in value are reported.</typeparam>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">
    /// The event data. Specific event definitions will constrain System.Windows.RoutedPropertyChangedEventArgs&lt;T&gt;
    /// to a type, with the type parameter of the constraint matching the type parameter
    /// constraint of a delegate implementation.
    /// </param>
    public delegate void RoutedPropertyChangedEventHandler<T>(object sender, RoutedPropertyChangedEventArgs<T> e);

    /// <summary>
    /// Provides data about a change in value to a dependency property as reported
    ///  by particular routed events, including the previous and current value of
    ///  the property that changed.
    /// </summary>
    /// <typeparam name="T">The type of the dependency property that has changed.</typeparam>
    public class RoutedPropertyChangedEventArgs<T> : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedPropertyChangedEventArgs{T}"/> class,
        /// with provided old and new values.
        /// </summary>
        /// <param name="oldValue">The previous value of the property, before the event was raised.</param>
        /// <param name="newValue">The current value of the property at the time of the event.</param>
        public RoutedPropertyChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the new value of a property as reported by a property-changed event.
        /// </summary>
        /// <value>
        /// The generic value. In a practical implementation of the System.Windows.RoutedPropertyChangedEventArgs&lt;T&gt;,
        /// the generic type of this property is replaced with the constrained type of
        /// the implementation.
        /// </value>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the previous value of the property as reported by a property-changed event.
        /// </summary>
        /// <value>
        /// The generic value. In a practical implementation of the System.Windows.RoutedPropertyChangedEventArgs&lt;T&gt;,
        /// the generic type of this property is replaced with the constrained type of
        /// the implementation.
        /// </value>
        public T OldValue { get; private set; }
    }
}
