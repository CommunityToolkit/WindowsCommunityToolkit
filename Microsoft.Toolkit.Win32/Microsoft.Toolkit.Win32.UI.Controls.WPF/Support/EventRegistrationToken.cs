namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/>
    /// </summary>
    public class EventRegistrationToken
    {
        internal global::System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRegistrationToken"/> class, a
        /// Wpf-enabled wrapper for <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/>
        /// </summary>
        public EventRegistrationToken(System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.EventRegistrationToken"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator EventRegistrationToken(
            System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken args)
        {
            return FromEventRegistrationToken(args);
        }

        /// <summary>
        /// Creates a <see cref="EventRegistrationToken"/> from <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken"/> instance containing the event data.</param>
        /// <returns><see cref="EventRegistrationToken"/></returns>
        public static EventRegistrationToken FromEventRegistrationToken(System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken args)
        {
            return new EventRegistrationToken(args);
        }
    }
}