namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkInputProcessingConfiguration
    {
        private global::Windows.UI.Input.Inking.InkInputProcessingConfiguration uwpInstance;

        public InkInputProcessingConfiguration(global::Windows.UI.Input.Inking.InkInputProcessingConfiguration instance)
        {
            this.uwpInstance = instance;
        }

        public InkInputRightDragAction RightDragAction { get => (InkInputRightDragAction)(int)uwpInstance.RightDragAction; set => uwpInstance.RightDragAction = (global::Windows.UI.Input.Inking.InkInputRightDragAction)(int)value; }

        public InkInputProcessingMode Mode { get => (InkInputProcessingMode)(int)uwpInstance.Mode; set => uwpInstance.Mode = (global::Windows.UI.Input.Inking.InkInputProcessingMode)(int)value; }

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