namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.TriggerCollection"/>
    /// </summary>
    public class TriggerCollection
    {
        internal global::Windows.UI.Xaml.TriggerCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.TriggerCollection"/>
        /// </summary>
        public TriggerCollection(global::Windows.UI.Xaml.TriggerCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.TriggerCollection"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.TriggerCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.TriggerCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TriggerCollection(
            global::Windows.UI.Xaml.TriggerCollection args)
        {
            return FromTriggerCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="TriggerCollection"/> from <see cref="global::Windows.UI.Xaml.TriggerCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.TriggerCollection"/> instance containing the event data.</param>
        /// <returns><see cref="TriggerCollection"/></returns>
        public static TriggerCollection FromTriggerCollection(global::Windows.UI.Xaml.TriggerCollection args)
        {
            return new TriggerCollection(args);
        }
    }
}