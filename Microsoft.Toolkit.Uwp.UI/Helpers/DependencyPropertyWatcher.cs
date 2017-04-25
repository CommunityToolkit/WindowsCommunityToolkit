using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Helpers
{
    /// <summary>
    /// Used to Track Changes of a Dependency Property
    /// </summary>
    /// <typeparam name="T">Value of Dependency Property</typeparam>
    public class DependencyPropertyWatcher<T> : DependencyObject, IDisposable
    {
        /// <summary>
        /// Value of Value Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(DependencyPropertyWatcher<T>),
                new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Called when Property Changes.
        /// </summary>
        public event EventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyWatcher{T}"/> class.
        /// </summary>
        /// <param name="target">Target Dependency Object</param>
        /// <param name="propertyPath">Path of Property</param>
        public DependencyPropertyWatcher(DependencyObject target, string propertyPath)
        {
            this.Target = target;
            BindingOperations.SetBinding(
                this,
                ValueProperty,
                new Binding() { Source = target, Path = new PropertyPath(propertyPath), Mode = BindingMode.OneWay });
        }

        /// <summary>
        /// Gets the target Dependency Object
        /// </summary>
        public DependencyObject Target { get; private set; }

        /// <summary>
        /// Gets the current Value
        /// </summary>
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
        }

        /// <summary>
        /// Called when the Property is updated
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Args</param>
        public static void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            DependencyPropertyWatcher<T> source = (DependencyPropertyWatcher<T>)sender;

            if (source.PropertyChanged != null)
            {
                source.PropertyChanged(source, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clears the Watcher object.
        /// </summary>
        public void Dispose()
        {
            this.ClearValue(ValueProperty);
        }
    }
}