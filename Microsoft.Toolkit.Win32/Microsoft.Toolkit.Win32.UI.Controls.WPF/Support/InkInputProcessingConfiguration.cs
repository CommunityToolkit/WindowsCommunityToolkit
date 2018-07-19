namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/>
    /// </summary>
    public class InkInputProcessingConfiguration
    {
        private global::Windows.UI.Input.Inking.InkInputProcessingConfiguration uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkInputProcessingConfiguration"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/>
        /// </summary>
        public InkInputProcessingConfiguration(global::Windows.UI.Input.Inking.InkInputProcessingConfiguration instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration.RightDragAction"/>
        /// </summary>
        public global::Windows.UI.Input.Inking.InkInputRightDragAction RightDragAction
        {
            get => uwpInstance.RightDragAction;
            set => uwpInstance.RightDragAction = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration.Mode"/>
        /// </summary>
        public global::Windows.UI.Input.Inking.InkInputProcessingMode Mode
        {
            get => uwpInstance.Mode;
            set => uwpInstance.Mode = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkInputProcessingConfiguration"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkInputProcessingConfiguration(
            global::Windows.UI.Input.Inking.InkInputProcessingConfiguration args)
        {
            return FromInkInputProcessingConfiguration(args);
        }

        /// <summary>
        /// Creates a <see cref="InkInputProcessingConfiguration"/> from <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkInputProcessingConfiguration"/> instance containing the event data.</param>
        /// <returns><see cref="InkInputProcessingConfiguration"/></returns>
        public static InkInputProcessingConfiguration FromInkInputProcessingConfiguration(global::Windows.UI.Input.Inking.InkInputProcessingConfiguration args)
        {
            return new InkInputProcessingConfiguration(args);
        }
    }
}