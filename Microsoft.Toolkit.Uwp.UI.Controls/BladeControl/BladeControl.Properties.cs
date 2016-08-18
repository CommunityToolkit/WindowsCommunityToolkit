using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="Blade"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl
    {
        /// <summary>
        /// Identifies the <see cref="Blades"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BladesProperty = DependencyProperty.Register("Blades", typeof(IList<Blade>), typeof(BladeControl), new PropertyMetadata(new ObservableCollection<Blade>(), PropertyChangedCallback));

        /// <summary>
        /// Identifies the <see cref="ActiveBlades"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActiveBladesProperty = DependencyProperty.Register("ActiveBlades", typeof(IList<Blade>), typeof(BladeControl), new PropertyMetadata(new ObservableCollection<Blade>()));

        /// <summary>
        /// Identifies the <see cref="ToggleBlade"/> attached property.
        /// </summary>
        public static readonly DependencyProperty ToggleBladeProperty = DependencyProperty.RegisterAttached("ToggleBlade", typeof(string), typeof(BladeControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a collection of blades
        /// </summary>
        public IList<Blade> Blades
        {
            get { return (IList<Blade>)GetValue(BladesProperty); }
            set { SetValue(BladesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of visible blades
        /// </summary>
        public IList<Blade> ActiveBlades
        {
            get { return (IList<Blade>)GetValue(ActiveBladesProperty); }
            set { SetValue(ActiveBladesProperty, value); }
        }

        /// <summary>
        /// Sets the ID of a blade to toggle on a UIElement tap
        /// </summary>
        /// <param name="element">The UIElement to toggle the blade</param>
        /// <param name="value">The ID of the blade we want to toggle</param>
        public static void SetToggleBlade(UIElement element, string value)
        {
            element.Tapped -= ToggleBlade;
            element.Tapped += ToggleBlade;

            element.SetValue(ToggleBladeProperty, value);
        }

        /// <summary>
        /// Gets the ID of a blade to toggle on a UIElement tap
        /// </summary>
        /// <param name="element">The UIElement to toggle the blade</param>
        /// <returns>The ID of the blade</returns>
        public static string GetToggleBlade(UIElement element)
        {
            return element.GetValue(ToggleBladeProperty).ToString();
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((BladeControl)dependencyObject).CycleBlades();
        }
    }
}
